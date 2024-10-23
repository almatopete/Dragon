using UnityEngine;

public class ExitGame : MonoBehaviour
{

       // Method to exit the game
    public void ExitApplication()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();

        // If running in the editor, stop the play mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Check for ESC key press in each frame
    private void Update()
    {
        // Check if the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitApplication();
        }
    }
}
