using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Setting Parallax")]
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f; // 0.5 = Setengah kecepatan lantai (Efek jauh)
    
    private float textureWidth;

    void Start()
    {
        // Hitung lebar gambar asli (sudah dikali scale)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        textureWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // Pastikan Game Manager siap
        if (GameEventManager.Instance == null) return;

        // 1. Ambil data kecepatan dari GameEventManager
        float speed = GameEventManager.Instance.baseMoveSpeed;
        float multiplier = GameEventManager.Instance.worldSpeedMultiplier;
        float direction = GameEventManager.Instance.worldDirection;

        // 2. Hitung kecepatan background (dikali parallaxFactor biar lebih pelan)
        float finalSpeed = speed * multiplier * direction * parallaxFactor;

        // 3. Gerakkan Background
        transform.Translate(Vector2.left * finalSpeed * Time.deltaTime);

        // 4. Logika Infinite Loop (Teleport)
        // Jika gambar sudah bergerak terlalu jauh ke kiri (melewati posisinya sendiri)
        // Kita asumsikan ada 2 gambar, jadi kita geser sejauh (2 * lebar) ke kanan
        if (transform.position.x < -textureWidth)
        {
            // Pindahkan ke belakang antrean
            transform.position += new Vector3(textureWidth * 2f, 0, 0);
        }
        
        // (Opsional) Reset jika arah mundur (Event Reverse)
        if (transform.position.x > textureWidth)
        {
             transform.position -= new Vector3(textureWidth * 2f, 0, 0);
        }
    }
}