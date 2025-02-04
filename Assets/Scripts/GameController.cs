using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{
    public GameObject weakEnemy;
    public GameObject mediumEnemy;
    public GameObject strongEnemy;
    public GameObject player;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreDisplay;
    public MeshRenderer background;
    public GameObject greyMeteor;
    public GameObject brownMeteor;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip backgroundMusic;
    public AudioClip gameOversfx;
    public AudioClip enemyDeathSfx;
    public AudioClip playerDeathSfx;
    [SerializeField]
    private float spawnRate = 5f; // Time until next enemy spawns
    [SerializeField]
    private float nextEnemy = 1f; // Countdown timer between enemy spawns
    [SerializeField]
    private float spawnRateDecrement = 0.1f; // Amount to decrease spawn rate over time
    [SerializeField]
    private float minSpawnRate = 0.5f; // Minimum spawn rate
    private float meteorSpawnRate = 4f;
    private float nextMeteor = 1f;
    private int totalScore = 0;
    private bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Play background music
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
        // Spawn player
        Instantiate(player, new Vector3(0, -1f, 0), Quaternion.identity);
        // Make sure panel is hidden when game starts
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if background music stops AND musicSource is not gameOver, player is dead, so play game over sfx
        if (!musicSource.isPlaying && musicSource.clip != gameOversfx){
            // play game over sfx one time
            musicSource.clip = gameOversfx;
            musicSource.loop = false;
            musicSource.Play();
        }
        // if game over, return
        if (isGameOver) return;
        // if player is destroyed, stop spawning enemies and end game
        if (GameObject.Find("Player(Clone)") == null){
            // destroy all enemies and powerups
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
            GameObject[] objects = enemies.Concat(powerups).ToArray();
            foreach (GameObject obj in objects)
            {
                Destroy(obj);
            }
            // if music is background music, stop it
            if (musicSource.clip == backgroundMusic) {
                musicSource.Stop();
            }
            // wait 1 second, end game
            Invoke("EndGame", 0.5f);
            return;
        } else {
            gameOverPanel.SetActive(false);
        }

        nextEnemy -= Time.deltaTime;
        nextMeteor -= Time.deltaTime;

        if (nextEnemy <= 0)
        {
            nextEnemy = spawnRate;
            SpawnEnemy();
        }
        if (nextMeteor <= 0)
        {
            nextMeteor = meteorSpawnRate;
            SpawnMeteor();
        }

        ReduceSpawnRate();

        // Offset background y cords for scrolling effect
        background.material.mainTextureOffset = new Vector2(0, Time.time / 8f);

        // Close game when escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CloseGame();
        }
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

    // spawn meteors
    void SpawnMeteor(){
        // Randomly select meteor type
        int random = Random.Range(0, 2);
        GameObject meteor;
        switch (random) {
            case 0:
                meteor = greyMeteor;
                break;
            case 1:
                meteor = brownMeteor;
                break;
            default:
                meteor = greyMeteor;
                break;
        }

        // Randomly select spawn position, above camera view (where player is facing)
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(6f, 7f);
        Vector3 spawnPosition = new Vector3(x, y, 0);

        // Spawn meteor
        Instantiate(meteor, spawnPosition, Quaternion.identity);
    }

    // when button is clicked, hide panel and spawn player
    public void RestartGame(){
        // spawn player
        Instantiate(player, new Vector3(0, -1f, 0), Quaternion.identity);
        // hide game over panel
        gameOverPanel.SetActive(false);
        isGameOver = false;
        // reset spawn rate
        spawnRate = 5f;
        nextEnemy = 1f;
        // reset score
        totalScore = 0;
        scoreDisplay.SetText("Score: " + totalScore);
        // stop gameOversfx, play background music on loop
        musicSource.Stop();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void AddScore(int score){
        totalScore += score;
        // Update score text
        scoreDisplay.SetText("Score: " + totalScore);
    }

    void EndGame() {
        gameOverPanel.SetActive(true);
        isGameOver = true;
    }
    void CloseGame(){
        Application.Quit();
    }
}
