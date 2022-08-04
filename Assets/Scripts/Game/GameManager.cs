using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[Serializable]
public class WaveInfo {
    public int[] enemiesAtSpawns;
    public float timeForNextWave;
}

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] spawnPoints;
    public WaveInfo[] waves;
    public GameObject barman;
    public AttacksScriptableObject[] attacks;
    public GameObject[] doors;

    float checkWaveTimer = 2;
    int nextWave = 0;
    List<GameObject> spawnedEnemies;

    void Start()
    {
        spawnedEnemies = new List<GameObject>();
        startNextWave();
    }

    void Update()
    {
        checkWaveTimer -= Time.deltaTime;
        if (checkWaveTimer <= 0) {
            foreach (GameObject enemy in spawnedEnemies.ToArray())
            {
                if (!enemy) spawnedEnemies.Remove(enemy);
            }
            checkWaveTimer = 2;
            if (spawnedEnemies.Count == 0) startNextWave();
        }
    }

    private void startNextWave() {
        if (nextWave < waves.Length) {
            for (int i=0; i < waves[nextWave].enemiesAtSpawns.Length; i++) {
                for (int j=0; j < waves[nextWave].enemiesAtSpawns[i]; j++) {
                    GameObject enemy = GameObject.Instantiate(enemyPrefab, spawnPoints[i].transform.position, Quaternion.identity);
                    enemy.GetComponent<PersonOther>().chasePlayer();
                    enemy.GetComponent<PersonOther>().attackInfo = attacks[UnityEngine.Random.Range(0,attacks.Length)];
                    spawnedEnemies.Add(enemy);
                }
            }
            FindObjectOfType<BeerSpawner>().respawn();
            StartCoroutine(openDoors());

            nextWave++;
            if (nextWave == waves.Length) {
                if (barman) {
                    spawnedEnemies.Add(barman);
                    barman.GetComponent<PersonOther>().chasePlayer();
                }
            }
        } else {
            win();
            checkWaveTimer = 10000;
        }
    }

    private void win() {
        FindObjectOfType<CamAttractor>().GameFinished();
        FindObjectOfType<PauseScript>().FinishGame(true);
        Destroy(FindObjectOfType<RigidbodyFirstPersonController>());
        Destroy(FindObjectOfType<PersonPlayer>().damagedImg.gameObject);
        Destroy(FindObjectOfType<PersonPlayer>().lowHealthImg.gameObject);
        Destroy(FindObjectOfType<PersonPlayer>());
        Destroy(FindObjectOfType<PlayerAttack>());
        FindObjectOfType<BeerTrigger>().removeFromGame();
    }

    private IEnumerator openDoors() {
        float angle = 0;
        while (angle < 90) {
            angle += Time.deltaTime * 90;
            foreach(GameObject d in doors) {
                d.transform.localRotation = Quaternion.Euler(0, -angle, 0);
            }
            yield return null;
        }
        yield return new WaitForSeconds(3);
        while (angle > 0) {
            angle -= Time.deltaTime * 90;
            foreach(GameObject d in doors) {
                d.transform.localRotation = Quaternion.Euler(0, -angle, 0);
            }
            yield return null;
        }
        foreach(GameObject d in doors) {
            d.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
