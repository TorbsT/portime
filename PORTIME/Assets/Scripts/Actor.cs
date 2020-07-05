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
    public float maxSpeed = 1f;
    public float jumpForce = 20f;
    [SerializeField] float movementAcceleration = 1f;

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
    PhysicMaterial groundedObjectPMat;
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
        jump = actorFrame.jump;
        grab = actorFrame.grab;
        drop = actorFrame.drop;
        shift = actorFrame.shift;
        if (gameMaster.shadowsOnlyInput)
        {
            mouseX = actorFrame.mouseX;
            mouseY = actorFrame.mouseY;
            moveForward = actorFrame.moveForward;
            moveSide = actorFrame.moveSide;
        }
        PerformPhysics();
        selectionManager.ObjectInteraction(grab, drop);
        Animate();
    }
    void PerformPhysics()
    {
        float standDistance = Mathf.Infinity;
        Collider standCollider = null;
        isOnPhysObj = false;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        groundedObjectPMat = null;

        Collider[] footColliders = Physics.OverlapSphere(groundCheck.position, groundDistance);
        foreach (Collider footCollider in footColliders)
        {
            if (footCollider.gameObject.layer == 10) continue; // Player layer
            Vector3 closestPoint = footCollider.ClosestPoint(groundCheck.position);
            if (Vector3.SqrMagnitude(closestPoint - groundCheck.position) < standDistance)
            {
                standCollider = footCollider;
                standDistance = Vector3.SqrMagnitude(closestPoint - groundCheck.position);
            }
        }

        if (standCollider != null)
        {
            if (standCollider.gameObject.layer == 9 && standCollider.gameObject.GetComponent<Rigidbody>() != null)
            {
                isGrounded = true;
                if (standCollider.gameObject == selectionManager.grabbedBlock)
                {
                    isOnPhysObj = true;
                    physObj = standCollider.gameObject;
                }
            }
            groundedObjectPMat = standCollider.material;
        }

        if (isPlayer || gameMaster.shadowsOnlyInput)
        {
            PreviouslyInBadMethod();
            GoodMethod();
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
        float dynamicFriction = 0f;
        float staticFriction = 0f;
        Vector3 walkDirection = Vector3.zero;
        if (groundedObjectPMat != null)
        {
            dynamicFriction = groundedObjectPMat.dynamicFriction;
            staticFriction = groundedObjectPMat.staticFriction;
            Debug.Log(groundedObjectPMat.name + " " + dynamicFriction);
        }
        walkDirection = transform.forward * moveForward + transform.right * moveSide;
        walkDirection = Vector3.Normalize(walkDirection);

        
        Vector3 horizontalVector = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float horizontalSpeed = Vector3.SqrMagnitude(horizontalVector);
        Vector3 addHorizontalVector = walkDirection * movementAcceleration * (dynamicFriction + 0.5f);
        float newHorizontalSpeed = Vector3.SqrMagnitude(horizontalVector + addHorizontalVector);

        if (horizontalSpeed < maxSpeed || newHorizontalSpeed < horizontalSpeed) rb.AddForce(addHorizontalVector);

        if (walkDirection == Vector3.zero)
        {
            
        }

        if (isGrounded)
        {
            if (isOnPhysObj) physObj.GetComponent<Block>().JumpedOn();
            if (jump) rb.AddForce(10f * transform.up * jumpForce);
        }
    }
    void BadMethod()
    {
        Vector3 relVel = transform.InverseTransformDirection(rb.velocity);
        relVel.x = moveSide * maxSpeed;
        relVel.z = moveForward * maxSpeed;
        if (isGrounded)
        {
            if (isOnPhysObj) physObj.GetComponent<Block>().JumpedOn();
            if (jump)
            {
                relVel.y = jumpForce;
            }
        }
        rb.velocity = transform.TransformDirection(relVel);
    }
    void PreviouslyInBadMethod()
    {
        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity * mouseSensitivityRatio;

        if (isPlayer) transform.localRotation *= Quaternion.Euler(0f, mouseX, 0f);
        if (rotateTorso)
        {
            TorsoSocket.localRotation *= Quaternion.Euler(0f, mouseY, 0f);
            if (clampVerticalRotation)
                TorsoSocket.transform.localEulerAngles = ClampVerticalRotation(TorsoSocket.transform.localEulerAngles);
        }
        else
        {
            if (isPlayer) HeadSocket.Rotate(0f, mouseY, 0f, Space.Self);
            //if (clampVerticalRotation)
            //    HeadSocket.transform.localEulerAngles = ClampVerticalRotation(TorsoSocket.transform.localEulerAngles);
        }
        if (rotate && isPlayer)
        {
            if (shift)
                blockRotator.Rotate(0f, 0f, 90f);
            else
                blockRotator.Rotate(0f, 90f, 0f);
        }
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
