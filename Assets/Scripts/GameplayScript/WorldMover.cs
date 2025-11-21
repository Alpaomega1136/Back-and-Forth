using UnityEngine;

public class WorldMover : MonoBehaviour
{
    void Update()
    {
        // Mencegah error jika Manager belum siap
        if (GameEventManager.Instance == null) return;

        // Ambil perintah kecepatan dari Pusat (GameEventManager)
        float speed = GameEventManager.Instance.baseMoveSpeed;
        float multiplier = GameEventManager.Instance.worldSpeedMultiplier;
        float dir = GameEventManager.Instance.worldDirection;

        // Bergerak
        transform.Translate(Vector2.left * (speed * multiplier * dir) * Time.deltaTime);
    }
}