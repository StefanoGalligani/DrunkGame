using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class SettingsScript : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public Toggle doubleVisionToggle;

    public static float sensitivity;
    public static bool disableDoubleVision;

    private void Start() {
        if (sensitivity > 0.1f) {
            sensitivitySlider.value = sensitivity;
        } else {
            sensitivity = sensitivitySlider.value;
        }
        doubleVisionToggle.isOn = disableDoubleVision;
        volumeSlider.value = AudioListener.volume;
    }

    public void UpdateSensitivity() {
        sensitivity = sensitivitySlider.value;
        if (FindObjectOfType<RigidbodyFirstPersonController>()) {
            FindObjectOfType<RigidbodyFirstPersonController>().mouseLook.XSensitivity = sensitivity;
            FindObjectOfType<RigidbodyFirstPersonController>().mouseLook.YSensitivity = sensitivity;
        }
    }

    public void UpdateSoundVolume() {
        AudioListener.volume = volumeSlider.value;
    }

    public void UpdateDoubleVision() {
        disableDoubleVision = doubleVisionToggle.isOn;
    }
}
