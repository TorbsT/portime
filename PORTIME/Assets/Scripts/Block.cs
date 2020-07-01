using System;  // For try and catch
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] float swingForce = 1000f;
    [SerializeField] float distanceKeeperForce = 0f;
    [SerializeField] float holdCooldown = 0.5f;
    [SerializeField] float currentHoldCooldown = 0f;
    [SerializeField] float rotateSpeed = 0.5f;
    public float weight = 1f;
    public GameObject grabber;
    Actor grabberScript;
    SelectionManager selectionManager;
    Interactable interactableScript;
    GameMaster gameMaster;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactableScript = GetComponent<Interactable>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        Drop(null);
    }
    void FixedUpdate()
    {
        Hold();
    }
    public void JumpedOn()
    {
        currentHoldCooldown = holdCooldown;
    }
    public void PickUp(GameObject interactor)
    {
        if (interactor == null) return;
        if (grabber != null)
        {
            grabber.GetComponent<SelectionManager>().grabbedBlock = null;
            grabber.GetComponent<SelectionManager>().grabbedScript = null;
        }
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
        //rb.useGravity = false;

        grabber = interactor;
        grabberScript = grabber.GetComponent<Actor>();
        interactableScript.grabberId = grabberScript.id;
        selectionManager = grabber.GetComponent<SelectionManager>();
        selectionManager.grabbedBlock = gameObject;
        selectionManager.grabbedScript = interactableScript;
        selectionManager.ClearWatched();
    }
    void Hold()
    {
        if (gameMaster.isRewinding) return;
        currentHoldCooldown -= Time.fixedDeltaTime;
        if (grabber != null && currentHoldCooldown < 0f)
        {
            //rb.useGravity = false;
            //grabber.GetComponent<Rigidbody>().AddForce(0f, -100f * weight, 0f);
            rb.velocity = Vector3.zero;
            Vector3 travelVector = (grabberScript.blockRotator.position - transform.position);
            Vector3 playerToBlock = (grabberScript.transform.position - transform.position);
            Vector3 playerToSocket = (grabberScript.blockRotator.position - grabberScript.transform.position);


            rb.AddForce(rb.mass*swingForce * travelVector);
            rb.AddForce(rb.mass*distanceKeeperForce * (Vector3.SqrMagnitude(playerToBlock) - Vector3.SqrMagnitude(playerToSocket)) * playerToBlock);
            //rb.angularVelocity = (grabberScript.blockRotator.rotation.eulerAngles-transform.rotation.eulerAngles);
            //transform.rotation = Quaternion.Lerp(transform.rotation, grabberScript.blockRotator.rotation, rotateSpeed);
            //rb.AddTorque(grabberScript.blockRotator.rotation.x-transform.rotation.x, 0f, 0f);
            //rb.angularVelocity = Vector3.zero;
            transform.rotation = grabberScript.blockRotator.rotation;
        }
        //else rb.useGravity = true;
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
