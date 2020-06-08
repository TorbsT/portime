using UnityEngine;

public class Actor : MonoBehaviour
{
    public Transform TorsoSocket;
    public Transform HeadSocket;
    public Transform CameraSocket;
    public Transform groundCheck;
    public LayerMask groundMask;

    public bool clampVerticalRotation = true;
    public float groundDistance = 0.4f;
    public float mouseSensitivity = 2f;
    public float mouseSensitivityRatio = 0.3f;
    public float movementSpeed = 1f;
    public float jumpForce = 20f;
    //public float minimumX = -9F;
    //public float maximumX = 9F;

    public float mouseX = 0;
    public float mouseY = 0f;
    public float moveSide = 0f;
    public float moveForward = 0f;
    public bool jump = false;
    public bool grab = false;
    public bool drop = false;

    public float viewRange = 60.0f;

    // for debug of input-ratio
    float inputCounter;
    float physicalCounter;
    float currentPhysicalValue;
    float lastPhysicalValue;

    Animator animator;
    Rigidbody rb;
    TimeBody timeBody;
    SelectionManager selectionManager;
    bool isGrounded;
    void Start()
    {
        inputCounter = 0;
        physicalCounter = 0;
        currentPhysicalValue = 0f;
        lastPhysicalValue = currentPhysicalValue;
        animator = GetComponent<Animator>();
        timeBody = GetComponent<TimeBody>();
        selectionManager = GetComponent<SelectionManager>();
        rb = GetComponent<Rigidbody>();
    }
    public void UpdateAsShadow()
    {
        mouseX = timeBody.shadowMouseX/4;  // /4
        mouseY = timeBody.shadowMouseY;
        moveSide = timeBody.shadowMoveSide/2;  // /2
        moveForward = timeBody.shadowMoveForward/2;  // /2
        jump = timeBody.shadowJump;
        grab = timeBody.shadowGrab;
        drop = timeBody.shadowDrop;

        PerformPhysics();
    }
    public void UpdateAsPlayer()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        moveSide = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        jump = Input.GetKey("space");

        grab = Input.GetKey("e");  // since it's fixedUpdate it's fucked with getkeydown
        drop = Input.GetKey("q");

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

        transform.localRotation *= Quaternion.Euler(0f, mouseX, 0f);
        TorsoSocket.localRotation *= Quaternion.Euler(0f, mouseY, 0f);
        if (clampVerticalRotation)
            TorsoSocket.transform.localEulerAngles = ClampVerticalRotation(TorsoSocket.transform.localEulerAngles);

        selectionManager.ItemSelection(grab, drop);

        animator.SetFloat("Velocity", rb.velocity.sqrMagnitude);
    }
    void GoodMethod()
    {
        if (moveSide * moveForward == 0)
            rb.AddForce(10f * transform.forward * (moveSide + moveForward));
        else
            rb.AddForce(10f * transform.forward * (moveSide + moveForward) / 2);
        if (jump && isGrounded)
            rb.AddForce(10f * transform.up * jumpForce);
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
    Vector3 ClampVerticalRotation(Vector3 originalRotation)
    {
        Vector3 resultVector = originalRotation;

        if (originalRotation.y > viewRange && originalRotation.y < 180f)
        {
            //Debug.Log(originalRotation.x);
            resultVector.y = viewRange;
        }
        else if (originalRotation.y < 360f - viewRange && originalRotation.y > 180f)
        {
            //Debug.Log(originalRotation.x);
            resultVector.y = -viewRange;
        }

        return resultVector;
    }
}
