using UnityEngine;

public class WorldMover : MonoBehaviour
{
    [Header("Kecepatan Dunia")]
    public float moveSpeed = 5f; // Kecepatan gerak ke kiri

    void Update()
    {
        // Menggerakkan objek ke arah KIRI (Vector2.left) setiap frame
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }
}