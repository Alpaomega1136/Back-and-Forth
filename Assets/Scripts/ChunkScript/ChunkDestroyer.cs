using UnityEngine;

public class ChunkDestroyer : MonoBehaviour
{
    public float destroyXPosition = -25f; // Titik X di mana chunk akan dihapus

    void Update()
    {
        // Karena WorldMover menggerakkan chunk ke kiri, nilai X akan makin kecil (negatif)
        // Jika posisi X sudah lebih kecil dari batas (misal -25), hapus objek ini
        if (transform.position.x < destroyXPosition)
        {
            Destroy(gameObject);
        }
    }
}