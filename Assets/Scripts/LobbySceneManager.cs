using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.InputSystem.UI;
public class LobbySceneManager : MonoBehaviour
{
[Header("UI Panelleri")]
public GameObject player1_UI_Canvas;
public GameObject player2_UI_Canvas;

[Header("İlk Seçilecek Butonlar")]
public GameObject player1_firstButton;
public GameObject player2_firstButton;

[Header("Geri Dönüş Input'u")]
public InputActionReference cancelAction;

// Sarı uyarı veren değişkenler (şimdilik sorun değil)
private bool player1Ready = false;
private bool player2Ready = false;

void OnEnable()
{
    // GameManager'ın var olup olmadığını ve PIM'e sahip olup olmadığını kontrol et
    if (GameManager.Instance != null && GameManager.Instance.GetComponent<PlayerInputManager>() != null)
    {
        // Abone ol
        GameManager.Instance.GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
    }
    if (cancelAction != null) cancelAction.action.Enable();
}

void OnDisable()
{
    if (GameManager.Instance != null && GameManager.Instance.GetComponent<PlayerInputManager>() != null)
    {
        // Abonelikten çık
        GameManager.Instance.GetComponent<PlayerInputManager>().onPlayerJoined -= OnPlayerJoined;
    }
    if (cancelAction != null) cancelAction.action.Disable();
}

void Start()
{
    SetupExistingPlayers();
    
    if (GameManager.Instance != null)
    {
        GameManager.Instance.EnableJoining();
    }
    else
    {
        Debug.LogError("Lobby Sahnesinde GameManager bulunamadı!");
    }
    // Oyuncuların kontrol şemasının "UI" olduğundan emin ol
    if (GameManager.Instance != null)
    {
        GameManager.Instance.SwitchAllPlayersToActionMap("UI");
    }
}

void Update()
{
    // Savunmacı Update döngüsü
    if (GameManager.Instance == null || GameManager.Instance.players.Count == 0) return;
    PlayerInput player1 = GameManager.Instance.players[0];
    if (player1 == null || player1.devices.Count == 0) return;
    if (cancelAction == null || !cancelAction.action.triggered) return;

    if (cancelAction.action.activeControl.device == player1.devices[0])
    {
        if (Fade_Manager.Instance != null)
        {
            Fade_Manager.Instance.StartFadeOutAndLoadScene("MainMenuScene");
        }
        else
        {
            Debug.LogError("Fade_Manager bulunamadı!");
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}

// --- KIRMIZI HATAYI ÇÖZEN, EKSİK OLAN FONKSİYON ---
public void OnPlayerJoined(PlayerInput playerInput)
{
    // Bu sahneye sadece 2. oyuncu katılabilir
    if (playerInput.playerIndex == 1)
    {
        SetupPlayerUI(playerInput, player2_UI_Canvas, player2_firstButton);
        // İkinci oyuncu katıldığına göre yeni katılımı kapat
        GameManager.Instance.DisableJoining();
    }
}
// --------------------------------------------------------

private void SetupExistingPlayers()
{
    if (GameManager.Instance == null) return;

    foreach (var player in GameManager.Instance.players)
    {
        if (player.playerIndex == 0)
        {
            SetupPlayerUI(player, player1_UI_Canvas, player1_firstButton);
        }
        else if (player.playerIndex == 1)
        {
            SetupPlayerUI(player, player2_UI_Canvas, player2_firstButton);
        }
    }
}

private void SetupPlayerUI(PlayerInput player, GameObject uiCanvas, GameObject firstButton)
{
    // UI ve EventSystem kurulumu
    var eventSystemObj = new GameObject($"Player_{player.playerIndex + 1}_EventSystem");
    var eventSystem = eventSystemObj.AddComponent<EventSystem>();
    var inputModule = eventSystemObj.AddComponent<InputSystemUIInputModule>();
    
    player.uiInputModule = inputModule; // Yeni ve doğru yöntem
    
    if (uiCanvas != null) uiCanvas.SetActive(true);
    if (eventSystem != null && firstButton != null)
    {
        eventSystem.SetSelectedGameObject(firstButton);
    }
}
}