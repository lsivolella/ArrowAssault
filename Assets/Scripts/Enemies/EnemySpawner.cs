using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]

    public class Wave
    {
        public bool customizeWave;
        public GameObject[] enemies;
        public int enemyCount;
        public float spawnDelay;
    }

    [SerializeField] LevelManager levelManager;
    [SerializeField] Text uiWaveMeter;
    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float waveCooldown = 1f;


    private Wave currentWave;
    private int currentWaveIndex = 0;
    private Transform targetPlayer;
    private bool waveEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnWave(currentWaveIndex));
        uiWaveMeter.GetComponent<Text>().text = "0";
    }

    IEnumerator SpawnWave(int index)
    {
        yield return new WaitForSeconds(waveCooldown);
        currentWave = waves[index];
        UpdateWaveMeter(index);

        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            if (!targetPlayer)
            {
                yield break; // Stops wave spawning if player dies.
            }

            if (currentWave.customizeWave)
            {
                GameObject customEnemy = currentWave.enemies[i];
                Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject newEnemy = Instantiate(customEnemy, randomSpot.position, randomSpot.rotation) as GameObject;
                newEnemy.transform.parent = transform;
            }
            else
            {
                GameObject randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
                Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject newEnemy = Instantiate(randomEnemy, randomSpot.position, randomSpot.rotation) as GameObject;
                newEnemy.transform.parent = transform;
            }

            if (i == currentWave.enemyCount - 1)
            {
                waveEnded = true;
            }
            else
            {
                waveEnded = false;
            }

            yield return new WaitForSeconds(currentWave.spawnDelay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waveEnded == true
            && GameObject.FindGameObjectsWithTag("MeleeEnemy").Length == 0
            && GameObject.FindGameObjectsWithTag("RangedEnemy").Length == 0
            && GameObject.FindGameObjectsWithTag("Boss").Length == 0)
        {
            waveEnded = false;

            if (currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++;
                StartCoroutine(SpawnWave(currentWaveIndex));
            }
            else
            {
                levelManager.Victory();
            }
        }
    }

    private void UpdateWaveMeter(int index)
    {
        uiWaveMeter.GetComponent<Text>().text = (index + 1).ToString();
    }

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }
}
