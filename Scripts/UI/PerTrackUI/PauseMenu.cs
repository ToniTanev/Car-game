using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;

    private GameObject pauseMenu;
    private GameObject ai;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.name == "PauseMenu")
            {
                pauseMenu = child.gameObject;
            }
        }

        ai = GameObject.FindGameObjectWithTag("AI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OnResumeButton()
    {
        Resume();
    }

    public void OnRecoverButton()
    {
        Time.timeScale = 1f;
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1f;

        GameState.isGameFinished = false;

        ai.GetComponent<AIFinish>().ResetFinishPasses();

        InitScene.LoadCurrentTrack();
    }
    public void OnMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Constants.mainMenuIndex);
    }
}
