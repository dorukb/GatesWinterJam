using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Image fadePanel;
    public bool ShouldFadeInStart = true;
    public float duration = 1f;
    Color transparent = new Color(0, 0, 0, 0);
    public void LoadStartScene()
    {
        //SceneManager.LoadScene(0);
        StartCoroutine(FadeOut(duration, transparent, Color.black, 0));

    }
    public void LoadIntroScene()
    {
        //SceneManager.LoadScene(0);
        StartCoroutine(FadeOut(duration, transparent, Color.black, 1));

    }
    public void LoadDialogueScene()
    {
        //SceneManager.LoadScene(2); 
        StartCoroutine(FadeOut(duration, transparent, Color.black, 2));

    }
    public void LoadAuctionScene()
    {
        //SceneManager.LoadScene(1);
        StartCoroutine(FadeOut(duration, transparent, Color.black, 3));

    }
    public void LoadEndScene()
    {
        //SceneManager.LoadScene(3);
        StartCoroutine(FadeOut(duration, transparent, Color.black, 4));

    }
    public void Start()
    {
        if (fadePanel && ShouldFadeInStart)
        {
            StartCoroutine(FadeIn(duration, Color.black, transparent));
        }
    }

    IEnumerator FadeOut(float duration, Color from, Color to, int sceneIndex)
    {
        fadePanel.gameObject.SetActive(true);

        float timePassed = 0f;
        float perct;
        while (timePassed < duration)
        {
            perct = timePassed / duration;
            fadePanel.color = Color.Lerp(from, to, perct);

            timePassed += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = to;
        SceneManager.LoadScene(sceneIndex);
    }
    IEnumerator FadeIn(float duration, Color from, Color to)
    {
        fadePanel.gameObject.SetActive(true);

        float timePassed = 0f;
        float perct;
        while (timePassed < duration)
        {
            perct = timePassed / duration;
            fadePanel.color = Color.Lerp(from, to, perct);
            timePassed += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = to;
        fadePanel.gameObject.SetActive(false);
    }
}

