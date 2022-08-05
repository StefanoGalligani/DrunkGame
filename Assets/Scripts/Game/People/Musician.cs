using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musician : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
        StartCoroutine(startPlaying());
    }

    private IEnumerator startPlaying() {
        yield return new WaitForSeconds(2);
        musicSource.Play();
    }

}
