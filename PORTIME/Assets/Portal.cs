using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject exitActor;
    [SerializeField] Transform exitCamera;
    [SerializeField] GameObject plane;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject collider = collision.gameObject;
        collider.transform.position = exitActor.transform.position;
        //collider.transform.Rotate(0f, exitActor.transform.rotation.x-collider.transform.rotation.y, 0f);
    }
}
