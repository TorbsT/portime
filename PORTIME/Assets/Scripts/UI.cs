using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject gameMasterObject;
    GameMaster gameMaster;

    public Text timeLeft;

    int timeLeftInt;


    private void Start()
    {
        gameMaster = gameMasterObject.GetComponent<GameMaster>();
    }
    void Update()
    {
        /*
        timeLeftInt = Mathf.CeilToInt(gameMaster.timeLeft);
        timeLeft.text = timeLeftInt.ToString();
        */
    }
}
