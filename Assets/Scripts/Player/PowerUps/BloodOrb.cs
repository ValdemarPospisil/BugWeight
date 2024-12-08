using UnityEngine;
using UnityEngine.Animations;

public class BloodOrb : MonoBehaviour
{
    private float healAmount = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            
            playerManager.Heal(healAmount);
            Destroy(transform.parent.gameObject);
            
        }
    }

    public void SetHealAmount(float amount)
    {
        this.healAmount = amount;
    }

}