using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public float currentXP { get; private set; }
    public int level { get; private set; } = 1;
    public float toNextLevel { get; private set; } = 100;

    public UnityEvent OnLevelUp = new UnityEvent();
    public static LevelManager Instance { get; private set; }
    private PlayerManager playerManager;

    public void Start()
    {
        currentXP = 0;
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        playerManager.UpdateUI();
    }
    public void AddXP(float amount)
    {
        currentXP += amount;
        CheckLevelUp();
        playerManager.UpdateUI();
    }

    private void CheckLevelUp()
    {
        if (currentXP >= toNextLevel)
        {
            currentXP -= toNextLevel;
            level++;
            toNextLevel += toNextLevel * 0.3f; // Increment difficulty to level up
            playerManager.UpdateUI();
            OnLevelUp.Invoke();
        }
    }
}
