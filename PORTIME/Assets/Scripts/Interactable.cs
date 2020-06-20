using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject player;
    public GameObject grabber;
    public int grabberId;
    public string actorTag = "Actor";

    SelectionManager interactorScript;
    Material initialMaterial;
    Renderer renderer;
    bool isPlayer;
    bool isBlock;
    bool isButton;
    bool isWatching;

    Block blockScript;
    Button buttonScript;

    void Awake()
    {
        initialMaterial = GetComponent<Renderer>().material;
        renderer = GetComponent<Renderer>();
        grabber = null;
        isWatching = false;
    }

    void Start()
    {
        player = GetPlayer();
        blockScript = GetComponent<Block>();
        buttonScript = GetComponent<Button>();
        if (blockScript == null) isBlock = false; else isBlock = true;
        if (buttonScript == null) isButton = false; else isButton = true;
    }

    public void Interact(GameObject interactor, string action)
    {
        interactorScript = interactor.GetComponent<SelectionManager>();
        if (interactor == player) isPlayer = true; else isPlayer = false;
        if (action == "watch" && isPlayer)
        {
            isWatching = true;
            if (!isBlock)
                renderer.material = interactorScript.watchMaterial;
            else if (blockScript.grabber != interactor)
                renderer.material = interactorScript.watchMaterial;
        }
        if (action == "unwatch" && isPlayer)
        {
            isWatching = false;
            renderer.material = initialMaterial;
        }
        if (action == "grab")
        {
            Debug.Log("BRUUUUH");
            if (isBlock && blockScript.grabber != interactor) GetComponent<Block>().PickUp(interactor);
            if (isButton) GetComponent<Button>().Click();
        }
        if (action == "drop")
        {
            if (isBlock)
            {
                GetComponent<Block>().Drop(interactor);
            }
        }
    }
    GameObject GetPlayer()
    {
        GameObject[] actors = GameObject.FindGameObjectsWithTag(actorTag);
        if (actors.Length != 1) return null;
        return actors[0];
    }
    public void HandleGrabFrame(GameObject interactor)
    {
        if (interactor != grabber)
        {
            if (interactor == null)
            {
                Interact(null, "drop");
            } else
            {
                Debug.Log("its here chief");
                Interact(interactor, "grab");
            }
        }
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
