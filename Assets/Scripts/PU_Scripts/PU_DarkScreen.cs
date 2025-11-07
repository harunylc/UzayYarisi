using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PU_DarkScreen : MonoBehaviour
{
    public bool isPowerUpObject;
    public Image darkPanelImage;
    private Coroutine activeCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPowerUpObject) return;

        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
            if (carHandler != null)
            {
                // Artık güçlendirmenin adını bir string olarak veriyoruz.
                carHandler.GivePowerUp("DarkScreen");
                Destroy(gameObject);
            }
        }
    }

    public void Activate(float duration)
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
        activeCoroutine = StartCoroutine(DarkenScreenRoutine(duration));
    }

    private IEnumerator DarkenScreenRoutine(float duration)
    {
        if (darkPanelImage == null) yield break;

        Color color = darkPanelImage.color;
        color.a = 204f / 255f;
        darkPanelImage.color = color;
        
        yield return new WaitForSeconds(duration);

        color.a = 0f;
        darkPanelImage.color = color;
        
        activeCoroutine = null;
    }
}