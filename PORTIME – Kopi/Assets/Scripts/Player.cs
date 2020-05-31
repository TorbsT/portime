using UnityEngine;

public class Player : MonoBehaviour
{
    
    public GameObject AllRotator;
    public GameObject CameraPlaceholder;
    public Transform groundCheck;
    public LayerMask groundMask;

    public bool clampVerticalRotation = true;
    public float groundDistance = 0.4f;
    public float mouseSensitivity = 2f;
    public float mouseSensitivityRatio = 0.3f;
    public float movementSpeed = 20f;
    public float jumpForce = 20f;
    public float minimumX = -90F;
    public float maximumX = 90F;

    float mouseX = 0f;
    float mouseY = 0f;
    float moveSide = 0f;
    float moveForward = 0f;

    private static bool isRewinding;
    Rigidbody rb;
    bool isGrounded;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //isRewinding = TimeBody.isReplaying;
        if (true)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * mouseSensitivityRatio;
        } else
        {
            mouseX = 0;
            mouseY = 0;
        }

        moveSide = Input.GetAxisRaw("Horizontal") * movementSpeed;
        moveForward = Input.GetAxisRaw("Vertical") * movementSpeed;
    }
    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        BadMethod();

        transform.Rotate(Vector3.up, mouseX, Space.Self);
        AllRotator.transform.localRotation *= Quaternion.Euler(-mouseY, 0f, 0f);
        if (clampVerticalRotation)
            AllRotator.transform.localRotation = ClampRotationAroundXAxis(AllRotator.transform.localRotation);
    }
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = 2f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, minimumX, maximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    void GoodMethod()
    {
        if (moveSide * moveForward == 0)
            rb.AddForce(transform.forward * (moveSide + moveForward));
        else
            rb.AddForce(transform.forward * (moveSide + moveForward) / 2);
    }
    void BadMethod()
    {
        Vector3 relVel = transform.InverseTransformDirection(rb.velocity);
        relVel.x = moveSide * movementSpeed;
        relVel.z = moveForward * movementSpeed;
        if (Input.GetKey("space") && isGrounded)
            relVel.y = jumpForce;
        rb.velocity = transform.TransformDirection(relVel);
    }
}
