using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject menuBackground;
    public GameObject beerLiquid;
    public AudioClip drinkSound;

    float currentScale;

    private void Start() {
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        menuBackground.SetActive(true);
        currentScale = 1;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }
    
    public void StartSandbox() {
        SceneManager.LoadScene("Sandbox");
    }

    public void OpenSettings() {
        settingsPanel.SetActive(true);
        menuBackground.SetActive(false);
    }

    public void OpenCredits() {
        creditsPanel.SetActive(true);
        menuBackground.SetActive(false);
        drinkBeer();
    }

    public void ClosePanels(bool credits = false) {
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        menuBackground.SetActive(true);
        if (credits) drinkBeer();
    }
    
    public void CloseGame() {
        Application.Quit();
    }

    private void drinkBeer() {
        currentScale -= 0.1f;
        if (currentScale >= 0) {
            beerLiquid.transform.localScale = new Vector3(1, currentScale, 1);
            GetComponent<AudioSource>().PlayOneShot(drinkSound);
        } else if (beerLiquid) {
            Destroy(beerLiquid);
            GetComponent<AudioSource>().PlayOneShot(drinkSound);
        }
    }
}
