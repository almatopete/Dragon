using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    public GameObject gameOverUI; // Assign the Game Over UI in the Inspector

    private void Awake()
    {
        // Singleton pattern to ensure one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Ensure the Game Over UI is hidden when the game starts
        gameOverUI.SetActive(false);
    }

    public void TriggerGameOver()
    {
        // Show the Game Over UI
        gameOverUI.SetActive(true);

        // Optionally, you can stop time to pause the game
        Time.timeScale = 0f; // Pauses the game
    }

    public void RestartGame()
    {
        // Reset time and reload the current scene
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        // Load the Main Menu Scene (assumes scene 0 is the main menu)
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(0); // Load the main menu scene
    }
}
