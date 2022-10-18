using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;

    //public CharacterController characterController;
    public Transform camera;

    public PlayerState playerState;
    public TeleportController teleportController;

    bool isGrounded;

    public float velocity = 3f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Transform effects;

    public SFXHandler sfxHandler;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        teleportController = GetComponent<TeleportController>();

        lastPos = transform.position;

        effects = this.gameObject.transform.GetChild(0);
        //playerState.Swap();
    }


    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = new Vector3(0, 0, 0);
        PhysicsMovement();
    }
    //private void FixedUpdate() // Caried the movement smoother, but failed at levitation and detecting key inputs

    private Vector3 particleMove = Vector3.zero;
    private bool shouldMove = false;
    private bool shouldJump = false;
    private bool isOnJumpBooster = false;

    private void PhysicsMovement()
    {
        if (jumpTime > jumpDelay && shouldJump) 
        {
            jumpTime = 0f;
        }
        else
        {
            jumpTime += Time.deltaTime;
            this.shouldJump = false;
        }

        if (shouldJump)
        {
            float jumpForce = 2;
            //jumpForce = ( isOnJumpBooster ? jumpForce * 4 : 2 );

            Vector3 velocity = new Vector3();
            const float gravity = -9.81f;

            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);

            rb.velocity = Vector3.zero;
            rb.AddForce(velocity, ForceMode.Impulse);

            this.shouldJump = false;
        }

        if (shouldMove)
            rb.velocity = particleMove;

        shouldMove = false;

        if (isOnJumpBooster)
        {
            float jumpForce = 12;
            Vector3 velocity = new Vector3();
            const float gravity = -9.81f;

            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);

            rb.velocity = Vector3.zero;
            rb.AddForce(velocity, ForceMode.Impulse);
        }
    }

    public Vector3 lastPos { get; private set; }
    private float levitationCounter = 0f;
    private bool isParticle = true;
    private bool isInteracting = false;
    private void Movement()
    {
        if (this.isParticle != playerState.isParticle && !playerState.isParticle)
            sfxHandler.Play(sfxHandler.clips[(int)SFXType.WAVEMODE], volume: 0.15f);

        if (Time.timeScale == 0) sfxHandler.audioSource.Stop();
        //if (Time.timeScale != 0 && !sfxHandler.audioSource.isPlaying) sfxHandler.audioSource.UnPause();

        this.isParticle = playerState.isParticle;

        //bool isInteracting = HandleInteraction();
        //if (isInteracting) { return; }
        //print(isInteracting);

        if (!isParticle)
        {
            rb.useGravity = false;
            WaveMovement();

            //HandleTeleport();
            //if (isTeleporting) { return; }

            return;
        }

        if (levitationCounter != 0f)
        {
            rb.velocity = Vector3.zero;
            transform.position += Vector3.up;
            sfxHandler.Stop();
        }

        // lastPos being resetted in HandleInteraction 
        this.isInteracting = HandleInteraction();
        if (isInteracting) { return; }

        rb.useGravity = true;

        lastPos = transform.position;
        levitationCounter = 0f;

        ParticleMovement();
    }

    private void ParticleMovement()
    {
        DefineJump();
        //DefineGravity();

        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xInput, 0.0f, zInput).normalized;

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;// + camera.eulerAngles.y;

        targetAngle += (teleportController.isTeleporting ? 0 : camera.eulerAngles.y);

        transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);


        //shouldMove = false;

        // Avoid running/climbing against collider
        if (IsAgainstCollider()) { return; }

        if (moveDirection.magnitude < 0.1f) { return; }

        Vector3 mDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        Vector3 smoothDir = Vector3.Lerp(transform.position, mDir.normalized, 1.5f);

        Vector3 move = (smoothDir.normalized * velocity);

        Vector3 rbMove = new Vector3(move.x, rb.velocity.y + move.y, move.z);
        //print(Mathf.Abs(rb.velocity.magnitude)); print(Mathf.Abs(Vector3.one.magnitude / 2f));

        // When Idle, First Moves are slower
        if ( Mathf.Abs(rb.velocity.magnitude) < Mathf.Abs(Vector3.one.magnitude / 1.5f) ) { rbMove /= 2f; }

        Vector3 multiplier = new Vector3(0.92f, 1, 0.92f);
        Vector3 rbMoveM = new Vector3(rbMove.x * multiplier.x, rbMove.y * multiplier.y, rbMove.z * multiplier.z);
        this.particleMove = rbMove;
        shouldMove = true;
       
        //Debug.DrawRay(transform.position, smoothDir * 10f, Color.blue);
        //Debug.DrawRay(transform.position, move * 10f, Color.green);
    }

    private void WaveMovement()
    {
        Levitation(Vector3.up);
    }

    private float jumpTime = 0f;
    private const float jumpDelay = 1.2f;
    private void DefineJump() //Player Jump
    {
        
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        // Because fixedUpdate is "slower" than Update, shouldJump must be true until it goes through fixedUpdate.
        if (this.shouldJump == false)
            this.shouldJump = (Input.GetButtonDown("Jump") && isGrounded);
        
        if (shouldJump) sfxHandler.Play(sfxHandler.clips[(int)SFXType.JUMP]);
    }

    private bool IsAgainstCollider()
    {
        Ray didHitObjRay = new Ray(transform.position, transform.rotation * Vector3.forward);
        RaycastHit h;
        bool didHitObj = Physics.Raycast(didHitObjRay, out h, 1);

        return
        ( didHitObj /*&& h.collider.gameObject.layer != Mathf.Log(groundMask.value, 2)*/ && h.collider.tag != "GroundLevel");
    }

    private void Levitation(Vector3 direction)
    {
        float levitateValue = Mathf.Abs(Mathf.Sin(levitationCounter)) / 2;
        Vector3 levitatePos = lastPos + (direction * levitateValue);

        Vector3 levitationPos = Vector3.Lerp(lastPos, levitatePos + (direction * 0.2f), 2);
        transform.position = levitationPos;

        levitationCounter += 1f * Time.deltaTime;
    }


    private bool hasHandledOnce = false;
    private ParticleSystem interactionEffect = null;
    // Returns true if it's handling an interaction
    private bool HandleInteraction()
    {
        Interactor pInteractor = this.gameObject.GetComponent<Interactor>();

        if (pInteractor.isInteracting)
        {
            switch (pInteractor.interaction.type)
            {
                case InteractionType.EnergyModule:

                    if (!playerState.isParticle) { return false; }

                    // Effect
                    GameObject effect = effects.GetChild(1).GetChild(1).gameObject;
                    effect.SetActive(true);
                    interactionEffect = effect.GetComponent<ParticleSystem>();

                    if (!hasHandledOnce)
                    {
                        Vector3 modulePos = pInteractor.interaction.interactiveObject.transform.position + Vector3.up;
                        StartCoroutine(MoveWithDelay(transform.position, modulePos, 1));
                        interactionEffect.Play();

                        AudioSource aSrc = pInteractor.interaction.interactiveObject.GetComponent<AudioSource>();
                        sfxHandler.Play(aSrc, sfxHandler.clips[(int)SFXType.SOLARMOD]);
                        hasHandledOnce = true;
                    }

                    Levitation(Vector3.up);

                    return true;

                case InteractionType.PortalCutscene:

                    Vector3 targetPosition = new Vector3(0, 6.15f, 72) + (Vector3.up * 5);
                    Vector3 portalPosition = new Vector3(0, 6.15f, 82) + (Vector3.up * 5);
                    if (!hasHandledOnce)
                    {
                        
                        rb.useGravity = false;
                        
                        StartCoroutine(MoveWithDelay(transform.position, targetPosition, 3));

                        rb.velocity = Vector3.zero;

                        StartCoroutine( teleportController.Teleport(targetPosition, portalPosition, 12, 2.5f) );

                        hasHandledOnce = true;
                    }


                    if (!playerState.isParticle) playerState.Swap();
                    if (transform.position.magnitude + 0.2f >= targetPosition.magnitude) lastPos = targetPosition;
                    if (transform.position.magnitude + 0.2f >= portalPosition.magnitude) lastPos = portalPosition;

                    Levitation(Vector3.up);

                    return true;
            }
        }
        else
        {
            // Returns to normal state
            hasHandledOnce = false;

            lastPos = transform.position;
            levitationCounter = 0f;

            if (interactionEffect != null)
                interactionEffect.Stop();

        }

        return false;
    }


    //
    private IEnumerator MoveWithDelay(Vector3 posA, Vector3 posB, float delay)
    {

        float counter = 0f;
        while (counter < delay)
        {
            Vector3 move = Vector3.Lerp(posA, posB, counter / delay);
            transform.position = move;

            counter += Time.deltaTime;


            yield return null;
        }

        transform.position = posB;

        lastPos = transform.position;
        levitationCounter = 0f;

        yield return null;
    }

    private bool isRespawning = false;
    private IEnumerator Respawn()
    {
        Vignette vignetteEffect;

        Volume v = GameObject.Find("PostProcess").GetComponent<Volume>();
        v.profile.TryGet(out vignetteEffect);

        this.isRespawning = true;

        while (vignetteEffect.intensity.value < 0.95f)
        {
            float intensityValue = Mathf.Lerp(0, 1, 2);
            vignetteEffect.intensity.value += intensityValue * Time.deltaTime;
            if (!playerState.isParticle) playerState.Swap();
            yield return null;
        }

        // Move Player
        Transform spawnpointPos = GameObject.Find("Spawnpoint").transform;
        rb.velocity = Vector3.zero;
        transform.position = spawnpointPos.position;

        while (vignetteEffect.intensity.value > 0.01f)
        {
            float intensityValue = Mathf.Lerp(0, 1, 2);
            vignetteEffect.intensity.value -= intensityValue * Time.deltaTime;
            yield return null;
        }

        this.isRespawning = false;

        // Play Effect
        GameObject effect = effects.GetChild(4).gameObject;
        var particle = effect.GetComponent<ParticleSystem>();
        particle.Play();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("JumpBooster") && isOnJumpBooster == false) isOnJumpBooster = true;// else isOnJumpBooster = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("JumpBooster") && isOnJumpBooster == true) isOnJumpBooster = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool didHitVoid = other.gameObject.CompareTag("Void");

        if (didHitVoid && !isRespawning)
        {
            // Fade screen Effect
            StartCoroutine(Respawn());

            
        }

        bool didHitPortalTrigger = (other.gameObject.name == "PortalTrigger");

        if (didHitPortalTrigger)
        {
            Interactor pInteractor = transform.GetComponent<Interactor>();
            pInteractor.interaction = new InteractionData(InteractionType.PortalCutscene);
        }
    }
}

