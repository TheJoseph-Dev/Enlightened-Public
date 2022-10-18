using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyModuleController : MonoBehaviour
{
    public GameObject UIInteractor;

    public LayerMask playerLayerMask;

    public WireProgressController[] wires;

    public Activator activationController;

    [SerializeField]
    private float chargeVelocity = 1f;
    [SerializeField]
    private float unchargeVelocity = 1f;

    private Interactor playerInteractor;
    private PlayerState playerState;
    private const InteractionType interactorType = InteractionType.EnergyModule;

    private bool isActivating = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < wires.Length; i++)
        {
            wires[i].fillProgress = 0.0f;
        }

        playerInteractor = GameObject.Find("Player").GetComponent<Interactor>();
        playerState = GameObject.Find("Player").GetComponent<PlayerState>();
    }

    private bool isCharging = false;
    private int wireCount = 0;
    // Update is called once per frame
    void Update()
    {
        //print(isCharging);
        //print(wireCount);

        HandleState();
        UpdateUI();

        if (wires.Length == 0) {
            //print(isCharging);
            activationController.Set(isCharging);
            return;
        }

        if (isCharging)
        {
            Charge();
        }
        else
        {
            Uncharge();
        }
    }


    private void HandleState()
    {
        bool isPlayerNear = Physics.CheckSphere(transform.position, 3f, playerLayerMask);
        bool isParticle = playerState.isParticle;

        if (isPlayerNear && isParticle)
        {
            HandleKeyPressed();
        }
        else
        {
            isCharging = false;
        }
    }

    private void UpdateUI()
    {
        // UI Interactor
        bool isPlayerNear = Physics.CheckSphere(transform.position, 3f, playerLayerMask);
        if (isPlayerNear)
        {
            var interactor = UIInteractor.GetComponent<SpriteRenderer>();
            var selectInteractor = UIInteractor.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

            float alpha = Mathf.Lerp(0.0f, 1.0f, 5) * Time.deltaTime;

            if (interactor.color.a < 1)
                interactor.color = new Color(255, 255, 255, interactor.color.a + alpha);

            if (selectInteractor.color.a < 1 && isCharging)
                selectInteractor.color = new Color(selectInteractor.color.r, selectInteractor.color.g, selectInteractor.color.b, selectInteractor.color.a + (2 * alpha));

            if (selectInteractor.color.a > 0 && !isCharging)
                selectInteractor.color = new Color(selectInteractor.color.r, selectInteractor.color.g, selectInteractor.color.b, selectInteractor.color.a - (2 * alpha));

        }
        else
        {
            var interactor = UIInteractor.GetComponent<SpriteRenderer>();
            var selectInteractor = UIInteractor.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

            float alpha = Mathf.Lerp(0.0f, 1.0f, 5) * Time.deltaTime;

            if (interactor.color.a > 0)
                interactor.color = new Color(255, 255, 255, interactor.color.a - alpha);
            
            if (selectInteractor.color.a > 0)
                selectInteractor.color = new Color(selectInteractor.color.r, selectInteractor.color.g, selectInteractor.color.b, selectInteractor.color.a - alpha);
        }
    }

    void Charge()
    {
        if (wires[0].fillProgress <= 1.0f) { this.wireCount = 0; }
        int index = wireCount;
        //print(index);
        if (wires[index].fillProgress >= 1.0f && wires[index].emptyRate <= 0.0f) {
            if (wireCount != wires.Length - 1) { wireCount++; }
            else if (wireCount == wires.Length - 1 && !isActivating) { activationController.Set(true); isActivating = true; }

            return;
        }

        float velocity = chargeVelocity;

        WireProgressController wire = wires[index];
        wire.emptyRate = -0.1f;
        float progress = Mathf.Lerp(wire.fillProgress, 1.0f, 10) * Time.deltaTime * velocity;
        wires[index].fillProgress += progress;

        if (wires[index].fillProgress >= 1.0f && wireCount != wires.Length - 1) { wireCount++; }
    }

    //TODO: Uncharge remaining wires while charging others
    void Uncharge()
    {
        if (wires[0].fillProgress >= 1.0f) { this.wireCount = 0; }
        int index = wireCount;
        if (wires[index].fillProgress <= 0.0f) { return; }

        float velocity = unchargeVelocity;

        WireProgressController wire = wires[index];
        float progress = Mathf.Lerp(wire.emptyRate, 1.0f, 10) * Time.deltaTime * velocity;
        wires[index].emptyRate += progress;

        if (wires[index].emptyRate >= wires[index].fillProgress && wireCount != wires.Length - 1) { wireCount++; }
        if (wires[index].emptyRate >= wires[index].fillProgress) { wire.fillProgress = -0.01f; wire.emptyRate = -0.1f; }


        if (wires[index].emptyRate >= 0.9f && wireCount == wires.Length - 1 && isActivating) { activationController.Set(false); isActivating = false; }
    }


    private void HandleKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isCharging = !isCharging; // Swap
            playerInteractor.interaction = isCharging ? new InteractionData(InteractionType.EnergyModule, gameObject) : new InteractionData();

        }

    }

    /*private void OnCollisionEnter(Collision collision)
    {

    }


    private void OnCollisionExit(Collision collision)
    {

    }*/
}
