// using UnityEngine;
// using UnityEngine.UI;
//
// public class UpgradeLobbyManager : MonoBehaviour
// {
//     [Header("Player 1 UI")]
//     public Button readyButtonP1;
//     public Button leftButtonP1;
//     public Button rightButtonP1;
//     public Image carImageP1;
//     public Sprite[] carSpritesP1;
//
//     [Header("Player 2 UI")]
//     public Button readyButtonP2;
//     public Button leftButtonP2;
//     public Button rightButtonP2;
//     public Image carImageP2;
//     public Sprite[] carSpritesP2;
//
//     private int carIndexP1 = 0;
//     private int carIndexP2 = 0;
//     private bool p1Ready = false;
//     private bool p2Ready = false;
//
//     private void Start()
//     {
//         // Başlangıçta arabaları ilk sprite'a ayarla
//         if (carSpritesP1.Length > 0) carImageP1.sprite = carSpritesP1[carIndexP1];
//         if (carSpritesP2.Length > 0) carImageP2.sprite = carSpritesP2[carIndexP2];
//
//         // Butonları aktif hale getir
//         SetPlayerControls(true, true);
//
//         // Listener’ları bağla
//         readyButtonP1.onClick.AddListener(() => OnReadyPressed(1));
//         readyButtonP2.onClick.AddListener(() => OnReadyPressed(2));
//         leftButtonP1.onClick.AddListener(() => ChangeCar(-1, 1));
//         rightButtonP1.onClick.AddListener(() => ChangeCar(1, 1));
//         leftButtonP2.onClick.AddListener(() => ChangeCar(-1, 2));
//         rightButtonP2.onClick.AddListener(() => ChangeCar(1, 2));
//     }
//
//     private void ChangeCar(int direction, int player)
//     {
//         if (player == 1 && !p1Ready)
//         {
//             carIndexP1 = (carIndexP1 + direction + carSpritesP1.Length) % carSpritesP1.Length;
//             carImageP1.sprite = carSpritesP1[carIndexP1];
//         }
//         else if (player == 2 && !p2Ready)
//         {
//             carIndexP2 = (carIndexP2 + direction + carSpritesP2.Length) % carSpritesP2.Length;
//             carImageP2.sprite = carSpritesP2[carIndexP2];
//         }
//     }
//
//     private void OnReadyPressed(int player)
//     {
//         if (player == 1)
//         {
//             p1Ready = true;
//             readyButtonP1.interactable = false;
//             leftButtonP1.interactable = false;
//             rightButtonP1.interactable = false;
//         }
//         else if (player == 2)
//         {
//             p2Ready = true;
//             readyButtonP2.interactable = false;
//             leftButtonP2.interactable = false;
//             rightButtonP2.interactable = false;
//         }
//
//         // Arabaları global olarak kaydet
//         PlayerSelectionData.player1CarIndex = carIndexP1;
//         PlayerSelectionData.player2CarIndex = carIndexP2;
//
//         // Eğer iki oyuncu da hazırsa yeni round'u başlat
//         if (p1Ready && p2Ready)
//         {
//             StartNextRound();
//         }
//     }
//
//     private void StartNextRound()
//     {
//         Debug.Log("🎮 İki oyuncu da hazır! Yeni harita yükleniyor...");
//
//         // Tüm butonları pasifleştir (ek güvenlik)
//         SetPlayerControls(false, false);
//
//         // SceneFlowManager üzerinden sahne yüklemesi yap
//         if (SceneFlowManager.Instance != null)
//         {
//             SceneFlowManager.Instance.LoadNextLevelOrEndGame();
//         }
//         else if (Fade_Manager.Instance != null)
//         {
//             Fade_Manager.Instance.StartFadeOutAndLoadScene("SampleScene");
//         }
//         else
//         {
//             UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
//         }
//
//         // Round bittikten sonra geri dönüldüğünde yeniden aktif edilsin
//         ResetLobby();
//     }
//
//     private void SetPlayerControls(bool p1Active, bool p2Active)
//     {
//         readyButtonP1.interactable = p1Active;
//         leftButtonP1.interactable = p1Active;
//         rightButtonP1.interactable = p1Active;
//
//         readyButtonP2.interactable = p2Active;
//         leftButtonP2.interactable = p2Active;
//         rightButtonP2.interactable = p2Active;
//     }
//
//     private void ResetLobby()
//     {
//         // Round bitip UpgradeLobbyScene'e dönülünce tekrar aktif olmalı
//         p1Ready = false;
//         p2Ready = false;
//
//         SetPlayerControls(true, true);
//     }
// }

using UnityEngine;
using UnityEngine.UI;

public class UpgradeLobbyManager : MonoBehaviour
{
    [Header("Player 1 UI")] public Button readyButtonP1;
    public Button leftButtonP1;
    public Button rightButtonP1;
    public Image carImageP1;
    public Sprite[] carSpritesP1;

