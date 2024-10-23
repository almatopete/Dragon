using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameManager : MonoBehaviour
{
    public static ExitGameManager Instance;
    public GameObject EndUI; // Assign the Game Over UI in the Inspector

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
        EndUI.SetActive(false);
    }

    // This method is called when another object enters the trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
         // Check if John is the object colliding with the finish line
        JohnMovement john = collision.GetComponent<JohnMovement>();

        // Check if John is the object colliding with the finish line
        if (john != null)
        {
            Debug.Log("Player entered the exit zone. Exiting the game...");
                EndUI.SetActive(true);

        // Optionally, you can stop time to pause the game
        //Time.timeScale = 0f; // Pauses the game
          //  Application.Quit();

            // If running in the editor, stop the play mode
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    public void RestartGame()
    {
        // Reset time and reload the current scene
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene("SampleScene");
    }
}
