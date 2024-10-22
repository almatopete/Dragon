using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public int totalCoins;  // Set the total number of coins in the Inspector
    private int collectedCoins = 0;

    public string nextLevelName;  // Name of the next level to load

    public void AddCoin()
    {
        collectedCoins++;
        Debug.Log("Coins collected: " + collectedCoins);

    }
}
