using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseScript : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject endScreen;
    public GameObject loseScreen;
    public GameObject winScreen;
    public Musician musician;

    public string[] tips;

    private int tipIndex;
    private bool paused = false;
    private bool finished = false;

    private void Start() {
        settingsPanel.SetActive(false);
        endScreen.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !finished) {
            paused = !paused;
            if (paused) {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                if (musician) musician.musicSource.Pause();
            } else {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                if (musician) musician.musicSource.UnPause();
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

    public void FinishGame(bool win) {
        finished = true;
        endScreen.SetActive(true);
        if (win) {
            winScreen.SetActive(true);
        } else {
            loseScreen.SetActive(true);
            tipIndex = Random.Range(0, tips.Length);
            setTip();
        }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void changeTip(int n) {
        tipIndex = (tipIndex + n + tips.Length) % tips.Length;
        setTip();
    }

    private void setTip() {
        loseScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tips[tipIndex];
    }
}
