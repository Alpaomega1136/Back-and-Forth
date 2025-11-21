using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerGravityController : MonoBehaviour
{
    [Header("Pengaturan Fisika")]
    public float defaultGravityScale = 5f;

    [Header("Pengaturan Deteksi")]
    public Transform groundCheckTop;    // Sensor KEPALA
    public Transform groundCheckBottom; // Sensor KAKI
    public LayerMask groundLayer;       
    public float groundCheckRadius = 0.3f; // Saya perbesar sedikit agar lebih gampang kena

    // --- STATE VARIABLES ---
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    
    // Status Deteksi Murni (Fakta Fisika)
    [SerializeField] private bool touchingBottom; // Apakah kaki kena lantai?
    [SerializeField] private bool touchingTop;    // Apakah kepala kena langit-langit?

    // Status Game
    [SerializeField] private bool isGravityInverted = false;

    // DEBUG MODE (Centang ini di Inspector untuk tes mode Zero Gravity)
    [Header("Debug Event")]
    [SerializeField] private bool isZeroGravityMode = false;
 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // Pastikan gravitasi awal benar
        ApplyGravitySettings();
    }

    void Update()
    {
        // Update gravitasi setiap saat
        if (isZeroGravityMode)
        {
            rb.gravityScale = 0; 
        }
        else
        {
            rb.gravityScale = isGravityInverted ? -defaultGravityScale : defaultGravityScale;
        }

        // INPUT PLAYER (Hanya Space)
        // Menghapus Input.GetMouseButtonDown(0) agar UI tidak tembus
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleInput();
        }
    }

    void FixedUpdate()
    {
        CheckSensors(); // Cek sensor terus menerus
        ApplyMovementCorrection();
    }

    // ---------------- 1. LOGIKA DETEKSI (MURNI FISIKA) ----------------
    // Bagian ini hanya bertugas melaporkan: "Kena atau Tidak?"
    // Tidak ada logika game di sini.
    void CheckSensors()
    {
        if (groundCheckBottom != null)
            touchingBottom = Physics2D.OverlapCircle(groundCheckBottom.position, groundCheckRadius, groundLayer);
        
        if (groundCheckTop != null)
            touchingTop = Physics2D.OverlapCircle(groundCheckTop.position, groundCheckRadius, groundLayer);
    }

    // ---------------- 2. LOGIKA INPUT (IZIN BERGERAK) ----------------
    // Di sini kita tentukan "Boleh Flip atau Tidak?"
    void HandleInput()
    {
        // KONDISI 1: MODE ZERO GRAVITY (EVENT)
        // Boleh flip kapan saja, tidak peduli sensor
        if (isZeroGravityMode)
        {
            FlipGravityState(); // Ubah status Inverted/Normal
            
            // Beri dorongan karena tidak ada gravitasi
            float push = isGravityInverted ? 5f : -5f;
            rb.linearVelocity = new Vector2(0, push);
        }
        
        // KONDISI 2: MODE NORMAL (GRAVITASI KE BAWAH)
        // Hanya boleh flip kalau Sensor BAWAH kena tanah
        else if (!isGravityInverted && touchingBottom)
        {
            FlipGravityState();
        }

        // KONDISI 3: MODE TERBALIK (GRAVITASI KE ATAS)
        // Hanya boleh flip kalau Sensor ATAS kena langit-langit
        else if (isGravityInverted && touchingTop)
        {
            FlipGravityState();
        }
    }

    // Fungsi untuk membalik status dan visual
    void FlipGravityState()
    {
        isGravityInverted = !isGravityInverted;
        
        // Reset kecepatan biar langsung 'snappy'
        rb.linearVelocity = Vector2.zero; 

        // Balik Gambarnya (Visual)
        if (sr != null) sr.flipY = isGravityInverted;
    }

    void ApplyMovementCorrection()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void ApplyGravitySettings()
    {
        if (!isZeroGravityMode)
            rb.gravityScale = isGravityInverted ? -defaultGravityScale : defaultGravityScale;
    }

    // Event System Hook
    public void SetZeroGravity(bool active)
    {
        isZeroGravityMode = active;
        // Reset velocity saat masuk/keluar mode supaya tidak 'terlempar' sisa momentum
        rb.linearVelocity = Vector2.zero; 
    }

    public void TriggerForcedFlip()
    {
        FlipGravityState();
    }

    // Visualisasi Debug (Biru = Atas, Merah = Bawah)
    // Jika Bola penuh (Solid) artinya KENA. Jika cuma Garis (Wire) artinya TIDAK KENA.
    void OnDrawGizmos()
    {
        if (groundCheckBottom != null)
        {
            Gizmos.color = Color.red;
            if (touchingBottom) Gizmos.DrawSphere(groundCheckBottom.position, groundCheckRadius); // Kena
            else Gizmos.DrawWireSphere(groundCheckBottom.position, groundCheckRadius); // Tidak Kena
        }

        if (groundCheckTop != null)
        {
            Gizmos.color = Color.blue;
            if (touchingTop) Gizmos.DrawSphere(groundCheckTop.position, groundCheckRadius); // Kena
            else Gizmos.DrawWireSphere(groundCheckTop.position, groundCheckRadius); // Tidak Kena
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Jika menabrak Rintangan (Paku)
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("GAME OVER! Kena Paku.");

            // Panggil fungsi GameOver di GameEventManager
            // Ini akan memunculkan Panel dan menghentikan Skor
            if (GameEventManager.Instance != null)
            {
                GameEventManager.Instance.GameOver();
            }
            else
            {
                // Jaga-jaga kalau lupa pasang GameEventManager
                Debug.LogError("GameEventManager belum dipasang di Scene!");
            }
            
            // Matikan Player (Supaya hilang dari layar)
            gameObject.SetActive(false);
        }
    }
}