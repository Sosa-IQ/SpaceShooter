using UnityEngine;


public class GameController : MonoBehaviour
{
    public GameObject weakEnemy;
    public GameObject mediumEnemy;
    public GameObject strongEnemy;
    [SerializeField]
    private float spawnRate = 5f; // Time until next enemy spawns
    [SerializeField]
    private float nextEnemy = 0f; // Countdown timer between enemy spawns
    [SerializeField]
    private float spawnRateDecrement = 0.1f; // Amount to decrease spawn rate over time
    [SerializeField]
    private float minSpawnRate = 0.5f; // Minimum spawn rate

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nextEnemy -= Time.deltaTime;

        if (nextEnemy <= 0)
        {
            nextEnemy = spawnRate;
            SpawnEnemy();
        }

        ReduceSpawnRate();
    }

    void SpawnEnemy(){
        // Randomly select enemy type
        // weaker enemies have higher chance of spawning
        float random = Random.Range(0f, 1f);
        GameObject enemy;
        if (random < 0.65f)
        {
            enemy = weakEnemy;
        }
        else if (random < 0.85f)
        {
            enemy = mediumEnemy;
        }
        else
        {
            enemy = strongEnemy;
        }

        // Randomly select spawn position, above camera view (where player is facing)
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(6f, 7f);
        Vector3 spawnPosition = new Vector3(x, y, 0);

        // Spawn enemeny
        Instantiate(enemy, spawnPosition, Quaternion.identity);
    }

    void ReduceSpawnRate(){
        // Reduce spawn rate over time
        if (spawnRate > minSpawnRate)
        {
            spawnRate -= spawnRateDecrement * Time.deltaTime;
            if (spawnRate < minSpawnRate)
            {
                spawnRate = minSpawnRate;
            }
        }
    }
}
