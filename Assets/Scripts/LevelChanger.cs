using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to switch scenes

public class LevelChanger : MonoBehaviour
{
    public string nextLevelName;  // Set the next level name in the Inspector

    // Triggered when John reaches the finish line
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if John is the object colliding with the finish line
        if (collision.GetComponent<JohnMovement>() != null)
        {
            // Load the next level
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
