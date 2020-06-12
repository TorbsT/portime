using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public string interactableTag = "Interactable";
    public Material watchMaterial;
    public Transform CameraSocket; // Used for raycast
    public Transform blockSocket;  // Used in Item.cs
    public float grabRange = 100f;

    GameObject watched;
    GameObject newWatched;
    public GameObject grabbedBlock = null;  // ONLY MODIFY THROUGH OTHER SCRIPTS
    public Interactable grabbedScript;  // ONLY MODIFY THROUGH OTHER SCRIPTS
    Interactable watchedScript;
    void Start()
    {
        ClearWatched();
    }
    public void ObjectInteraction(bool grab, bool drop)
    {
        if (drop)
            Drop();
        RaycastHit hit;
        Ray ray = new Ray(CameraSocket.position, CameraSocket.transform.forward);  // rai rai!
        Debug.DrawRay(CameraSocket.position, CameraSocket.transform.forward, Color.red, 0.01f);
        if (Physics.Raycast(ray, out hit, grabRange, (1 << 8)))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                newWatched = hit.collider.gameObject;
                if (newWatched != watched)
                {
                    ClearWatched();
                    watched = newWatched;
                    watchedScript = watched.GetComponent<Interactable>();
                    watchedScript.Interact(gameObject, "watch");
                }
                if (grab)
                {
                    watchedScript.Interact(gameObject, "grab");
                }
            }
            else ClearWatched();  // When looking at an object, but it isn't grabbable
        }
        else ClearWatched();  // When not looking at an object
    }
    public void Drop()
    {
        if (grabbedBlock != null)
            grabbedScript.Interact(gameObject, "drop");
    }
    public void ClearWatched()
    {
        if (watched != null)
        {
            watchedScript.Interact(gameObject, "unwatch");
        }
        watched = null;
        watchedScript = null;

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
