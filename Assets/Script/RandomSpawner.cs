using UnityEngine;
using System.Collections;

public class RandomSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public Color gizmoColor = Color.green;
    public float minSec = 10f;
    public float maxSec = 20f;

    private GameObject currentSpawnedObject;
    private float spawnInterval;

    private void Start()
    {
        StartCoroutine(SpawnObjectRoutine());
    }

    private IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            spawnInterval = Random.Range(minSec, minSec);
            yield return new WaitForSeconds(spawnInterval);

            if (currentSpawnedObject != null)
            {
                Destroy(currentSpawnedObject);
            }

            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            currentSpawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Vector3 bottomLeft = new Vector3(spawnAreaMin.x, spawnAreaMin.y, 0);
        Vector3 bottomRight = new Vector3(spawnAreaMax.x, spawnAreaMin.y, 0);
        Vector3 topLeft = new Vector3(spawnAreaMin.x, spawnAreaMax.y, 0);
        Vector3 topRight = new Vector3(spawnAreaMax.x, spawnAreaMax.y, 0);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);

        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.2f);
        Gizmos.DrawCube((bottomLeft + topRight) / 2, new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 0));
    }
}
