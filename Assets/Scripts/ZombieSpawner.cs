using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // Code taken from the Bond SHMUP Chapter
    [Header("Inscribed")]
    public bool spawnEnemies = true;

    public GameObject[] prefabEnemies;              // Array of Enemy prefabs
    
    public float enemySpawnPerSecond = 0.5f; // # of Enemies spawned/second
    
    public float boundsInset = 2f;   // Inset from the sides

    private int maxEnemiesAtOnce = 8;

    /*[Tooltip("Min/Max spawns per second - Minimum/Maximum spawn rate value for clamping the spawn per second value when it's modified by the difficulty settings")]
    public Vector2 minMaxSpawnPerSecond = new Vector2(0.5f, 1.5f);*/

    [Header("Dynamic")]
    [SerializeField] private bool Spawning = false;
    [SerializeField] private Vector3 bounds;
    [SerializeField] private List<GameObject> enemies;
    
    private GameObject spawnVolume;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnVolume = this.gameObject;
        bounds = spawnVolume.GetComponent<Collider>().bounds.size;
        // Invoke SpawnEnemy() once (in 2 seconds, based on default values)
        Invoke(nameof(SpawnEnemy), 6f);
    }

    public void SpawnEnemy()
    {
        // If spawnEnemies is false, skip to the next invoke of SpawnEnemy()
        if (!spawnEnemies)
        {
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
            return;
        }

        Spawning = true;

        // Set the initial position for the Enemy about to spawn
        Vector3 pos = Vector3.zero;
        
        pos.x = Random.Range(-(bounds.x * .5f) + boundsInset, (bounds.x * .5f) - boundsInset);
        pos.z = Random.Range(-(bounds.z * .5f) + boundsInset, (bounds.z * .5f) - boundsInset);

        // Pick a random Enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);

        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx],this.gameObject.transform.position + pos, Quaternion.identity);

        enemies.Add(go);


        //go.transform.position = pos;

        // Invoke SpawnEnemy() again
        if (enemies.Count <= maxEnemiesAtOnce)
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
        else
            Spawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (enemies.Count != 0) // Check if any enemies died if there are more than 1 around
        {
            for (int i = enemies.Count - 1; i > 0; i--)
            {
                if (enemies[i] == null)
                    enemies.RemoveAt(i); // Remove this GO from the list so that it can spawn new enemies
            }
        }

        // Not crazy about keeping these accurate and checking every frame so this one I'm alright with this code being here instead of Update.
        if (enemies.Count <= maxEnemiesAtOnce && !Spawning)
        { 
            Spawning = true;
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
        }
    }
}
