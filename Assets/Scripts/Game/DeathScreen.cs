using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    void OnEnable()
    {
        restartButton.Select();
    }

    void Update()
    {
        
    }

    public void RestartGame()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StartNewGame();
    }
}
