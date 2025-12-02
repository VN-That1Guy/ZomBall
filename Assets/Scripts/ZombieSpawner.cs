using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // Code taken from the Bond SHMUP Chapter
    [Header("Inscribed")]
    public bool spawnEnemies = true;

    public GameObject[] prefabEnemies;              // Array of Enemy prefabs
    
    public float enemySpawnPerSecond = 3f; // # of Enemies spawned/second
    
    public float boundsInset = 2f;   // Inset from the sides
    
    /*[Tooltip("Min/Max spawns per second - Minimum/Maximum spawn rate value for clamping the spawn per second value when it's modified by the difficulty settings")]
    public Vector2 minMaxSpawnPerSecond = new Vector2(0.5f, 1.5f);*/

    [Header("Dynamic")]
    [SerializeField] private Vector3 bounds;

    private GameObject spawnVolume;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnVolume = this.gameObject;
        bounds = spawnVolume.GetComponent<Collider>().bounds.size;
        // Invoke SpawnEnemy() once (in 2 seconds, based on default values)
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    public void SpawnEnemy()
    {
        // If spawnEnemies is false, skip to the next invoke of SpawnEnemy()
        if (!spawnEnemies)
        {
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
            return;
        }

        // Set the initial position for the Enemy about to spawn
        Vector3 pos = Vector3.zero;
        
        pos.x = Random.Range(-bounds.x + boundsInset, bounds.x - boundsInset);
        pos.z = Random.Range(-bounds.z + boundsInset, bounds.z - boundsInset);

        // Pick a random Enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);

        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx],this.gameObject.transform.position + pos, Quaternion.identity);

        

        
        //go.transform.position = pos;

        // Invoke SpawnEnemy() again
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
