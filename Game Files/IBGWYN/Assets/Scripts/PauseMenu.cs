using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject HUDUI;

    bool pause;
    bool optionsMode;

    private void Start()
    {
        HUDUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        OptionsMenuUI.SetActive(false);
    }

    private void Update()
    {
        if(!optionsMode)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pause = !pause;
            }

            pauseMenuUI.SetActive(pause);
            HUDUI.SetActive(!pause);

            Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = pause;

            Time.timeScale = pause ? 0 : 1;
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

    public void volume(float newValue)
    {
        AudioListener.volume = newValue/100;
    }
}
