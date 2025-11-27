using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Daftar Semua Chunk")]
    public List<GameObject> chunkPrefabs; 
    public List<GameObject> eventChunkPrefabs;

    [Header("Semua Item Event Object")]
    public List<GameObject> eventItems;
    
    public Transform eventItemSpawnPoint;

    [Header("Posisi Awal")]
    public Transform startPoint; 
    
    [Header("Pengaturan Jarak")]
    [Tooltip("Geser ini untuk mengatur kerapatan antar chunk. Jika menumpuk, besarkan angkanya.")]
    public float spawnAdjustmentX = 0f; // <-- INI FITUR BARUNYA
    
    // Variabel internal
    private Transform currentExitPoint; 
    public float spawnTriggerX = 0f;  

    private int eventCounter = 0;// Sementara setiap 5 chunk

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
            eventCounter++;
            Debug.Log("Event Counter: " + eventCounter);
            if (eventCounter > 5) eventCounter = 0;
            SpawnRandomChunk(eventCounter == 5);
        }
    }

    void SpawnRandomChunk(bool eventChunk = false)
    {
        if (chunkPrefabs.Count == 0) return; 
        
        GameObject prefabToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        if (eventChunk && eventChunkPrefabs.Count > 0)
        {
            Debug.Log("Spawning Event Chunk");
            prefabToSpawn = eventChunkPrefabs[Random.Range(0, eventChunkPrefabs.Count)];
        }

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

        Transform eventItemPoint = newChunk.transform.Find("EventItemSpawn");
            
        if (eventChunk && eventItemPoint != null && eventItems.Count > 0)
        {
            Debug.Log("Spawning Event Item");
            GameObject eventItemPrefab = eventItems[Random.Range(0, eventItems.Count)];
            if (eventItemPrefab == null)
            {
                Debug.LogError("Event item prefab is NULL or destroyed!");
            }
            Instantiate(eventItemPrefab, eventItemPoint.position, Quaternion.identity, newChunk.transform);
        }

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