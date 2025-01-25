using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Random Spawn Enemy")]
    public List<GameObject> enemyObject = new List<GameObject>();

    [Header("Random Time Range (seconds)")]
    public float minTime = 10f;
    public float maxTime = 20f;

    private void Start()
    {
        if (enemyObject.Count > 0)
        {
            StartCoroutine(RandomlyActivateGameObjects());
        }
        else
        {
            Debug.LogWarning("No GameObjects assigned to RandomActivator script!");
        }
    }

    private IEnumerator RandomlyActivateGameObjects()
    {
        while (true)
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
}