using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject player;
    public GameObject grabber;

    SelectionManager interactorScript;
    Material initialMaterial;
    Renderer renderer;
    bool isPlayer;
    bool isItem;
    bool isButton;


    void Awake()
    {
        initialMaterial = GetComponent<Renderer>().material;
        renderer = GetComponent<Renderer>();
        grabber = null;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        if (gameObject.GetComponent<Item>() == null) isItem = false; else isItem = true;
        if (gameObject.GetComponent<Button>() == null) isButton = false; else isButton = true;
    }

    public void Interact(GameObject interactor, string action)
    {
        interactorScript = interactor.GetComponent<SelectionManager>();
        if (interactor == player) isPlayer = true;
        if (action == "watch")
        {
            Debug.Log("Watching");
            if (isPlayer) renderer.material = interactorScript.watchMaterial;
        }
        if (action == "unwatch")
        {
            if (isPlayer) renderer.material = initialMaterial;
        }
        //if (action == "grab")
    }
    /*
    void FixedUpdate()
    {
        if (grabber != null) Hold();
    }

    public void Interact(GameObject interactor, string action)
    {
        if (action == "hover")  // big brain
        {
            if (interactor == player)
                gameObject.GetComponent<Renderer>().material = interactor.GetComponent<SelectionManager>().hoverMaterial;
        }
        if (action == "unhover")
        {
            if (interactor == player)
            {
                gameObject.GetComponent<Renderer>().material = initialMaterial;
            }
        }
        if (action == "grab")
        {
            if (grabber != null) grabber.GetComponent<SelectionManager>().grabbedItem = null;
            grabber = interactor;
            Debug.Log("Grabbed");


            //gameObject.GetComponent<Renderer>().material = interactor.GetComponent<SelectionManager>().hoverMaterial;
        }
        if (action == "drop")  // ONLY CALL WHEN ACTUALLY UNGRABBING
        {
            Drop();
        }

        if (grabber != null) Equip();
    }
    void Hold()
    {
        rb.velocity = 10f * (largeItemSocket.position - transform.position);
        rb.angularVelocity = Vector3.zero;
        //transform.position = largeItemSocket.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.5f);
    }
    void Equip()
    {
        actorCollider = grabber.GetComponent<Collider>();
        rb.useGravity = false;
        largeItemSocket = grabber.GetComponent<SelectionManager>().largeItemSocket;
        if (grabber == player) GetComponent<Renderer>().material = initialMaterial;
    }
    public void Drop()
    {
        rb.useGravity = true;
        grabber = null;
        //gameObject.GetComponent<Renderer>().material = initialMaterial;
    }
    */
}
