using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JohnMovement : MonoBehaviour
{

    public GameObject BulletPrefab;

    public float Speed;
    public float JumpForce;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;

    private bool Grounded; 

    private float Horizontal;

    private float LastShoot; 

    private int Health = 5;

    private int Coins = 0;  // Monedas obtenidas
    private int Level = 1;  // Nivel actual

    // New variable for start position
    private Vector3 startPosition;

    public Vector3 startScene1 = new Vector3(-0.65f, 0.16f, 0f);
    public Vector3 startScene2 = new Vector3(-0.48f, 0.72f, 0f);

    private UIManager uiManager;


    private void Awake()
    {
        // Ensure John is not destroyed when transitioning scenes
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent <Animator>();

           // Subscribe to the Firebase initialization event
        FirebaseManager.OnFirebaseInitialized += OnFirebaseInitialized;

        // Save the initial starting position of John
        startPosition = transform.position;

        // Load progress immediately if Firebase is already initialized
        if (FirebaseManager.Instance != null && FirebaseManager.Instance.IsFirebaseInitialized)
        {
            OnFirebaseInitialized();
        }

        // Reference to UIManager in the scene
    uiManager = FindObjectOfType<UIManager>();

    // Update the UI with initial values
    uiManager.UpdateCoins(Coins);
    uiManager.UpdateLevel(Level);
        
    }

        // This method will be called when Firebase is ready
    private void OnFirebaseInitialized()
    {
        // Load the player progress after Firebase is initialized
        FirebaseManager.Instance.LoadPlayerProgress((loadedCoins, loadedLevel) => {
            Coins = loadedCoins;
            Level = loadedLevel;
            Debug.Log($"Progreso cargado: {Coins} monedas, nivel {Level}");
        });
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        FirebaseManager.OnFirebaseInitialized -= OnFirebaseInitialized;
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");


        if (Horizontal < 0.0f) transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

        Animator.SetBool("running",Horizontal !=0.0f);

        Debug.DrawRay(transform.position, Vector3.down *0.1f, Color.red);

        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f)) {
            Grounded = true;

        } else Grounded = false; 

        if (Input.GetKeyDown(KeyCode.W) && Grounded){
            
            Jump();
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f){
            Shoot();
            LastShoot = Time.time; 
        }

        // Check if John has fallen below a certain point (e.g., Y = -10)
        if (transform.position.y < -3f)
        {
            GameOver();  // Trigger game over when falling out of the map
            ResetPlayer();
        }
    }

     private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private void Shoot(){

        Vector3 direction; 
        if (transform.localScale.x == 1.0f) direction = Vector3.right; 
        else direction = Vector3.left; 


       GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
       bullet.GetComponent<BulletScript>().SetDirection(direction);
        
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2 (Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    public void Hit ()
    {
        Health = Health -1;
        if (Health == 0 || Health <= 0) {
            ResetPlayer();
            GameOver();
            
        } 
    }

    // Método para agregar monedas
    public void AddCoin()
    {
        Coins++;
        Debug.Log($"Monedas: {Coins}");

        uiManager.UpdateCoins(Coins);
        // Guardar progreso después de recoger monedas
        FirebaseManager.Instance.SavePlayerProgress(Coins, Level);
    }

    // Método para completar un nivel
    public void CompleteLevel()
    {
        Level++;
        // Update the UI when a level is completed
        uiManager.UpdateLevel(Level);
        Debug.Log($"Nivel completado: {Level}");
    }

    private void GameOver()
    {
        // Call Game Over manager to trigger the Game Over screen
        GameOverManager.Instance.TriggerGameOver();
    }

    // Reset player to the start position and restore health
    private void ResetPlayer()
    {

          if (SceneManager.GetActiveScene().name == "SampleScene")
        { Destroy(gameObject); 
        }
        // Set position to the initial start position
        transform.position = startPosition;

        // Optionally reset health to max or some default value
        Health = 5;
        // Optionally, if there is a freeze on movement or animations, reset those as well
        Horizontal = 0f;

        Coins = 0;
        Level =1;

        Grounded=false;

        Debug.Log("Player reset to start position.");

        // Reset velocity to ensure smooth repositioning
        Rigidbody2D.velocity = Vector2.zero;
        ResetPlayerProgress();
        
    }

    public void movePlayer() {
                   // Set John's starting position based on the current scene
        switch (SceneManager.GetActiveScene().name)
        {
            case "SampleScene":  // Replace with your actual scene names
                startPosition = startScene1;
                break;
            case "Scene2":  // Replace with your actual scene names
                startPosition = startScene2;
                break;
            default:
                startPosition = transform.position;  // Use the current position if the scene is not recognized
                break;
                
        }
        transform.position = startPosition;

    }

    public void ResetPlayerProgress()
{
    // Reset coins and level locally
    Coins = 0;
    Level = 1;

    // Save the reset state to Firebase
    FirebaseManager.Instance.ResetPlayerProgressInFirebase();

    Debug.Log("Player progress has been reset.");
}


    public int GetCoins()
    {
        return Coins;
    }

    public int GetLevel()
    {
        return Level;
    }

    private void OnApplicationQuit()
{
    ResetPlayerProgress();
}

}
