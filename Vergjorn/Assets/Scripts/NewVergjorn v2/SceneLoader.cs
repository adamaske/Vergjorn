using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public CanvasGroup canvasGroup;

    float t = 0;
    public float timeToFade;

    public bool fadeInScene;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (fadeInScene)
        {
            StartCoroutine(FadeInScene());
        }
        
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutScene(sceneName));
    }

    IEnumerator FadeInScene()
    {
        canvasGroup.alpha = 1;
        yield return null;
        while(t < timeToFade)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / timeToFade);
            t += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        t = 0;
    }

    IEnumerator FadeOutScene(string sceneName)
    {
        canvasGroup.alpha = 0;
        yield return null;
        while (t < timeToFade)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / timeToFade);
            t += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        yield return null;
        SceneManager.LoadScene(sceneName);
    }
}
