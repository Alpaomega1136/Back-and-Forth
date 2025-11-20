using UnityEngine;

public class WorldMover : MonoBehaviour
{
    [Header("Setting Dasar")]
    public float moveSpeed = 5f; 

    // --- TAMBAHAN BARU (Sembunyi di Inspector tapi bisa diakses script lain) ---
    [HideInInspector] public float speedMultiplier = 1f; 
    [HideInInspector] public float direction = 1f;       

    void Update()
    {
        // Rumus: Speed Dasar x Pengali (Cepat/Lambat) x Arah (Maju/Mundur)
        float finalSpeed = moveSpeed * speedMultiplier * direction;
        
        transform.Translate(Vector2.left * finalSpeed * Time.deltaTime);
    }
}