using UnityEngine;

public class Button : MonoBehaviour
{

    [SerializeField] Transform bottomPosition;
    [SerializeField] Transform topPosition;
    [SerializeField] GameObject buttonSocket;
    [SerializeField] int[] triggerIds;

    [SerializeField] bool isPressurePlate;

    [SerializeField] Mechanism[] triggerMechs;
    readonly float holdTime = 1f;
    float holdFor;
    

    Rigidbody rb;
    void Start()
    {
        GetMechs();
        holdFor = 0f;
        if (isPressurePlate)
        {
            rb = buttonSocket.GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }
    void Update()
    {
        holdFor -= Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (isPressurePlate)
        {
            //buttonSocket.transform.localRotation = Quaternion.identity;
            if (buttonSocket.transform.localPosition.z < topPosition.localPosition.z)
            {
                rb.AddForce(0f, 1f, 0f);
                TriggerMechs();
            }
            else
            {
                //rb.velocity = Vector3.zero;
                buttonSocket.transform.position = topPosition.position;
                UnTriggerMechs();
            }
            buttonSocket.transform.localPosition = new Vector3(0f, 0f, buttonSocket.transform.localPosition.z);
        }
        else
        {
            if (holdFor > 0f)
            {
                buttonSocket.transform.position = Vector3.Lerp(buttonSocket.transform.position, bottomPosition.position, 0.5f);
            }
            else
            {
                buttonSocket.transform.position = Vector3.Lerp(buttonSocket.transform.position, topPosition.position, 0.5f);
            }
        }
    }
    void TriggerMechs()
    {

        foreach (Mechanism mech in triggerMechs)
        {
            mech.Trigger();
        }
    }
    void UnTriggerMechs()
    {
        foreach (Mechanism mech in triggerMechs)
        {
            mech.UnTrigger();
        }
    }
    void GetMechs()
    {
        triggerMechs = new Mechanism[triggerIds.Length];
        Mechanism[] mechArray = FindObjectsOfType(typeof(Mechanism)) as Mechanism[];
        for (int i = 0; i < triggerIds.Length; i++)
        {
            int id = triggerIds[i];
            foreach (Mechanism mech in mechArray)
            {
                if (id == mech.id) triggerMechs[i] = mech;
            }
        }
    }
    public void Click()
    {
        holdFor = holdTime;
    }
}
