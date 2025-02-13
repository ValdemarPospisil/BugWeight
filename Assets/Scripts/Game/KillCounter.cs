using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    private int killCount = 0;
    private int totalKillCount = 0;
    private int tollKillCount = 0;
    private float tollCooldown = 0;
    [SerializeField] private TextMeshProUGUI killCounterText;
    [SerializeField] private TextMeshProUGUI tollKillCounterText;
    private bool isTollActive = false;
    private bool isDeathTollActive = false;
   // private int playerDeathCount = 0;

    void Start()
    {
        killCount = 0;
        isDeathTollActive = false;

    }
    public void ActivateDeathToll()
    {
        isDeathTollActive = true;
        isTollActive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDeathTollActive)
        {
            if (tollCooldown <= 0 && !isTollActive)
            {
                isTollActive = true;
                Debug.Log("Death Toll is now active");
            }
            else
            {
                Debug.Log("Death Toll is on cooldown" + tollCooldown);
                tollCooldown -= Time.deltaTime;
            }
        }
        
    }

    public void AddKill()
    {
        killCount++;
        totalKillCount++;
        killCounterText.text = "Kills: " + totalKillCount;
        if (isTollActive)
        {
            tollKillCount++;
            tollKillCounterText.text = "Toll Kills: " + tollKillCount;
        }
    }


    public void DeathToll(float cooldown)
    {
        tollCooldown = cooldown;
        isTollActive = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;

        foreach (GameObject enemy in enemies)
        {
            if (count >= tollKillCount)
            break;

            IDamageable enemyDamagable = enemy.GetComponent<IDamageable>();
            if (enemyDamagable != null)
            {
            enemyDamagable.TakeDamage(5000);
            count++;
            }
        }
        tollKillCount = 0;
        tollKillCounterText.text = "Toll Kills: " + tollKillCount;
    }
}
