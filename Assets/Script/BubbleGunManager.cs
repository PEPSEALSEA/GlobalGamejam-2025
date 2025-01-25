using UnityEngine;
using System.Collections.Generic;

public class BubbleGunManager : MonoBehaviour
{
    public static BubbleGunManager Instance { get; private set; } // Singleton

    public GameObject bubblePrefab;
    public int initialBubbleCount = 20;
    public float minYDistance = 2f;
    public float maxYDistance = 4f;
    public float maxXRange = 5f;
    public float bubbleSpeed = 2f;
    private Queue<GameObject> bubbleQueue = new Queue<GameObject>();

    private float spawnTimer;

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

    void Start()
    {
        spawnTimer = Random.Range(20f, 30f);

        // Initialize the queue with inactive bubbles
        for (int i = 0; i < initialBubbleCount; i++)
        {
            GameObject bubble = Instantiate(bubblePrefab);
            bubble.SetActive(false);
            bubbleQueue.Enqueue(bubble);
        }

        // Spawn initial bubbles (not really necessary in this case, just for consistency)
        GenerateInitialBubbles();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnBubble();
            spawnTimer = Random.Range(20f, 30f); // Reset the spawn timer for the next bubble
        }

        // Move active bubbles down like clouds
        List<GameObject> activeBubbles = new List<GameObject>(bubbleQueue);
        foreach (var bubble in activeBubbles)
        {
            if (bubble != null && bubble.activeSelf) // Ensure bubble is not destroyed
            {
                bubble.transform.position += Vector3.down * bubbleSpeed * Time.deltaTime;

                // If the bubble goes out of the screen, reset it
                if (bubble.transform.position.y < Camera.main.transform.position.y - 6f)
                {
                    bubble.SetActive(false);
                    SpawnBubble();
                }
            }
        }
    }


    void GenerateInitialBubbles()
    {
        float lastY = 0;
        for (int i = 0; i < initialBubbleCount; i++)
        {
            Vector3 newPos = GetRandomPosition(lastY);
            lastY = newPos.y;
            SpawnBubble(newPos);
        }
    }

    void SpawnBubble(Vector3 position = default)
    {
        GameObject bubble = bubbleQueue.Dequeue();

        if (position == default)
        {
            position = GetRandomPosition(Camera.main.transform.position.y + 10f);
        }

        bubble.transform.position = position;
        bubble.SetActive(true);
        bubbleQueue.Enqueue(bubble);
    }

    Vector3 GetRandomPosition(float lastY)
    {
        float randomX = Random.Range(-maxXRange, maxXRange);
        float randomY = lastY + Random.Range(minYDistance, maxYDistance);
        return new Vector3(randomX, randomY, 0f);
    }
}
