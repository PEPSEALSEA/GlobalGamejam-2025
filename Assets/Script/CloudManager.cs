using UnityEngine;
using System.Collections.Generic;

public class CloudManager : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
    public int initialCloudCount = 20;
    public float minYDistance = 2f;
    public float maxYDistance = 4f;
    public float maxXRange = 5f;
    public float cloudSpeed = 2f;
    private Queue<GameObject> cloudQueue = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < initialCloudCount; i++)
        {
            GameObject cloud = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)]);
            cloud.SetActive(false);
            cloudQueue.Enqueue(cloud);
        }

        GenerateInitialClouds();
    }

    void Update()
    {
        foreach (var cloud in cloudQueue)
        {
            if (cloud.activeSelf)
            {
                cloud.transform.position += Vector3.down * cloudSpeed * Time.deltaTime;

                if (cloud.transform.position.y < Camera.main.transform.position.y - 6f)
                {
                    cloud.SetActive(false);
                    SpawnCloud();
                }
            }
        }
    }

    void GenerateInitialClouds()
    {
        float lastY = 0;
        for (int i = 0; i < initialCloudCount; i++)
        {
            Vector3 newPos = GetRandomPosition(lastY);
            lastY = newPos.y;
            SpawnCloud(newPos);
        }
    }

    void SpawnCloud(Vector3 position = default)
    {
        GameObject cloud = cloudQueue.Dequeue();

        if (position == default)
        {
            position = GetRandomPosition(Camera.main.transform.position.y + 10f);
        }

        cloud.transform.position = position;
        cloud.SetActive(true);
        cloudQueue.Enqueue(cloud);
    }

    Vector3 GetRandomPosition(float lastY)
    {
        float randomX = Random.Range(-maxXRange, maxXRange);
        float randomY = lastY + Random.Range(minYDistance, maxYDistance);
        return new Vector3(randomX, randomY, 0f);
    }
}
