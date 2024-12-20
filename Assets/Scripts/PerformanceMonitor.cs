using UnityEngine;
using TMPro;
public class PerformanceMonitor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI ramText;

    private float deltaTime = 0.0f;

    void Update()
    {
        // Calculate FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("FPS: {0:0.}", fps);

        // Calculate RAM usage
        long totalMemory = System.GC.GetTotalMemory(false);
        ramText.text = string.Format("RAM: {0} MB", totalMemory / (1024 * 1024));
    }
}