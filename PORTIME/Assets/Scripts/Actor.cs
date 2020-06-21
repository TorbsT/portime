using UnityEngine;

public class Actor : MonoBehaviour
{
    public Transform blockRotator;
    public Transform TorsoSocket;
    public Transform HeadSocket;
    public Transform CameraSocket;
    public Transform groundCheck;
    public LayerMask groundMask;
    //[SerializeField] LayerMask physObjMask;

    public bool rotateTorso = false;
    public bool clampVerticalRotation = true;
    public bool isPlayer = false;
    public float groundDistance = 0.4f;
    public float mouseSensitivity = 2f;
    public float mouseSensitivityRatio = 0.3f;
    public float movementSpeed = 1f;
    public float jumpForce = 20f;


    public int id = 0;

    public float mouseX = 0;
    public float mouseY = 0f;
    public int moveSide = 0;
    public int moveForward = 0;
    public bool jump = false;
    public bool grab = false;
    public bool drop = false;
    public bool rotate = false;
    public bool shift = false;

    
    float inputMouseX;
    float inputMouseY;
    int inputMoveSide;
    int inputMoveForward;
    bool inputJump;
    bool inputGrab;
    bool inputDrop;
    bool inputRotate;
    bool inputShift;

    bool isOnPhysObj = false;
    GameObject physObj;

    public float viewRange = 60.0f;


    Animator animator;
    Rigidbody rb;
    TimeBody timeBody;
    SelectionManager selectionManager;
    GameMaster gameMaster;
    bool isGrounded;
    void Awake()
    {
        animator = GetComponent<Animator>();
        timeBody = GetComponent<TimeBody>();
        selectionManager = GetComponent<SelectionManager>();
        rb = GetComponent<Rigidbody>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    void Update()
    {
        if (isPlayer)
            InputAsPlayer();
    }

    void InputAsPlayer()
    {
        inputMouseX = Input.GetAxis("Mouse X");
        inputMouseY = Input.GetAxis("Mouse Y");

        inputMoveSide = (int)Input.GetAxisRaw("Horizontal");
        inputMoveForward = (int)Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown("space")) inputJump = true;
        if (Input.GetKeyDown("e")) inputGrab = true;
        if (Input.GetKeyDown("q")) inputDrop = true;
        if (Input.GetKeyDown("f")) inputRotate = true;
        if (Input.GetKey(KeyCode.LeftShift)) inputShift = true;

        if (Input.GetKeyDown("r")) gameMaster.TravelBack();
    }
    public void UpdateActor()
    {
        rotate = inputRotate;
        mouseX = inputMouseX;
        mouseY = inputMouseY;
        moveSide = inputMoveSide;
        moveForward = inputMoveForward;
        jump = inputJump;
        grab = inputGrab;
        drop = inputDrop;
        shift = inputShift;

        inputMouseX = 0f;
        inputMouseY = 0f;
        inputMoveSide = 0;
        inputMoveForward = 0;
        inputJump = false;
        inputGrab = false;
        inputDrop = false;
        inputRotate = false;
        inputShift = false;
        PerformPhysics();
        selectionManager.ObjectInteraction(grab, drop);
        Animate();
    }
    public void UpdateShadow(ActorFrame actorFrame)  // used for animation
    {
        rotate = actorFrame.rotate;
        mouseX = actorFrame.mouseX;
        mouseY = actorFrame.mouseY;
        moveSide = actorFrame.moveSide;
        moveForward = actorFrame.moveForward;
        jump = actorFrame.jump;
        grab = actorFrame.grab;
        drop = actorFrame.drop;
        shift = actorFrame.shift;
        PerformPhysics();
        selectionManager.ObjectInteraction(grab, drop);
        Animate();
    }
    void PerformPhysics()
    {
        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity * mouseSensitivityRatio;

        isOnPhysObj = false;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (!isGrounded)
        {
            Collider[] footColliders = Physics.OverlapSphere(groundCheck.position, groundDistance);
            foreach (Collider footCollider in footColliders)
            {
                if (footCollider.gameObject.layer != 9) continue;
                if (footCollider.gameObject.GetComponent<Rigidbody>() == null)
                {
                    Debug.LogError("WHAAAAAATTTTTT");
                    continue;
                }
                Debug.Log("BRRUUUUH");
                isGrounded = true;
                if (footCollider.gameObject != selectionManager.grabbedBlock) continue;
                isGrounded = true;
                isOnPhysObj = true;
                physObj = footCollider.gameObject;
            }

        }
        BadMethod();

        transform.localRotation *= Quaternion.Euler(0f, mouseX, 0f);
        if (rotateTorso)
        {
            TorsoSocket.localRotation *= Quaternion.Euler(0f, mouseY, 0f);
            if (clampVerticalRotation)
                TorsoSocket.transform.localEulerAngles = ClampVerticalRotation(TorsoSocket.transform.localEulerAngles);
        }
        else
        {
            HeadSocket.Rotate(0f, mouseY, 0f, Space.Self);
            //if (clampVerticalRotation)
            //    HeadSocket.transform.localEulerAngles = ClampVerticalRotation(TorsoSocket.transform.localEulerAngles);
        }
        if (rotate)
        {
            if (shift)
                blockRotator.Rotate(0f, 0f, 90f);
            else
                blockRotator.Rotate(0f, 90f, 0f);
        }
    }
    public void Animate()
    {
        if (jump) animator.SetTrigger("JumpTrigger");
        animator.SetBool("HoldBlock", (selectionManager.grabbedBlock != null));
        animator.SetFloat("Velocity", rb.velocity.sqrMagnitude);
    }
    void GoodMethod()
    {
        if (moveSide * moveForward == 0)
            rb.AddForce(10f * transform.forward * (moveSide + moveForward)*movementSpeed);
        else
            rb.AddForce(10f * transform.forward * (moveSide + moveForward)*movementSpeed / 2);
        if (jump && isGrounded)
            rb.AddForce(10f * transform.up * jumpForce);
    }
    void BadMethod()
    {
        Vector3 relVel = transform.InverseTransformDirection(rb.velocity);
        relVel.x = moveSide * movementSpeed;
        relVel.z = moveForward * movementSpeed;
        if (isGrounded && isOnPhysObj)
        {
            physObj.GetComponent<Block>().JumpedOn();
        }
        if (jump && isGrounded)
        {
            relVel.y = jumpForce;
        }
        rb.velocity = transform.TransformDirection(relVel);
    }
    Vector3 ClampVerticalRotation(Vector3 originalRotation)
    {
        Vector3 resultVector = originalRotation;

        if (originalRotation.y > viewRange && originalRotation.y < 180f)
        {
            resultVector.y = viewRange;
        }
        else if (originalRotation.y < 360f - viewRange && originalRotation.y > 180f)
        {
            resultVector.y = -viewRange;
        }

        return resultVector;
    }
    public Transform GetCameraSocket()
    {
        Transform cameraSocket = CameraSocket;
        return cameraSocket;
    }
}
