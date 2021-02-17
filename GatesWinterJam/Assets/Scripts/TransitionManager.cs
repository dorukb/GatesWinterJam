using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadAuctionScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadDialogueScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadEndScene()
    {
        SceneManager.LoadScene(3);
    }
}
