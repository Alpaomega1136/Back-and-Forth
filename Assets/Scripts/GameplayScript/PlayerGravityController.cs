using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    [Header("Pengaturan Fisika")]
    public float defaultGravityScale = 5f;

    [Header("Pengaturan Deteksi")]
    public Transform groundCheckTop;    // Sensor Kepala
    public Transform groundCheckBottom; // Sensor Kaki
    public LayerMask groundLayer;       
    public float groundCheckRadius = 0.3f; 

    // --- STATE VARIABLES ---
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim; // Referensi ke Animator
    
    // Status Deteksi
    [SerializeField] private bool touchingBottom; 
    [SerializeField] private bool touchingTop;    
    
    [SerializeField] private bool isGravityInverted = false;

    // DEBUG MODE (Event Zero Gravity)
    [Header("Debug Event")]
    [SerializeField] private bool isZeroGravityMode = false;
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); // Ambil komponen Animator

        ApplyGravitySettings();
    }

    void Update()
    {
        // 1. Update Gravitasi (Normal / Zero G)
        if (isZeroGravityMode)
        {
            rb.gravityScale = 0; 
        }
        else
        {
            rb.gravityScale = isGravityInverted ? -defaultGravityScale : defaultGravityScale;
        }

        // 2. Input Player (Hanya Spasi)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleInput();
        }

        // 3. Update Animasi (Kirim data ke Animator)
        UpdateAnimationState();
    }

    void FixedUpdate()
    {
        CheckSensors(); 
        ApplyMovementCorrection();
    }

    // ---------------- LOGIKA UTAMA ----------------

    void CheckSensors()
    {
        if (groundCheckBottom != null)
            touchingBottom = Physics2D.OverlapCircle(groundCheckBottom.position, groundCheckRadius, groundLayer);
        
        if (groundCheckTop != null)
            touchingTop = Physics2D.OverlapCircle(groundCheckTop.position, groundCheckRadius, groundLayer);
    }

    void HandleInput()
    {
        // Saat Zero Gravity (Terbang Bebas)
        if (isZeroGravityMode)
        {
            FlipGravityState();
            // Dorongan kecil agar bergerak di udara
            float push = isGravityInverted ? 5f : -5f;
            rb.linearVelocity = new Vector2(0, push);
        }
        // Saat Normal (Harus Napak)
        else if (!isGravityInverted && touchingBottom)
        {
            FlipGravityState();
        }
        else if (isGravityInverted && touchingTop)
        {
            FlipGravityState();
        }
    }

    void FlipGravityState()
    {
        isGravityInverted = !isGravityInverted;
        rb.linearVelocity = Vector2.zero; // Reset kecepatan biar responsif
        
        // Balik Gambar (Visual)
        if (sr != null) sr.flipY = isGravityInverted;

        SoundManager.Instance.PlayGravityChangeSFX();
    }

    void ApplyMovementCorrection()
    {
        // Kunci posisi X agar player tidak maju/mundur (Dunia yang bergerak)
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void ApplyGravitySettings()
    {
        if (!isZeroGravityMode)
            rb.gravityScale = isGravityInverted ? -defaultGravityScale : defaultGravityScale;
    }

    // ---------------- LOGIKA ANIMASI (BARU) ----------------

    void UpdateAnimationState()
    {
        if (anim == null) return;

        // 1. Kirim IsGrounded
        bool isGrounded = touchingBottom || touchingTop;
        anim.SetBool("IsGrounded", isGrounded);

        // 2. Kirim VerticalSpeed
        float verticalSpeed = rb.linearVelocity.y * Mathf.Sign(rb.gravityScale);
        anim.SetFloat("VerticalSpeed", verticalSpeed);

        // 3. [BARU] Kirim Status Zero Gravity
        // Supaya Animator tahu: "Oh, ini lagi mode terbang, boleh naik turun bebas"
        anim.SetBool("IsZeroGravity", isZeroGravityMode);
    }

    // ---------------- EVENT SYSTEM HOOKS ----------------

    public void SetZeroGravity(bool active)
    {
        isZeroGravityMode = active;
        rb.linearVelocity = Vector2.zero; 
    }

    public void TriggerForcedFlip()
    {
        FlipGravityState();
    }

    // ---------------- GAME OVER LOGIC ----------------

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Kena Rintangan (Paku/Tembok)
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("GAME OVER! Kena Obstacle.");
            CallGameOver();
        }
        // 2. Jatuh Keluar Layar (DeathZone)
        else if (other.CompareTag("DeathZone"))
        {
            Debug.Log("GAME OVER! Jatuh Keluar Layar.");
            CallGameOver();
        }
    }

    void CallGameOver()
    {
        // Panggil Manager untuk tampilkan Panel & Hentikan Waktu
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.GameOver();
        }
        
        // Sembunyikan Player
        gameObject.SetActive(false);
    }

    // ---------------- DEBUGGING ----------------
    void OnDrawGizmos()
    {
        if (groundCheckBottom != null)
        {
            Gizmos.color = Color.red;
            if (touchingBottom) Gizmos.DrawSphere(groundCheckBottom.position, groundCheckRadius);
            else Gizmos.DrawWireSphere(groundCheckBottom.position, groundCheckRadius);
        }

        if (groundCheckTop != null)
        {
            Debug.Log("GAME OVER! Kena Paku.");

            // Panggil fungsi GameOver di GameEventManager
            // Ini akan memunculkan Panel dan menghentikan Skor
            if (GameEventManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.collision);
                GameEventManager.Instance.GameOver();
            }
            else
            {
                // Jaga-jaga kalau lupa pasang GameEventManager
                Debug.LogError("GameEventManager belum dipasang di Scene!");
            }
            
            // Matikan Player (Supaya hilang dari layar)
            gameObject.SetActive(false);
            Gizmos.color = Color.blue;
            if (touchingTop) Gizmos.DrawSphere(groundCheckTop.position, groundCheckRadius);
            else Gizmos.DrawWireSphere(groundCheckTop.position, groundCheckRadius);
        }
    }
}