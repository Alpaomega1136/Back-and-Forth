using UnityEngine;

public class EventItem : MonoBehaviour
{
    // Pilihan event biar gampang dipilih di Inspector
    public enum EventType 
    { 
        ZeroGravity, 
        SpeedUp, 
        SlowDown, 
        Reverse, 
        ScreenRotate,
        ForcedFlip
    }

    [Header("Konfigurasi Event")]
    public EventType type;
    public float duration = 5f; // Berapa lama efeknya?

    [Header("Visual")]
    public GameObject pickupEffect; // (Opsional) Partikel saat diambil

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah yang nabrak itu Player (pastikan Player punya tag "Player")
        if (other.CompareTag("Player"))
        {
            ActivateEvent();
            
            // Efek visual (kalau ada)
            if(pickupEffect != null) Instantiate(pickupEffect, transform.position, Quaternion.identity);

            // Hapus item ini (seolah diambil)
            Destroy(gameObject);
        }
    }

    void ActivateEvent()
    {
        // Panggil GameEventManager sesuai tipe yang dipilih
        switch (type)
        {
            case EventType.ZeroGravity:
                GameEventManager.Instance.TriggerZeroGravity(duration);
                break;
            
            case EventType.SpeedUp:
                GameEventManager.Instance.TriggerSpeedChange(2f, duration); // 2x Cepat
                break;
            
            case EventType.SlowDown:
                GameEventManager.Instance.TriggerSpeedChange(0.5f, duration); // Setengah Kecepatan
                break;

            case EventType.Reverse:
                GameEventManager.Instance.TriggerReverseDirection(duration);
                break;

            case EventType.ScreenRotate:
                GameEventManager.Instance.TriggerScreenRotate(duration);
                break;
                
            case EventType.ForcedFlip:
                GameEventManager.Instance.TriggerForcedFlip();
                break;
        }
    }
}