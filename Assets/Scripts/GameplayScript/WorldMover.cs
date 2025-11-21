using UnityEngine;
using TMPro;


public class WorldMover : MonoBehaviour
{
    [Header("Setting Dasar")]
    public float moveSpeed = 5f; 


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;

    // --- TAMBAHAN BARU (Sembunyi di Inspector tapi bisa diakses script lain) ---
    [HideInInspector] public float speedMultiplier = 0.1f; 
    [HideInInspector] public float direction = 1f;



    private float score;

    void Update()
    {
        // Rumus: Speed Dasar x Pengali (Cepat/Lambat) x Arah (Maju/Mundur)
        float finalSpeed = moveSpeed * speedMultiplier * direction;
        
        transform.Translate(Vector2.left * finalSpeed * Time.deltaTime);
        score += finalSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString();
        UpdateHiscore();
    }
    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if(score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
            PlayerPrefs.Save();

        }
        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString();
    }
}