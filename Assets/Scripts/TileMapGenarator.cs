using System.Collections.Generic; // lets us use Dictionary, list and HashSet
using UnityEngine;

//This script creates an INFINITY 2D tilemap system
//it spawn tiles(chunks) around the player and removes chunks that are far way to save performance


public class InfiniteTilemapGenerator2D : MonoBehaviour
{
    [Header("Chunk Settings")]
    public GameObject[] chunkPrefabs;   // a list of tilemap chunk prefabs Unity can spawn

    public float chunkWidth = 24f;      // width of a chunk
    public float chunkHeight = 16f;     // height of a chunk
    public int viewRadius = 2;          // how many chunks out from the player

    [Header("Player")]
    public Transform player;   //the player that the chunks will genarate around         

    // a dictionary that keeps track of which chunk have been spawned
    //key = chunk grid position (x, y), Value = the chunk GameObject in the world
    private Dictionary<Vector2Int, GameObject> spawnedChunks = new Dictionary<Vector2Int, GameObject>();
    //the chunk the player is currently inside 
    private Vector2Int currentCenterChunk;

    void Start()
    {
        //Make sure the player object is assigned otherwise this system cannot work
        if (player == null)
        {
            Debug.LogError("InfiniteTilemapGenerator2D: Player not assigned!");
            enabled = false; // turn of the script so it dosnt keep running with errors
            return;
        }
        //find which chunk the player starts in
        currentCenterChunk = GetChunkCoord(player.position);
        //spawn the cchunks around the player
        UpdateVisibleChunks();
    }

    void Update()
    {
        if (player == null) return;

        // check if the player moved to a new chunk
        Vector2Int newCenter = GetChunkCoord(player.position);
        // if we have moved into a new chunk update which chunks should exist
        if (newCenter != currentCenterChunk)
        {
            currentCenterChunk = newCenter;
            UpdateVisibleChunks();
        }
    }

    //Converts a world position into chunk grid coordinates
    //
    Vector2Int GetChunkCoord(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / chunkWidth);
        int y = Mathf.FloorToInt(worldPos.y / chunkHeight);
        return new Vector2Int(x, y);
    }

    //handles spawning the right chunks around the player and removing the ones that are far away from the player
    void UpdateVisibleChunks()
    {
        // a list of chunks coordinates that should have spawned
        HashSet<Vector2Int> neededCoords = new HashSet<Vector2Int>();

        //loop through all chunks within view redius
      for (int y = -viewRadius; y <= viewRadius; y++)
        {
            for (int x = -viewRadius; x <= viewRadius; x++)
            {
                Vector2Int coord = new Vector2Int(currentCenterChunk.x + x,
                                                  currentCenterChunk.y + y);
                neededCoords.Add(coord);

                // if this chunk dosnt exist yet spawn it
                if (!spawnedChunks.ContainsKey(coord))
                {
                    SpawnChunk(coord);
                }
            }
        }

        // find chunks that are no longer within view radius and remove them
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kvp in spawnedChunks)
        {
            // if this chunk is not needed anymore mark it for deletion
            if (!neededCoords.Contains(kvp.Key))
            {
                Destroy(kvp.Value); //remove the chunk from the scene
                toRemove.Add(kvp.Key);
            }
        }
        // actually remove the old chunks from our dictionary
        foreach (Vector2Int coord in toRemove)
        {
            spawnedChunks.Remove(coord);
        }
    }

    //spawn a chunk at given grid coordinate
    void SpawnChunk(Vector2Int coord)
    {
        //make sure at least one chunk prefab is assigned
        if (chunkPrefabs.Length == 0)
        {
            Debug.LogWarning("InfiniteTilemapGenerator2D: No chunk prefabs assigned!");
            return;
        }
        //randomly choose a chunk variation to place
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];

        //convert the chunk grid coordinates into an actual world position
        Vector3 worldPos = new Vector3(coord.x * chunkWidth,
                                       coord.y * chunkHeight,
                                       0f);

        //instatiate = create a copy of the prefab in the world
        GameObject chunk = Instantiate(prefab, worldPos, Quaternion.identity, transform);
        //record that we spawned this chunk so we can track and delete it later
        spawnedChunks.Add(coord, chunk);
    }
}
