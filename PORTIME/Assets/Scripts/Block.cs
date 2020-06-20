using System;  // For try and catch
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
            try
            {
                interactor.GetComponent<SelectionManager>().grabbedScript.GetComponent<Block>().grabber.GetComponent<SelectionManager>().ClearWatched();  // what the actual fuck
                interactor.GetComponent<SelectionManager>().grabbedScript.GetComponent<Block>().Drop(null);
            }
            catch (Exception e)
            {
                Debug.LogError("{0} Exception caught." + e);
            }
        }
        rb.useGravity = false;

        grabber = interactor;
        grabberScript = grabber.GetComponent<Actor>();
        interactableScript.grabberId = grabberScript.id;
        Debug.Log("bruhhhhhddsf " + grabber);
        Debug.Log("ja " + grabberScript.id);
        Debug.Log(interactableScript.grabberId);
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
            rb.AddForce(1000f*(grabberScript.blockRotator.position - transform.position));
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.Lerp(transform.rotation, grabberScript.blockRotator.rotation, 0.5f);
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
            interactableScript.grabberId = -1;
            selectionManager = null;
        }
    }
}
