using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Ethereal Form")]
public class EtherealFormPowerUp : PowerUp
{
    private float etherealSpeed;
    private float etherealDuration;
    public override void Activate(GameObject player)
    {
        UpdateProperties();
        player.GetComponent<PlayerController>().EtherealForm(etherealSpeed, etherealDuration);
    }
    

    public override void Deactivate()
    {
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tierVariables.Count)
        {
            var tier = tierVariables[currentTier - 1];
            etherealSpeed = tier.damage;
            etherealDuration = tier.variable;
        }

    }
}