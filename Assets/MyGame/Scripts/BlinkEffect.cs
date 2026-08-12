using UnityEngine;
using TMPro;

public class BlinkEffect : MonoBehaviour
{
    [Header("Cài đặt hiệu ứng")]
    [Range(0.1f, 10f)] public float speed = 2f;
    [Range(0f, 1f)] public float minAlpha = 0.3f;
    [Range(0f, 1f)] public float maxAlpha = 1f;

    private TextMeshProUGUI targetText;

    void Start()
    {
        targetText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (targetText == null) return;
        
        float alpha = Mathf.PingPong(Time.time * speed, maxAlpha - minAlpha) + minAlpha;
        
        Color c = targetText.color;
        c.a = alpha;
        targetText.color = c;
    }
}