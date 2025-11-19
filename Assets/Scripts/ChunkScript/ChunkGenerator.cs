using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Daftar Semua Chunk")]
    public List<GameObject> chunkPrefabs; 

    [Header("Posisi Awal")]
    public Transform startPoint; 
    
    [Header("Pengaturan Jarak")]
    [Tooltip("Geser ini untuk mengatur kerapatan antar chunk. Jika menumpuk, besarkan angkanya.")]
    public float spawnAdjustmentX = 0f; // <-- INI FITUR BARUNYA
    
    // Variabel internal
    private Transform currentExitPoint; 
    public float spawnTriggerX = 0f;  

    void Start()
    {
        GameObject tempStart = new GameObject("Temp_Start");
        tempStart.transform.position = startPoint.position;
        currentExitPoint = tempStart.transform;

        SpawnRandomChunk();
        SpawnRandomChunk();
        SpawnRandomChunk();
    }

    void Update()
    {
        if (currentExitPoint != null && currentExitPoint.position.x < spawnTriggerX)
        {
            SpawnRandomChunk();
        }
    }

    void SpawnRandomChunk()
    {
        if (chunkPrefabs.Count == 0) return; 
        
        GameObject prefabToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];

        // --- BAGIAN PENGATUR JARAK ---
        
        // 1. Ambil posisi X dari Ekor Terakhir + OFFSET MANUAL
        // Tambahkan 'spawnAdjustmentX' untuk mendorong chunk baru lebih ke kanan
        float spawnX = currentExitPoint.position.x + spawnAdjustmentX;

        // 2. Ambil posisi Y dari Start Point (biar tetap lurus datar)
        float spawnY = startPoint.position.y;

        // 3. Gabungkan
        Vector3 finalSpawnPos = new Vector3(spawnX, spawnY, 0);

        // -----------------------------

        GameObject newChunk = Instantiate(prefabToSpawn, finalSpawnPos, Quaternion.identity);

        ChunkExitPoint newExit = newChunk.GetComponentInChildren<ChunkExitPoint>();

        if (newExit != null)
        {
            currentExitPoint = newExit.transform;
        }
        else
        {
            Debug.LogError("AWAS: Prefab ini lupa dikasih script ChunkExitPoint!");
        }
    }
}