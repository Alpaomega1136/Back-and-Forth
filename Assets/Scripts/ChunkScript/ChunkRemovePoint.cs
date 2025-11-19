using UnityEngine;

public class ChunkExitPoint : MonoBehaviour
{   
    void OnDrawGizmos()
    {
        // Menggambar bola kuning di editor supaya gampang dilihat matamu
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}