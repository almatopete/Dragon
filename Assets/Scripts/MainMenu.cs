using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para cargar escenas

public class MainMenu : MonoBehaviour
{
    // Método para el botón de Jugar
    public void Jugar()
    {
        // Cargar la primera escena del juego (por nombre o índice)
        SceneManager.LoadScene("SampleScene");
    }

    // Método para el botón de Opciones (opcional)
    public void Opciones()
    {
        Debug.Log("Abrir menú de opciones"); // Aquí podrías abrir un menú de opciones
    }

    // Método para el botón de Salir
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Si estás en el editor de Unity, parar la ejecución
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
