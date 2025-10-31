using UnityEngine;

public class TireGrounded : MonoBehaviour
{
   public bool tireGrounded =false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            tireGrounded = true;
            Debug.Log("Grounded");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            tireGrounded = false;
            Debug.Log("NOTGrounded");
            
        }
    }
}
