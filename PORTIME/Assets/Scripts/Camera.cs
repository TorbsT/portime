using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject PlayerCamera;
    public Vector3 cameraOffset = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = PlayerCamera.transform.rotation;
        transform.position = PlayerCamera.transform.position;
        transform.localPosition += cameraOffset;
    }
}
