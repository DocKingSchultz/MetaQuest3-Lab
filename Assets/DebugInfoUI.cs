using TMPro;
using UnityEngine;

public class DebugInfoUI : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    void Update()
    {
        // Example debug info
        debugText.text = $"FPS: {1.0f / Time.deltaTime:F1}\nPosition: {Camera.main.transform.position}";
    }
}