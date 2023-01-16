using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public float pauseTimeScale;

    bool paused;

    public bool unpauseOnSave;
    public bool unpuaseOnLoad;
    public GameObject pauseMenu;

    public CanvasGroup infoGroup;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        pauseMenu.SetActive(false);

        infoGroup.alpha = 0;
        infoGroup.blocksRaycasts = false;
        infoGroup.interactable = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void PauseGame()
    {
        paused = true;

        pauseMenu.SetActive(true);

        Time.timeScale = pauseTimeScale;
    }

    public void UnpauseGame()
    {
        paused = false;

        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
    }

    public void OpenInfoTab()
    {
        if (infoGroup.interactable)
        {
            //Close it
            infoGroup.interactable = false;
            infoGroup.alpha = 0;
            infoGroup.blocksRaycasts = false;
        }
        else
        {
            //open it
            infoGroup.interactable = true;
            infoGroup.alpha = 1;
            infoGroup.blocksRaycasts = true;
        }
    }
    public void Save()
    {
        SaveManager.Instance.OnSave();
        if (unpauseOnSave)
        {
            UnpauseGame();
        }
        
    }

    public void Load()
    {
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
        if (unpuaseOnLoad)
        {
            UnpauseGame();
        }
       
    }

    public void Resume()
    {
        UnpauseGame();
    }

    public void Exit()
    {
        UnpauseGame();
        SaveManager.Instance.OnSave();
        SceneLoader.Instance.LoadScene("MainMenu");
    }
    public bool Paused()
    {
        return paused;
    }
}
