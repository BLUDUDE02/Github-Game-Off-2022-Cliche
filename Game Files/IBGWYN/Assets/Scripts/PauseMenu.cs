using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject HUDUI;

    bool pause = true;
    bool optionsMode;
    bool MainMenu = true;

    private void Start()
    {
        if(HUDUI != null)
        {
            MainMenu = false;
            HUDUI.SetActive(true);
            pause = false;
        }
        
        pauseMenuUI.SetActive(pause);
        OptionsMenuUI.SetActive(false);
    }

    private void Update()
    {
        if(!optionsMode && !MainMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pause = !pause;
            }

            pauseMenuUI.SetActive(pause);
            HUDUI.SetActive(!pause);

            Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = pause;

            AudioListener.pause = pause;

            Time.timeScale = pause ? 0 : 1;
        }

        if(optionsMode)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        optionsMode = true;
        OptionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void Back()
    {
        optionsMode = false;
        OptionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void volume(float newValue)
    {
        AudioListener.volume = newValue/100;
    }
}
