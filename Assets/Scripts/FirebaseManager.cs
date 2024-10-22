using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public FirebaseFirestore firestore;

    private bool firebaseInitialized = false;

    public static event Action OnFirebaseInitialized;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Mantener el objeto entre escenas.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase is ready, initialize Firestore
                firestore = FirebaseFirestore.DefaultInstance;
                firebaseInitialized = true;

                Debug.Log("Firebase Firestore initialized.");

                // Trigger the event to notify that Firebase is ready
                OnFirebaseInitialized?.Invoke();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }

    // Método para guardar el progreso del jugador
    public void SavePlayerProgress(int coins, int level)
    {
        // Referencia a la colección y documento dentro de Firestore
        DocumentReference docRef = firestore.Collection("players").Document("John");

        // Datos a guardar
        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
            { "coins", coins },
            { "level", level },
            { "timestamp", FieldValue.ServerTimestamp }
        };

        // Guardar en Firestore
        docRef.SetAsync(playerData).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Progreso del jugador guardado en Firestore.");
            }
            else
            {
                Debug.LogError("Error guardando progreso: " + task.Exception);
            }
        });
    }

    // Método para cargar el progreso del jugador
   public void LoadPlayerProgress(System.Action<int, int> callback)
    {
        // Ensure Firebase is initialized before trying to access Firestore
        if (!firebaseInitialized)
        {
            Debug.LogError("Firebase is not initialized yet. Cannot load player progress.");
            return;
        }

        DocumentReference docRef = firestore.Collection("players").Document("John");

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int coins = snapshot.GetValue<int>("coins");
                    int level = snapshot.GetValue<int>("level");

                    // Pass the loaded data back using the callback
                    callback(coins, level);
                }
                else
                {
                    Debug.Log("Player data not found. Starting from default values.");
                    callback(0, 1);  // Default values for coins and level
                }
            }
            else
            {
                Debug.LogError("Error loading player progress: " + task.Exception);
            }
        });
    }

    // Add a public property to access the initialization status
    public bool IsFirebaseInitialized
    {
        get { return firebaseInitialized; }
    }

    public void ResetPlayerProgressInFirebase()
{
    // Reference to the collection and document in Firestore
    DocumentReference docRef = firestore.Collection("players").Document("John");

    // Data to reset
    Dictionary<string, object> resetData = new Dictionary<string, object>
    {
        { "coins", 0 },
        { "level", 1 },
        { "timestamp", FieldValue.ServerTimestamp }
    };

    // Update Firestore with reset data
    docRef.SetAsync(resetData).ContinueWithOnMainThread(task => {
        if (task.IsCompleted)
        {
            Debug.Log("Player progress reset in Firestore.");
        }
        else
        {
            Debug.LogError("Error resetting progress: " + task.Exception);
        }
    });
}

}
