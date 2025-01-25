using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [Header("Random Spawn Enemy")]
    public List<GameObject> enemyObject = new List<GameObject>();

    [Header("Random Time Range (seconds)")]
    public float minTime = 10f;
    public float maxTime = 20f;

    private bool isSpawning = true;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (enemyObject.Count > 0)
        {
            spawnCoroutine = StartCoroutine(RandomlyActivateGameObjects());
        }
        else
        {
            Debug.LogWarning("No GameObjects assigned to EnemyManager script!");
        }
    }

    private IEnumerator RandomlyActivateGameObjects()
    {
        while (isSpawning)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            foreach (GameObject obj in enemyObject)
            {
                obj.SetActive(false);
            }

            int randomIndex = Random.Range(0, enemyObject.Count);
            enemyObject[randomIndex].SetActive(true);
        }
    }
    public void ToggleSpawner()
    {
        if (isSpawning)
        {
            StopSpawner();
        }
        else
        {
            StartSpawner();
        }
    }

    public void StartSpawner()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            spawnCoroutine = StartCoroutine(RandomlyActivateGameObjects());
        }
    }
    public void StopSpawner()
    {
        if (isSpawning)
        {
            isSpawning = false;
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
            }
        }
    }
}
