using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject grabber;
    Actor grabberScript;
    SelectionManager selectionManager;
    Interactable interactableScript;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactableScript = GetComponent<Interactable>();
        Drop(null);
    }
    void FixedUpdate()
    {
        Hold();
    }

    public void PickUp(GameObject interactor)
    {
        if (interactor == null) return;
        if (interactor.GetComponent<SelectionManager>().grabbedBlock != null) // if handles nullpointreference at first grab
        {
            interactor.GetComponent<SelectionManager>().grabbedScript.GetComponent<Block>().grabber.GetComponent<SelectionManager>().ClearWatched();
            interactor.GetComponent<SelectionManager>().grabbedScript.GetComponent<Block>().Drop(null);  
        }
        rb.useGravity = false;

        grabber = interactor;
        grabberScript = grabber.GetComponent<Actor>();
        selectionManager = grabber.GetComponent<SelectionManager>();
        selectionManager.grabbedBlock = gameObject;
        selectionManager.grabbedScript = interactableScript;
        selectionManager.ClearWatched();
    }
    void Hold()
    {
        if (grabber != null)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(100f*(grabberScript.blockSocket.position - transform.position));
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.5f);
        }
    }
    public void Drop(GameObject interactor)
    {
        if (interactor == grabber ^ interactor == null)  // null means forced by system, not player
        {
            rb.useGravity = true;

            selectionManager.grabbedBlock = null;
            selectionManager.grabbedScript = null;
            grabber = null;
            grabberScript = null;
            selectionManager = null;
        }
    }
}
