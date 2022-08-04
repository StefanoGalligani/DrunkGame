using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject settingsPanel;
    private bool paused = false;

    private void Start() {
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            if (paused) {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = paused;
            settingsPanel.SetActive(paused);
        }
    }

    public void BackToGame() {
        paused = false;
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMenu() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }

    public void RestartGame() {
        SceneManager.LoadScene("Game");
    }
}
