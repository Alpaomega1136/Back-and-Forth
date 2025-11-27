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

        // Spawn chunk di posisi sementara untuk mengukur offset entry point
        GameObject newChunk = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        
        // Cari entry point di chunk baru
        ChunkEntryPoint entryPoint = newChunk.GetComponentInChildren<ChunkEntryPoint>();
        
        if (entryPoint == null)
        {
            Debug.LogError($"AWAS: Prefab '{prefabToSpawn.name}' tidak punya ChunkEntryPoint! Gunakan posisi chunk sebagai gantinya.");
            // Fallback: gunakan posisi chunk itu sendiri
            Vector3 fallbackPos = new Vector3(
                currentExitPoint.position.x + spawnAdjustmentX,
                startPoint.position.y,
                0
            );
            newChunk.transform.position = fallbackPos;
        }
        else
        {
            // Hitung offset antara entry point dan pivot chunk
            Vector3 entryOffset = entryPoint.transform.position - newChunk.transform.position;
            
            // Posisi akhir: exit point sebelumnya - offset entry point + spacing
            Vector3 finalSpawnPos = new Vector3(
                currentExitPoint.position.x - entryOffset.x + spawnAdjustmentX,
                startPoint.position.y - entryOffset.y,
                0
            );
            
            newChunk.transform.position = finalSpawnPos;
        }

        // Spawn event item jika ada
        Transform eventItemPoint = newChunk.transform.Find("EventItemSpawn");
            
        if (eventChunk && eventItemPoint != null && eventItems.Count > 0)
        {
            Debug.Log("Spawning Event Item");
            GameObject eventItemPrefab = eventItems[Random.Range(0, eventItems.Count)];
            if (eventItemPrefab != null)
            {
                Instantiate(eventItemPrefab, eventItemPoint.position, Quaternion.identity, newChunk.transform);
            }
            else
            {
                Debug.LogError("Event item prefab is NULL!");
            }
        }

        // Update exit point untuk chunk selanjutnya
        ChunkExitPoint newExit = newChunk.GetComponentInChildren<ChunkExitPoint>();

        if (newExit != null)
        {
            currentExitPoint = newExit.transform;
        }
        else
        {
            Debug.LogError($"AWAS: Prefab '{prefabToSpawn.name}' lupa dikasih script ChunkExitPoint!");
        }
    }
}