using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
[RequireComponent(typeof(PlayerInputManager))]
public class GameManager : MonoBehaviour
{
public static GameManager Instance { get; private set; }
private PlayerInputManager playerInputManager;
public List<PlayerInput> players = new List<PlayerInput>();

private void Awake()
{
    if (Instance != null)
    {
        Destroy(gameObject);
        return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);

    playerInputManager = GetComponent<PlayerInputManager>();
    playerInputManager.onPlayerJoined += HandlePlayerJoined;

    // Player Prefab'ını Resources klasöründen kodla yükle ve ata.
    GameObject playerPrefab = Resources.Load<GameObject>("PlayerController");
    if (playerPrefab != null)
    {
        playerInputManager.playerPrefab = playerPrefab;
    }
    else
    {
        Debug.LogError("PlayerController prefab'ı, Resources klasöründe 'PlayerController' adıyla bulunamadı!");
    }
}

private void OnDestroy()
{
    if (playerInputManager != null)
    {
        playerInputManager.onPlayerJoined -= HandlePlayerJoined;
    }
}

public void HandlePlayerJoined(PlayerInput playerInput)
{
    Debug.Log($"Oyuncu {playerInput.playerIndex + 1} katıldı!");
    if (!players.Contains(playerInput))
    {
        players.Add(playerInput);
    }
    DontDestroyOnLoad(playerInput.gameObject);
}

public void EnableJoining()
{
    playerInputManager.EnableJoining();
    Debug.Log("Oyuncu katılımı AÇIK.");
}

public void DisableJoining()
{
    playerInputManager.DisableJoining();
    Debug.Log("Oyuncu katılımı KAPALI.");
}
// GameManager.cs'in en altına, diğer fonksiyonların dışına ekleyin.

public void SwitchAllPlayersToActionMap(string mapName)
{
    // Oyuncu listesindeki her bir oyuncu için...
    foreach (PlayerInput player in players)
    {
        // ...PlayerInput bileşenini bul ve Action Map'ini değiştir.
        player.SwitchCurrentActionMap(mapName);
        Debug.Log($"Oyuncu {player.playerIndex + 1} kontrol şeması '{mapName}' olarak değiştirildi.");
    }
}
}