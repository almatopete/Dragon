using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name); // Log the name of the object that collides

        JohnMovement john = collision.GetComponent<JohnMovement>();

        if (john != null)
        {
            john.AddCoin();  // Agregar una moneda al jugador
            Destroy(gameObject);  // Destruir la moneda
        }
    }
}
