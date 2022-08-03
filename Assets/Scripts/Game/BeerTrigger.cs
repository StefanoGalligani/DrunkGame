using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeerTrigger : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public int beerHealth = 7;
    public AudioSource audioSource;
    public AudioClip soundDrink;
    private List<GameObject> beers;

    private void Start() {
        beers = new List<GameObject>();
        changeAlpha(tooltipText, 0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            drinkBeer();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!beers.Contains(other.gameObject)) {
            beers.Add(other.gameObject);
            changeAlpha(tooltipText, 1);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (beers.Contains(other.gameObject)) {
            beers.Remove(other.gameObject);
            if (beers.Count == 0) changeAlpha(tooltipText, 0);
        }
    }

    private void drinkBeer() {
        if (beers.Count > 0) {
            audioSource.PlayOneShot(soundDrink);
            transform.parent.parent.GetComponent<PersonPlayer>().heal(beerHealth);
            GameObject b = beers[0]; 
            beers.Remove(b);
            b.GetComponent<Beer>().drank(transform.parent.position);
            if (beers.Count == 0) changeAlpha(tooltipText, 0);
        }
    }

    private void changeAlpha(TextMeshProUGUI i, float a) {
        Color c = i.color;
        c.a = a;
        i.color = c;
    }

    public void removeFromGame() {
        changeAlpha(tooltipText, 0);
        Destroy(gameObject);
    }
}
