using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
public class LobbyInputLinker : MonoBehaviour
{
// Bu script'i Player1_Handler ve Player2_Handler objelerine ekleyeceğiz.
// Inspector'dan her oyuncunun kendi EventSystem'ini sürükleyeceğiz.
    public InputSystemUIInputModule targetUIInputModule;

    private void Awake()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();

        // Oyuncunun PlayerInput bileşenine, bu UI modülünü kullanmasını söylüyoruz.
        // Bu, "Player Index" ayarlamanın modern ve doğru yoludur.
        if (playerInput != null && targetUIInputModule != null)
        {
            playerInput.uiInputModule = targetUIInputModule;
        }
    }
}
