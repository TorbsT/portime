using UnityEngine;
using UnityEngine.UI;

public class testSCript : MonoBehaviour
{
    Text txt;
    int i;
    // Update is called once per frame
    void Start()
    {
        txt = GetComponent<Text>();
        txt.text = "";
    }
    void Update()
    {
        i++;
        if (i > 1f/Time.deltaTime)
        {
            txt.text += "a";
            i = 0;
        }
    }
}
