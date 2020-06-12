using UnityEngine;

public class Mechanism : MonoBehaviour
{
    [SerializeField] bool isDoor = false;
    [SerializeField] Vector3 mechBottom = Vector3.zero;
    [SerializeField] Vector3 mechTop = Vector3.zero;
    [SerializeField] float openTime = 0.5f;
    [SerializeField] float closeRelation = 1f;

    Transform mechSocket;
    bool isActive;
    void Start()
    {
        if (isDoor)
        {
            mechSocket = GameObject.Find("DoorSocket").transform;
        }
    }
    void FixedUpdate()
    {
        if (isDoor)
        {
            if (isActive)
            {
                mechSocket.transform.localPosition = Vector3.Lerp(mechSocket.transform.localPosition, mechTop, openTime);
            }
            else
            {
                mechSocket.transform.localPosition = Vector3.Lerp(mechSocket.transform.localPosition, mechBottom, openTime*closeRelation);
            }
        }
    }
    public void Trigger()
    {
        isActive = true;
    }
    public void UnTrigger()
    {
        isActive = false;
    }
}
