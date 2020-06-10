﻿using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public string interactableTag = "Interactable";
    public Material watchMaterial;
    public Transform CameraSocket; // Used for raycast
    public Transform largeItemSocket;  // Used in Item.cs
    public float grabRange = 100f;

    GameObject watched;
    GameObject newWatched;
    public GameObject grabbedItem = null;
    Interactable watchedScript;
    void Start()
    {
        watched = null;
    }
    public void ObjectInteraction(bool grab, bool drop)
    {
        RaycastHit hit;
        Ray ray = new Ray(CameraSocket.position, CameraSocket.transform.forward);  // rai rai!
        Debug.DrawRay(CameraSocket.position, CameraSocket.transform.forward, Color.red, 0.01f);
        if (Physics.Raycast(ray, out hit, grabRange))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag(interactableTag))
            {
                newWatched = hit.collider.gameObject;
                if (newWatched != watched)
                {
                    if (watched != null) watchedScript.Interact(gameObject, "unwatch");
                    watched = newWatched;
                    watchedScript = watched.GetComponent<Interactable>();
                    watchedScript.Interact(gameObject, "watch");
                }
                if (grab)
                {
                    watchedScript = watched.GetComponent<Interactable>();
                    watchedScript.Interact(gameObject, "grab");
                }
            }
             //ClearHover("isnt grabbable");  // When looking at an object, but it isn't grabbable
        }
        else //ClearHover("not looking at an object");  // When not looking at an object
        if (drop && grabbedItem != null)
        {
            //ClearGrab();
        }
    }
    /*
    public void ClearGrab()  // drops both for actor and item
    {
        if (grabbedItem == null) return;
        grabbedItemScript.Interact(gameObject, "drop");
        grabbedItem = null;
    }
    void SetHover(string message)
    {
        watched = newHoverItem;
        if (interactableIsItem)
        {
            hoverItemScript = watched.GetComponent<Item>();
            hoverItemScript.Interact(gameObject, "hover");
        }
    }
    void ClearHover(string message)
    {
        if (watched != null)
        {
            hoverItemScript.Interact(this.gameObject, "unhover");
            watched = null;
            hoverItemScript = null;
        }
    }
    */
}
