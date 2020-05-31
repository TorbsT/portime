using UnityEngine;

public class Actor : MonoBehaviour
{
    public GameObject AllRotator;
    public GameObject CameraPlaceholder;
    public Transform groundCheck;
    public LayerMask groundMask;

    public bool clampVerticalRotation = true;
    public float groundDistance = 0.4f;
    public float mouseSensitivity = 2f;
    public float mouseSensitivityRatio = 0.3f;
    public float movementSpeed = 1f;
    public float jumpForce = 20f;
    public float minimumX = -90F;
    public float maximumX = 90F;

    public float mouseX = 0f;
    public float mouseY = 0f;
    public float moveSide = 0f;
    public float moveForward = 0f;
    public bool jump = false;

    Rigidbody rb;
    TimeBody timeBody;
    bool isGrounded;
    void Start()
    {
        timeBody = gameObject.GetComponent<TimeBody>();
        rb = GetComponent<Rigidbody>();
    }
    public void UpdateAsShadow()
    {
        mouseX = timeBody.shadowMouseX/4;
        mouseY = timeBody.shadowMouseY;
        moveSide = timeBody.shadowMoveSide/2;
        moveForward = timeBody.shadowMoveForward/2;
        jump = timeBody.shadowJump;

        PerformPhysics();
    }
    public void UpdateAsPlayer()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        moveSide = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        jump = Input.GetKey("space");

        PerformPhysics();
    }
    void PerformPhysics()
    {
        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity * mouseSensitivityRatio;

        moveSide *= movementSpeed;
        moveForward *= movementSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        BadMethod();

        transform.Rotate(Vector3.up, mouseX, Space.Self);
        AllRotator.transform.localRotation *= Quaternion.Euler(-mouseY, 0f, 0f);
        if (clampVerticalRotation)
            AllRotator.transform.localRotation = ClampRotationAroundXAxis(AllRotator.transform.localRotation);
    }
    void BadMethod()
    {
        Vector3 relVel = transform.InverseTransformDirection(rb.velocity);
        relVel.x = moveSide * movementSpeed;
        relVel.z = moveForward * movementSpeed;
        if (jump && isGrounded)
            relVel.y = jumpForce;
        rb.velocity = transform.TransformDirection(relVel);
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
}
