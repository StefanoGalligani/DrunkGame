using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

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
            if (spawnedEnemies.Count == 0) startNextWave();
            checkWaveTimer = 2;
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
            Debug.Log("Won");
            checkWaveTimer = 10000;
        }
    }

    private IEnumerator openDoors() {
        yield return null;
    }
}
