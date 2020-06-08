using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public string grabbableTag = "grabbable";
    public Material hoverMaterial;
    public Transform CameraSocket; // Used for raycast
    public float grabRange = 100f;

    GameObject hoverItem;
    GameObject newHoverItem;
    GameObject grabbedItem;
    Item itemScript;
    Item newItemScript;
    public void ItemSelection()
    {
        RaycastHit hit;
        Ray ray = new Ray(CameraSocket.position, CameraSocket.transform.forward);  // rai rai!
        Debug.DrawRay(CameraSocket.position, CameraSocket.transform.forward, Color.red, 0.01f);
        if (Physics.Raycast(ray, out hit, grabRange))
        {
            if (hit.collider.CompareTag(grabbableTag))
            {
                newHoverItem = hit.collider.gameObject;
                newItemScript = newHoverItem.GetComponent<Item>();

                if (newHoverItem != hoverItem)
                {
                    ClearHover("Looked at a new item");
                    SetHover("yeah");
                }

                // dank code that maybe runs
            }
            else ClearHover("isnt grabbable");  // When looking at an object, but it isn't grabbable
        }
        else ClearHover("not looking at an object");  // When not looking at an object
    }

    void SetHover(string message)
    {
        Debug.Log(message);
        hoverItem = newHoverItem;
        itemScript = newItemScript;
        newItemScript.Interact(this.gameObject, "hover");
    }
    void ClearHover(string message)
    {
        //Debug.Log(message);
        if (hoverItem != null)
        {
            Debug.Log(message);
            itemScript.Interact(this.gameObject, "unhover");
            hoverItem = null;
            itemScript = null;
        }
    }
}
