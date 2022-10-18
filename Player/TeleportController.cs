using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(LineRenderer))]
public class TeleportController : MonoBehaviour
{

    [SerializeField]
    private GameObject indicatorObject;

    private GameObject indicatorObjectCopy;
    private GameObject currentIndicatorObject;


    //public Transform camera; // May not use
    //public Volume pVolume;

    public PlayerMovement playerMovement;
    public PlayerState playerState;
    public Rigidbody playerRig;

    public LineRenderer lineRenderer;
    //public GameObject effects;

    public List<string> unanbleTags;

    public SFXHandler sfxHandler;

    //public bool playerMode = .Particle;
    public bool isTeleporting { get; private set; }

    private List<Vector3> pathList = new List<Vector3>();


    private KeyCode key = KeyCode.T;

    // Start is called before the first frame update
    void Start()
    {
        isTeleporting = false;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyPress();

        if (currentIndicatorObject != null)
        {
            MoveIndicatorObject();
            Teleport();
        }
    }

    public IEnumerator Teleport(Vector3 posA, Vector3 posB, float delay, float tpTime = 0.5f)
    {
        yield return new WaitForSeconds(delay);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);

        //Vector3[] positions = { posB };
        StartCoroutine(MoveWithDelay(posA, posB, tpTime));
    }

    private void Teleport()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentIndicatorObject.activeSelf == false)
            {
                currentIndicatorObject = null;
                lineRenderer.enabled = false;
                isTeleporting = false;
                return;
            }


            Vector3 origin = transform.position;
            Vector3 targetPosition = currentIndicatorObject.transform.position;
            float delay = 0.4f;

            indicatorObjectCopy = Instantiate(currentIndicatorObject);

            //pathList.Reverse();
            print((origin, targetPosition));
            StartCoroutine(MovesWithDelay(pathList.ToArray(), delay, false));

            //VolumeEffect(pathList.Count);

            //StartCoroutine(MoveWithDelay(origin, targetPosition, delay));

            Destroy(currentIndicatorObject);
            currentIndicatorObject = null;

            playerState.HandleTeleport();

            //VolumeEffect(pathList.Count);
            isTeleporting = false;

            print("Teleporting");
        }
    }



    private IEnumerator MoveWithDelay(Vector3 posA, Vector3 posB, float delay)
    {
        //float waitDuration = 0.2f;

        float counter = 0f;
        while (counter < delay)
        {
            Vector3 move = Vector3.Lerp(posA, posB, counter / delay);
            transform.position = move;
            //print(move);
            counter += Time.deltaTime;

            lineRenderer.SetPosition(0, move);

            yield return null;
        }

        transform.position = posB;


        // Line Renderer
        List<Vector3> lineRendererPositions = new List<Vector3>();

        for (int lineCount = 0; lineCount < lineRenderer.positionCount; lineCount++)
        {
            lineRendererPositions.Add(lineRenderer.GetPosition(lineCount));
        }

        lineRendererPositions.RemoveAt(0);
        lineRenderer.SetPositions(lineRendererPositions.ToArray());
        lineRenderer.positionCount -= 1;


        yield return null;
    }

    private IEnumerator MovesWithDelay(Vector3[] positions, float delay, bool distortion = true)
    {
        if (positions.Length <= 0) { yield return null; }
        //float waitDuration = 0.2f;

        int sfxIndex = 0;

        var posA = transform.position;
        for(int i = 0; i < positions.Length; i++)
        {
            var posB = positions[i];

            if (distortion) VolumeEffect(positions.Length);

            yield return MoveWithDelay(posA, posB, delay);

            if (distortion) VolumeEffect(positions.Length);
            //VolumeEffect(positions.Length);
            posA = positions[i];
            playerRig.velocity = Vector3.zero;

            if (sfxIndex > 2) sfxIndex = 0;
            sfxHandler.Play(sfxHandler.clips[(int)SFXType.TP1 + sfxIndex]);
            sfxIndex++;
        }

        lineRenderer.enabled = false;


        print(indicatorObjectCopy);
        Destroy(indicatorObjectCopy);
        indicatorObjectCopy = null;

        // Effect
        var effects = this.gameObject.transform.GetChild(0);
        GameObject effect = effects.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        //print(effect.name);
        effect.SetActive(true);
        var finalEffect = effect.GetComponent<ParticleSystem>();
        finalEffect.Play();
           

        yield return null;
    }

    private float lensIntensity = 0.0f;
    private void VolumeEffect(int times = 1)
    {
        LensDistortion lens;

        Volume pVolume = GameObject.Find("PostProcess").GetComponent<Volume>();
        pVolume.profile.TryGet(out lens);

        const float zoom = 0.8f;

        print(isTeleporting);
        if (!isTeleporting) {

            lensIntensity += zoom;
            lens.intensity.value = lensIntensity;

        }
        else
        {

            lensIntensity += -zoom;
            lens.intensity.value = lensIntensity;

        }

        print(lens.intensity.value);
    }

    private void MoveIndicatorObject()
    {
        pathList.Clear();
        currentIndicatorObject.SetActive(true);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);


        float maxDistance = 8.35f;// + 8f;

        Vector3 lastPPos = playerMovement.lastPos;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit targetHitInfo;
        bool targetHit = Physics.Raycast(ray, out targetHitInfo, maxDistance + 8f);
        

        Vector3 newOrigin = new Vector3(transform.position.x, lastPPos.y + 0.5f, transform.position.z);
        Vector3 newDir = (targetHitInfo.point - newOrigin);

        newDir = targetHit ? newDir : ray.direction;//(ray.GetPoint(maxDistance) - newOrigin);
        //Physics.Linecast(newOrigin, targetHitInfo.point);

        //print(targetHitInfo.point);
        ray = new Ray(newOrigin, newDir);
        Debug.DrawRay(newOrigin, newDir * maxDistance, Color.red);//ray.direction * maxDistance, Color.red);
        //print(ray.GetPoint(maxDistance));

        RaycastHit hitInfo;

        
        bool didHit = Physics.Raycast(ray, out hitInfo, maxDistance);
        //print(hitInfo.point);

        if (didHit)
        {
            // Checks if ray is hitting a valid surface
            if (unanbleTags.Contains( hitInfo.collider.tag )) { return; }

            Vector3 target = hitInfo.point;
            Quaternion indicatorRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

            Material mat = currentIndicatorObject.GetComponent<Renderer>().material;
            Color normalColor = new Color(1.5f, 1.2f, 0.25f);
            mat.SetColor("_MainColor", normalColor * 1.22f);

            if (hitInfo.collider.tag == "Mirror")
            {
                var result = Reflection(hitInfo, ray, maxDistance);
                if (result.Item1 != Vector3.zero)
                {
                    (target, indicatorRotation) = (result.Item1, result.Item2);
                }
                else
                {
                    currentIndicatorObject.SetActive(false);
                    return;
                }
            }
            else
            {
                pathList.Add(target);
            }
            

            currentIndicatorObject.transform.position = target;
            currentIndicatorObject.transform.rotation = indicatorRotation;
        }
        else
        {
            //Throws a down ray and checks if it finds a surface in a range
            
            currentIndicatorObject.transform.position = ray.GetPoint(maxDistance);

            Material mat = currentIndicatorObject.GetComponent<Renderer>().material;
            Color redColor = new Color(1.68f, 0.2f, 0.12f);
            mat.SetColor("_MainColor", redColor * 1.2f); // Red

            /*
            bool hitFloor = false;

            int counter = 0;

            while (hitFloor != true && counter < maxDistance)
            {
                Vector3 pointOrigin = ray.GetPoint(maxDistance - counter);
                Ray downRay = new Ray(pointOrigin, Vector3.down * maxDistance);
                Debug.DrawRay(downRay.origin, downRay.direction, Color.blue);

                RaycastHit hitFloorInfo;

                hitFloor = Physics.Raycast(downRay, out hitFloorInfo);

                if (hitFloor)
                {
                    print("Hitted Floor");
                    pathList.Add(hitFloorInfo.point);

                    currentIndicatorObject.transform.position = hitFloorInfo.point;
                    currentIndicatorObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                }

                counter++;
            }*/
        }

    }

    private (Vector3, Quaternion) Reflection(RaycastHit firstHit, Ray firstRay, float maxDistance)
    {
        Vector3 target = firstHit.point;
        Quaternion indicatorRotation = Quaternion.FromToRotation(Vector3.up, firstHit.normal);

        float maxDistanceReflected = maxDistance * 1.75f;

        print("Hitted a mirror");
        float remainingLength = maxDistance;
        Ray reflectedRay = firstRay;

        RaycastHit reflectedHitInfo = new RaycastHit();

        bool didReflect;

        for (int reflections = 0; reflections < 20; reflections++)
        {
            didReflect = Physics.Raycast(reflectedRay, out reflectedHitInfo, maxDistanceReflected); //remainingLength);

            if (didReflect)
            {

                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, reflectedHitInfo.point);

                if (reflectedHitInfo.collider.tag != "Mirror") { break; }

                Debug.DrawRay(reflectedRay.origin, reflectedRay.direction * maxDistance, Color.green);
                remainingLength -= Vector3.Distance(reflectedRay.origin, reflectedHitInfo.point);
                reflectedRay = new Ray(reflectedHitInfo.point, Vector3.Reflect(reflectedRay.direction, reflectedHitInfo.normal));

                Debug.DrawRay(reflectedHitInfo.point, reflectedRay.direction * maxDistance, Color.magenta);

                
                pathList.Add(reflectedHitInfo.point);
                //pathList.Add(reflectedRay);
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, reflectedRay.origin + reflectedRay.direction * maxDistanceReflected);
            }
        }

        //print(reflectedHitInfo.point);
        pathList.Add(reflectedHitInfo.point);

        target = reflectedHitInfo.point;
        indicatorRotation = Quaternion.FromToRotation(Vector3.up, reflectedHitInfo.normal);

        return (target, indicatorRotation);
        
    }

    private void HandleKeyPress()
    {
        if (playerState.isParticle) {
            if (currentIndicatorObject != null)
                Destroy(currentIndicatorObject);

            lineRenderer.enabled = false;

            isTeleporting = false;

            return;
        }

        if (Input.GetKeyDown(this.key))
        {
            if (currentIndicatorObject == null)
            {
                lineRenderer.enabled = true;
                currentIndicatorObject = Instantiate(indicatorObject);
                isTeleporting = true;
            }
            else
            {
                if (currentIndicatorObject != null)
                    Destroy(currentIndicatorObject);

                lineRenderer.enabled = false;

                isTeleporting = false;
            }
        }


    }
}
