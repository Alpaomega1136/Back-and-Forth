using UnityEngine;
using System.Collections;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance; // Singleton biar mudah dipanggil

    [Header("Referensi")]
    public PlayerGravityController player;
    // Kita pakai array karena mungkin nanti ada banyak object yang bergerak (Lantai, Background, dll)
    public WorldMover[] worldMovers; 
    public Camera mainCamera;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Cari referensi otomatis jika kosong (Safety)
        if (player == null) player = FindFirstObjectByType<PlayerGravityController>();
        if (worldMovers.Length == 0) worldMovers = FindObjectsByType<WorldMover>(FindObjectsSortMode.None);
        if (mainCamera == null) mainCamera = Camera.main;
    }

    // --- DAFTAR FUNGSI EVENT ---

    // 1. EVENT: ZERO GRAVITY (Terbang Bebas)
    public void TriggerZeroGravity(float duration)
    {
        StartCoroutine(ZeroGravityRoutine(duration));
    }

    IEnumerator ZeroGravityRoutine(float duration)
    {
        Debug.Log("EVENT: Zero Gravity Start!");
        player.SetZeroGravity(true); // Panggil fungsi di script Player
        
        yield return new WaitForSeconds(duration); // Tunggu...
        
        player.SetZeroGravity(false); // Matikan
        Debug.Log("EVENT: Zero Gravity End.");
    }

    // 2. EVENT: SPEED CHANGE (Cepat / Lambat)
    public void TriggerSpeedChange(float multiplier, float duration)
    {
        StartCoroutine(SpeedChangeRoutine(multiplier, duration));
    }

    IEnumerator SpeedChangeRoutine(float multiplier, float duration)
    {
        Debug.Log($"EVENT: Speed x{multiplier} Start!");
        
        // Ubah semua benda yang bergerak (Lantai, BG, dll)
        foreach (var mover in worldMovers)
        {
            if(mover != null) mover.speedMultiplier = multiplier;
        }

        yield return new WaitForSeconds(duration);

        // Reset ke normal (1)
        foreach (var mover in worldMovers)
        {
             if(mover != null) mover.speedMultiplier = 1f;
        }
        Debug.Log("EVENT: Speed Normal.");
    }

    // 3. EVENT: REVERSE DIRECTION (Mundur)
    public void TriggerReverseDirection(float duration)
    {
        StartCoroutine(ReverseRoutine(duration));
    }

    IEnumerator ReverseRoutine(float duration)
    {
        Debug.Log("EVENT: Mundur!");
        
        // Ubah arah gerak dunia jadi kebalikan (-1)
        foreach (var mover in worldMovers)
        {
             if(mover != null) mover.direction = -1f;
        }

        yield return new WaitForSeconds(duration);

        // Reset ke normal (1)
        foreach (var mover in worldMovers)
        {
             if(mover != null) mover.direction = 1f;
        }
    }

    // 4. EVENT: LAYAR TERBALIK (Screen Rotate)
    public void TriggerScreenRotate(float duration)
    {
        StartCoroutine(ScreenRotateRoutine(duration));
    }

    IEnumerator ScreenRotateRoutine(float duration)
    {
        Debug.Log("EVENT: Pusing!");
        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 180); // Putar 180 derajat

        yield return new WaitForSeconds(duration);

        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset
    }
    
    // 5. EVENT: FLIP GRAVITASI PAKSA
    public void TriggerForcedFlip()
    {
        player.TriggerForcedFlip();
    }
}