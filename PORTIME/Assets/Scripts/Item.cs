﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject player;
    public GameObject grabber;

    Material initialMaterial;
    bool wasHovered;  // so you don't need to change material every update
    bool wasGrabbed;  // idk probably useful later

    Rigidbody rb;

    Transform leftHand;
    Transform rightHand;
    Transform largeItemSocket;

    Collider itemCollider;
    Collider actorCollider;

    void Awake()
    {
        initialMaterial = gameObject.GetComponent<Renderer>().material;
        grabber = null;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        itemCollider = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        if (grabber != null) Hold();
    }

    public void Interact(GameObject interactor, string action)
    {
        if (action == "hover" && false)  // big brain
        {
            if (interactor == player)
                gameObject.GetComponent<Renderer>().material = interactor.GetComponent<SelectionManager>().hoverMaterial;
        }
        if (action == "unhover" && false)
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
            gameObject.GetComponent<Renderer>().material = interactor.GetComponent<SelectionManager>().hoverMaterial;
        }
        if (action == "drop")  // ONLY CALL WHEN ACTUALLY UNGRABBING
        {
            Drop();
        }

        if (grabber != null) Equip();
    }
    void Hold()
    {
        gameObject.transform.position = largeItemSocket.position;
    }
    void Equip()
    {
        actorCollider = grabber.GetComponent<Collider>();
        rb.useGravity = false;
        largeItemSocket = grabber.GetComponent<SelectionManager>().largeItemSocket;
    }
    void Drop()
    {
        rb.useGravity = true;
        grabber = null;
        gameObject.GetComponent<Renderer>().material = initialMaterial;
    }
    void OnCollisionStay(Collision collision)
    {
        Debug.LogWarning("Layer as static name, possible cause for collision bugs");
        //if (collision.gameObject.layer.Equals("Actors"))
        //{
            Debug.Log("bruh");
            if (collision.gameObject == grabber)
            {
                Debug.Log("nice");
                actorCollider = grabber.GetComponent<Collider>();
            }
        //}
    }
}
