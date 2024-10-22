using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public string nextLevelName;  // Set the next level name in the Inspector

         // Public variables to define starting positions for different scenes

    // Triggered when John reaches the finish line
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Check if John is the object colliding with the finish line
        JohnMovement john = collision.GetComponent<JohnMovement>();

        // Check if John is the object colliding with the finish line
        if (john != null)
        {

            john.CompleteLevel();
            // Save current progress before loading the next scene
            FirebaseManager.Instance.SavePlayerProgress(john.GetCoins(), john.GetLevel());

            // Load the next level
            SceneManager.LoadScene(nextLevelName);
            john.movePlayer();
        }
    }
}
