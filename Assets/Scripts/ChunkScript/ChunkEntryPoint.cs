using UnityEngine;

public class ChunkEntryPoint : MonoBehaviour
{
    // Script ini hanya sebagai marker
    // Letakkan di child object yang menandai titik awal/entry chunk
    
    private void OnDrawGizmos()
    {
        // Gambar gizmo hijau untuk entry point
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2);
    }
}