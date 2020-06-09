using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public string grabbableTag = "grabbable";
    public Material hoverMaterial;
    public Transform CameraSocket; // Used for raycast
    public Transform largeItemSocket;  // Used in Item.cs
    public float grabRange = 100f;

    GameObject hoverItem;
    GameObject newHoverItem;
    public GameObject grabbedItem = null;
    Item grabbedItemScript;
    Item hoverItemScript;
    Item newHoverItemScript;
    public void ItemSelection(bool grab, bool drop)
    {
        RaycastHit hit;
        Ray ray = new Ray(CameraSocket.position, CameraSocket.transform.forward);  // rai rai!
        Debug.DrawRay(CameraSocket.position, CameraSocket.transform.forward, Color.red, 0.01f);
        if (Physics.Raycast(ray, out hit, grabRange))
        {
            if (hit.collider.CompareTag(grabbableTag))
            {
                newHoverItem = hit.collider.gameObject;
                newHoverItemScript = newHoverItem.GetComponent<Item>();

                if (newHoverItem != hoverItem)
                {
                    ClearHover("Looked at a new item");
                    SetHover("yeah");
                }
                if (grab && grabbedItem == null)
                {
                    grabbedItem = hoverItem;
                    grabbedItemScript = grabbedItem.GetComponent<Item>();
                    hoverItemScript.Interact(gameObject, "grab");
                }
            }
            else ClearHover("isnt grabbable");  // When looking at an object, but it isn't grabbable
        }
        else ClearHover("not looking at an object");  // When not looking at an object
        if (drop && grabbedItem != null)
        {
            ClearGrab();
        }
    }
    public void ClearGrab()  // drops both for actor and item
    {
        if (grabbedItem == null) return;
        grabbedItemScript.Interact(gameObject, "drop");
        grabbedItem = null;
    }
    void SetHover(string message)
    {
        hoverItem = newHoverItem;
        hoverItemScript = newHoverItemScript;
        newHoverItemScript.Interact(this.gameObject, "hover");
    }
    void ClearHover(string message)
    {
        if (hoverItem != null)
        {
            hoverItemScript.Interact(this.gameObject, "unhover");
            hoverItem = null;
            hoverItemScript = null;
        }
    }
}