    [Header("Player 2 UI")] public Button readyButtonP2;
    public Button leftButtonP2;
    public Button rightButtonP2;
    public Image carImageP2;
    public Sprite[] carSpritesP2;

    private int carIndexP1 = 0;
    private int carIndexP2 = 0;
    private bool isReadyP1 = false;
    private bool isReadyP2 = false;

    void Start()
    {
        // UI event bağlama
        if (leftButtonP1) leftButtonP1.onClick.AddListener(() => ChangeCar(-1, 1));
        if (rightButtonP1) rightButtonP1.onClick.AddListener(() => ChangeCar(1, 1));
        if (readyButtonP1) readyButtonP1.onClick.AddListener(() => PlayerReady(1));

        if (leftButtonP2) leftButtonP2.onClick.AddListener(() => ChangeCar(-1, 2));
        if (rightButtonP2) rightButtonP2.onClick.AddListener(() => ChangeCar(1, 2));
        if (readyButtonP2) readyButtonP2.onClick.AddListener(() => PlayerReady(2));

        // Başlangıç doğrulama ve ilk görsel
        ValidateAndInit(1);
        ValidateAndInit(2);
    }

    // Dizi ve Image atanmış mı? Uzunluk >=1 mi? İlk resmi göster
    private void ValidateAndInit(int player)
    {
        if (player == 1)
        {
            if (carSpritesP1 == null || carSpritesP1.Length == 0 || carImageP1 == null)
            {
                SetP1Interactable(false);
                return;
            }

            carIndexP1 = Wrap(carIndexP1, carSpritesP1.Length);
            UpdateCarImage(1);
            SetP1Interactable(true);
        }
        else
        {
            if (carSpritesP2 == null || carSpritesP2.Length == 0 || carImageP2 == null)
            {
                SetP2Interactable(false);
                return;
            }

            carIndexP2 = Wrap(carIndexP2, carSpritesP2.Length);
            UpdateCarImage(2);
            SetP2Interactable(true);
        }
    }

    private void SetP1Interactable(bool on)
    {
        if (leftButtonP1) leftButtonP1.interactable = on;
        if (rightButtonP1) rightButtonP1.interactable = on;
        if (readyButtonP1) readyButtonP1.interactable = on;
    }

    private void SetP2Interactable(bool on)
    {
        if (leftButtonP2) leftButtonP2.interactable = on;
        if (rightButtonP2) rightButtonP2.interactable = on;
        if (readyButtonP2) readyButtonP2.interactable = on;
    }

    private int Wrap(int idx, int len)
    {
        if (len <= 0) return 0;
        idx %= len;
        if (idx < 0) idx += len;
        return idx;
    }

    private void ChangeCar(int direction, int player)
    {
        if (player == 1)
        {
            if (isReadyP1 || carSpritesP1 == null || carSpritesP1.Length == 0) return;
            carIndexP1 = Wrap(carIndexP1 + direction, carSpritesP1.Length);
            UpdateCarImage(1);
        }
        else
        {
            if (isReadyP2 || carSpritesP2 == null || carSpritesP2.Length == 0) return;
            carIndexP2 = Wrap(carIndexP2 + direction, carSpritesP2.Length);
            UpdateCarImage(2);
        }
    }

    private void UpdateCarImage(int player)
    {
        if (player == 1)
        {
            if (carSpritesP1 == null || carSpritesP1.Length == 0 || carImageP1 == null) return;
            carIndexP1 = Wrap(carIndexP1, carSpritesP1.Length);
            carImageP1.sprite = carSpritesP1[carIndexP1];
        }
        else
        {
            if (carSpritesP2 == null || carSpritesP2.Length == 0 || carImageP2 == null) return;
            carIndexP2 = Wrap(carIndexP2, carSpritesP2.Length);
            carImageP2.sprite = carSpritesP2[carIndexP2];
        }
    }

    private void PlayerReady(int player)
    {
        if (player == 1)
        {
            isReadyP1 = true;
            if (readyButtonP1) readyButtonP1.interactable = false;
            if (leftButtonP1) leftButtonP1.interactable = false;
            if (rightButtonP1) rightButtonP1.interactable = false;
        }
        else
        {
            isReadyP2 = true;
            if (readyButtonP2) readyButtonP2.interactable = false;
            if (leftButtonP2) leftButtonP2.interactable = false;
            if (rightButtonP2) rightButtonP2.interactable = false;
        }

        // Seçimi kaydet
        PlayerSelectionData.player1CarIndex = carIndexP1;
        PlayerSelectionData.player2CarIndex = carIndexP2;

        if (isReadyP1 && isReadyP2)
        {
            PlayerSelectionData.player1CarIndex = carIndexP1;
            PlayerSelectionData.player2CarIndex = carIndexP2;

            if (SceneFlowManager.Instance != null)
            {
                // Fade ile düzgün sahne geçişi
                if (SceneFlowManager.Instance != null)
                {
                    SceneFlowManager.Instance.LoadNextLevelOrEndGame();
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
                }
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
            }
        }
    }
}