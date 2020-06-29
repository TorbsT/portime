using System.Collections;
using UnityEngine;

public class RewindEffect : MonoBehaviour
{
    public Material rewindMaterial;
    GameMaster gameMaster;
    void Start()
    {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (gameMaster.isRewinding)
            Graphics.Blit(src, dest, rewindMaterial);
        else
            Graphics.Blit(src, dest);
    }
}
