using UnityEngine;

public class Button : MonoBehaviour
{

    [SerializeField] Transform bottomPosition;
    [SerializeField] Transform topPosition;
    [SerializeField] GameObject buttonSocket;
    [SerializeField] GameObject[] triggerMechs;


    [SerializeField] bool isPressurePlate;

    readonly float holdTime = 1f;
    float holdFor;

    Rigidbody rb;
    void Start()
    {
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
                rb.AddRelativeForce(0f, 0f, 1f);
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
        foreach (GameObject mech in triggerMechs)
        {
            mech.GetComponent<Mechanism>().Trigger();
        }
    }
    void UnTriggerMechs()
    {
        foreach (GameObject mech in triggerMechs)
        {
            mech.GetComponent<Mechanism>().UnTrigger();
        }
    }
    public void Click()
    {
        holdFor = holdTime;
    }
}
