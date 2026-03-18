using UnityEngine;

// This script controlls enemy waves in the game
// it desides when a wave starts, how many enemies spawn, where enemies spawn and when to move to the next wave

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera; //this is used so to know where the screen is so enemies spawn outside of it
    public Transform player; 
    public UpgradeManager upgradeManager;

    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs; //thats the array(list) of enemies to spawn

    [Header("Wave Settings")]
    public int startingEnemies = 4; //how many enemies the first wave starts with 
    public int enemiesPerWaveIncrease = 2; //how  many extar enemies are added each new wave
    public int maxWave = 100; //maximum number of waves

    [Header("Spawn Settings")]
    public float spawnMargin = 2f;        // how far outside the camera view
    public float spawnRingThickness = 3f; // thickness of the spawn ring

    [Header("Boss settings")]
    public GameObject bossPrefab;

    private int currentWave = 0; //this to keep track what wave are we on 
    private bool waveInProgress = false; 

    public int CurrentWave => currentWave; // this a public read-only so the ui reads the infomation 

    private void Awake()
    {
        if (mainCamera == null) //if we didnt assigned any camera is going to use the main camera
        {
            mainCamera = Camera.main;
        }

        if (player == null) // if no player was assigned in the inspector try to find the player using its tag
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private void Start()
    {
        StartNextWave(); // starts the very first wave when the game begins
    }

    private void Update()
    {
        if (!waveInProgress) //if the wave is not active, do nothing
            return;

        //simple approach: check how many enemies with tag "Enemy" are alive
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0) //if no enemies are alive 
        {
            waveInProgress = false; //mark the wave as finished

            if (currentWave < maxWave) //if we still have more waves to play
            {
                //Instead of starting immediately, ask the Upgrade Manager to take over
                if (upgradeManager != null)
                {
                    upgradeManager.PresentUpgrades();
                }
                else
                {
                    //Fallback if we forgot to hook it up
                    StartNextWave();
                }
            }
            else
            {
                Debug.Log("All waves have been completed! Game has finished."); //if no waves left prints this
            }
        }
    }

    public void StartNextWave()
    {
        currentWave++;

        //here we check if the wave is a boss wave(boss spawn every 5 waves: 5,10,15......) 
        if (currentWave % 5 == 0)
        {
            Debug.Log("BOSS WAVE STARTED!");
            SpawnBoss();
        }
        else
        {
            // --- NORMAL WAVE LOGIC ---
            int enemiesToSpawn = startingEnemies + (currentWave - 1) * enemiesPerWaveIncrease;
            Debug.Log($"Starting wave {currentWave}, spawning {enemiesToSpawn} enemies");
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnOneEnemy();
            }
        }

        waveInProgress = true;
    }

    private void SpawnOneEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) // this checks if enemyPrefabs exist
        {
            Debug.LogWarning("WaveManager: No enemy prefabs assigned!!!");
            return;
        }

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // pick a random enemy prefab from the array

        Vector3 spawnPos = GetSpawnPositionAroundCamera(); // get a random spawn position around the camera

        Instantiate(prefab, spawnPos, Quaternion.identity); // create the enemy in the scene
    }

    // *** FIXED: now returns Vector3 and uses offset ***
    private Vector3 GetSpawnPositionAroundCamera()
    {
        if (mainCamera == null) // sefety check
        {
            Debug.LogWarning("WaveManager: No camera assigned!!");
            return Vector3.zero;
        }

        Vector3 camPos = mainCamera.transform.position; // get the camera position

        // get camera size
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        // calculate radius from the center to screen corner
        float viewRadius = Mathf.Sqrt(halfWidth * halfWidth + halfHeight * halfHeight);

        //minimum and maximum distance from camera
        float minRadius = viewRadius + spawnMargin;
        float maxRadius = minRadius + spawnRingThickness;
        
        //pick a random angle
        float angle = Random.Range(0f, Mathf.PI * 2f);
        
        //pick a random distance in the spawn ring
        float radius = Random.Range(minRadius, maxRadius);

        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius; // convert angle + distance into x and y positions

        Vector3 spawnPos = camPos + offset; // final spawn position

        
        spawnPos.z = 0f;

        return spawnPos;
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null)
        {
            Vector3 spawnPos = GetSpawnPositionAroundCamera();
            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Boss Prefab is missing in WaveManager!");
        }
    }
}
