using UnityEngine;

public class Camera : MonoBehaviour
{
    [System.NonSerialized] public Transform playerCamera;
    GameMaster gameMaster;
    public Vector3 cameraOffset = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        playerCamera = gameMaster.GetPlayer().GetComponent<Actor>().GetCameraSocket();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerCamera.rotation;
        transform.position = playerCamera.position;
        transform.localPosition += cameraOffset;
    }
}
