using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineBrain cBrain;
    public CinemachineFreeLook cinemachineFreeLook;

    public GameObject target;
    public PortalCutscene cutscene;

    TeleportController teleportController;
    PlayerMovement moveController;
    Interactor interactor;


    // Start is called before the first frame update
    void Start()
    {
        cBrain = GetComponent<CinemachineBrain>();
        cinemachineFreeLook = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        teleportController = target.GetComponent<TeleportController>();
        moveController = target.GetComponent<PlayerMovement>();
        interactor = target.GetComponent<Interactor>();

        lookAtObject = new GameObject("CameraLookAt");
    }

    // An object to look at
    private GameObject lookAtObject;

    // Update is called once per frame
    void Update() // Changing from FixedUpdate to Update + Using player rigidbody at FixedUpdate solved jittering :D
    {
        if (!StopOnInteraction())
            CameraModes();
    }


    private void CameraModes()
    {
        if (cBrain == null) { print("Deu Ruim"); return; }
        if (teleportController == null) { print("Deu Ruim 2"); return; }

        // GameObject.Find("Player")
        lookAtObject.transform.position = moveController.lastPos + (Vector3.up / 2);

        cinemachineFreeLook.m_XAxis.m_InvertInput = !GameSettings.invertMouse;
        if (teleportController.isTeleporting)
        {

            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 100 * GameSettings.mouseSensibility;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 2 * GameSettings.mouseSensibility;
            cinemachineFreeLook.m_YAxis.m_InputAxisName = "Vertical";
            cinemachineFreeLook.m_XAxis.m_InputAxisName = "Horizontal";



            //bool didInput = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

            cinemachineFreeLook.LookAt = lookAtObject.transform;
            cinemachineFreeLook.Follow = lookAtObject.transform; //!didInput ? null : GameObject.Find("Player").transform;

            //Destroy(lookAtObject);
        }
        else
        {
            //cBrain.enabled = true;
            cinemachineFreeLook.LookAt = lookAtObject.transform;
            cinemachineFreeLook.Follow = lookAtObject.transform;

            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 250 * GameSettings.mouseSensibility;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 2 * GameSettings.mouseSensibility;
            cinemachineFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
            cinemachineFreeLook.m_XAxis.m_InputAxisName = "Mouse X";
        }
        //{ cBrain.enabled = false; } else { cBrain.enabled = true; }

    }

    private bool hasHandledCutscene = false;
    private bool StopOnInteraction() 
    {
        bool shouldPlayCutscene = (interactor.interaction.type == InteractionType.PortalCutscene && !hasHandledCutscene);

        if (shouldPlayCutscene)
        {
            cBrain.enabled = false;

            //print("Trigge");
            cutscene.Trigger();

            hasHandledCutscene = true;

            return true;
        }


        return false;
    }

}
