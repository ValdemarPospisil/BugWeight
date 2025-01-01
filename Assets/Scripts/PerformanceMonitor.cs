using UnityEngine;
using TMPro;
public class PerformanceMonitor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI ramText;
    private float updateInterval = 1f; // Update every second

    private float deltaTime = 0.0f;

    private void Start()
    {
        if (ramText == null)
        {
            enabled = false;
            return;
        }
        InvokeRepeating(nameof(UpdateMemoryUsage), 0f, updateInterval);
    }

    
    void Update()
    {
        // Calculate FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("FPS: {0:0.}", fps);

        // Calculate RAM usage
        //long totalMemory = System.GC.GetTotalMemory(true);
       // ramText.text = string.Format("RAM: {0} MB", totalMemory / (1024 * 1024));
    }

    private void UpdateMemoryUsage()
    {
        long memoryUsage = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        ramText.text = $"RAM: {FormatMemory(memoryUsage)}";
    }


    private string FormatMemory(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        int order = 0;
        while (bytes >= 1024 && order < sizes.Length - 1)
        {
            order++;
            bytes /= 1024;
        }
        return $"{bytes:0.##} {sizes[order]}";
    }
}