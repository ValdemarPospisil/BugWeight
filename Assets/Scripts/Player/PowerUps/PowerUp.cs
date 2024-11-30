using UnityEngine;
public abstract class PowerUp : ScriptableObject
{
    public string skillName; // Name of the power-up
    [TextArea(3, 10)]
    public string skillDescription; // Description of the power-up
    public Sprite icon; // Icon for the UI

    protected PowerUpManager manager;

    public void Initialize(PowerUpManager powerUpManager)
    {
        manager = powerUpManager;
    }

    public abstract void Activate(GameObject player);
    public abstract void Deactivate(GameObject player);
}
