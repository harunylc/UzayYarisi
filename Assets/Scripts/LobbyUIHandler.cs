using UnityEngine;
using UnityEngine.InputSystem;
public class LobbyUIHandler : MonoBehaviour
{
    [Tooltip("PlayerInputActions asset'inden Cancel eylemini buraya sürükleyin.")]
    public InputActionReference cancelAction;
    private void OnEnable()
    {
        if (cancelAction != null) cancelAction.action.Enable();
    }

    private void OnDisable()
    {
        if (cancelAction != null) cancelAction.action.Disable();
    }

    void Update()
    {
        // Eğer B tuşuna basılırsa...
        if (cancelAction != null && cancelAction.action.WasPressedThisFrame())
        {
            // ...Fade animasyonu ile MainMenuScene'e geri dön.
            Debug.Log("Ana Menüye dönülüyor...");
            Fade_Manager.Instance.StartFadeOutAndLoadScene("MainMenuScene");
        }
    }
}
