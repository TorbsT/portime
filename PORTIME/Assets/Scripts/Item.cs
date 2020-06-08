using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject player;
    public GameObject grabber;

    Material initialMaterial;
    bool wasHovered;  // so you don't need to change material every update
    bool wasGrabbed;  // idk probably useful later

    void Awake()
    {
        initialMaterial = gameObject.GetComponent<Renderer>().material;
        grabber = null;
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Interact(GameObject interactor, string action)
    {
        if (action == "hover")
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
            grabber = interactor;
        }
        if (action == "ungrab")  // ONLY CALL WHEN ACTUALLY UNGRABBING
        {
            grabber = null;
        }
    }
}
