using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject cloudPrefab;
    public int initialCloudCount = 10;
    public float spawnXMin = -5f;
    public float spawnXMax = 5f;
    public float spawnYMin = 5f;
    public float spawnYMax = 10f;
    public float cloudSpeed = 2f;
    public float minimumDistance = 2f;

    private List<GameObject> cloudPool = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < initialCloudCount; i++)
        {
            GameObject cloud = Instantiate(cloudPrefab);
            cloud.SetActive(false);
            cloudPool.Add(cloud);
        }

        for (int i = 0; i < initialCloudCount; i++)
        {
            ActivateCloud(cloudPool[i]);
        }
    }

    private void Update()
    {
        foreach (GameObject cloud in cloudPool)
        {
            if (cloud.activeSelf)
            {
                cloud.transform.position += Vector3.down * cloudSpeed * Time.deltaTime;

                if (!IsInCameraView(cloud))
                {
                    cloud.SetActive(false);
                }
            }
        }

        EnsureActiveClouds();
    }

    private bool IsInCameraView(GameObject cloud)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(cloud.transform.position);
        return screenPoint.y > 0;
    }

    private void EnsureActiveClouds()
    {
        int activeClouds = 0;

        foreach (GameObject cloud in cloudPool)
        {
            if (cloud.activeSelf)
            {
                activeClouds++;
            }
        }

        while (activeClouds < initialCloudCount)
        {
            foreach (GameObject cloud in cloudPool)
            {
                if (!cloud.activeSelf)
                {
                    ActivateCloud(cloud);
                    activeClouds++;
                    break;
                }
            }

            if (activeClouds < initialCloudCount)
            {
                GameObject newCloud = Instantiate(cloudPrefab);
                newCloud.SetActive(false);
                cloudPool.Add(newCloud);
            }
        }
    }

    private void ActivateCloud(GameObject cloud)
    {
        Vector3 newPosition;
        int attempts = 0;
        bool validPosition;

        do
        {
            newPosition = new Vector3(
                Random.Range(spawnXMin, spawnXMax),
                Random.Range(spawnYMin, spawnYMax),
                0f);

            validPosition = IsPositionValid(newPosition);

            attempts++;
        } while (!validPosition && attempts < 100);

        cloud.transform.position = newPosition;
        cloud.SetActive(true);
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (GameObject cloud in cloudPool)
        {
            if (cloud.activeSelf)
            {
                float yDistance = Mathf.Abs(position.y - cloud.transform.position.y);
                if (yDistance < minimumDistance)
                {
                    return false;
                }
            }
        }
        return true;
    }


}
