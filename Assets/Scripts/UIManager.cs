using UnityEngine;
using TMPro;  // If using TextMeshPro

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;  // Drag and drop the CoinsText in the Inspector
    public TextMeshProUGUI levelText;  // Drag and drop the LevelText in the Inspector

    // Reference to the JohnMovement script
    private JohnMovement john;

    private void Start()
    {
        // Find the JohnMovement script in the scene (assuming John has the "Player" tag)
        john = GameObject.FindWithTag("Player").GetComponent<JohnMovement>();

        // Initialize the UI with the initial values
        UpdateCoins(john.GetCoins());
        UpdateLevel(john.GetLevel());
    }

    private void Update()
    {
        // Update the UI elements with the current values
        UpdateCoins(john.GetCoins());
        UpdateLevel(john.GetLevel());
    }

    public void UpdateCoins(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + coins.ToString();
        }
    }

    public void UpdateLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level.ToString();
        }
    }
}
