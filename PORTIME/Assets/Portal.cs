using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject exit;
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
        collider.transform.position = exit.transform.position;
        collider.transform.Rotate(0f, exit.transform.rotation.x-collider.transform.rotation.y, 0f);
    }
}
