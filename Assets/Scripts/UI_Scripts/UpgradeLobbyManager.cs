using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullanıldığı için gerekli

public class UpgradeLobbyManager : MonoBehaviour
{
    // 1. MANAGER REFERANSLARI (Sahnedeki PointManager'lar buraya atanacak)
    [Header("Manager References")]
    public PointManager pointManagerP1;
    public PointManager pointManagerP2;

    // 2. OYUNCU 1 UI VE VERİ
    [Header("Player 1 UI")] 
    public Button readyButtonP1;
    public Button leftButtonP1;
    public Button rightButtonP1;
    public Image carImageP1;
    public Sprite[] carSpritesP1;
    [Header("Player 1 Car Prefabs (For Data Link)")]
    // CarSpritesP1 ile ayni sirada olmali!
    public GameObject[] carPrefabsP1; 

    // 3. OYUNCU 2 UI VE VERİ
    [Header("Player 2 UI")] 
    public Button readyButtonP2;
    public Button leftButtonP2;
    public Button rightButtonP2;
    public Image carImageP2;
    public Sprite[] carSpritesP2;
    [Header("Player 2 Car Prefabs (For Data Link)")]
    // CarSpritesP2 ile ayni sirada olmali!
    public GameObject[] carPrefabsP2; 

    private int carIndexP1 = 0;
    private int carIndexP2 = 0;
    private bool isReadyP1 = false;
    private bool isReadyP2 = false;
    
    // 4. HARİTA BİLGİSİ UI DEĞİŞKENLERİ
    [Header("Harita Bilgisi UI")] 
    public Image haritaImage;
    public TMP_Text haritaText;
    
    void Start()
    {
        // Button Listener atamaları
        if (leftButtonP1) leftButtonP1.onClick.AddListener(() => ChangeCar(-1, 1));
        if (rightButtonP1) rightButtonP1.onClick.AddListener(() => ChangeCar(1, 1));
        if (readyButtonP1) readyButtonP1.onClick.AddListener(() => PlayerReady(1));

        if (leftButtonP2) leftButtonP2.onClick.AddListener(() => ChangeCar(-1, 2));
        if (rightButtonP2) rightButtonP2.onClick.AddListener(() => ChangeCar(1, 2));
        if (readyButtonP2) readyButtonP2.onClick.AddListener(() => PlayerReady(2));

        ValidateAndInit(1);
        ValidateAndInit(2);

        // =========================================================================
        // YENİ BÖLÜM: BİR SONRAKİ HARİTANIN BİLGİSİNİ ÇEKME
        // =========================================================================
        
        // Singleton referansını kullanarak Manager'a erişim
        if (SceneFlowManager.Instance != null)
        {
            // Manager'daki GetNextLevelData metodu ile bir sonraki haritanın verisini çekiyoruz
            SceneData nextLevelData = SceneFlowManager.Instance.GetNextLevelData();

            if (nextLevelData != null)
            {
                // UI Elementlerini Gelen Veriye Göre Güncelle
                if (haritaImage != null)
                {
                    haritaImage.sprite = nextLevelData.SceneImage;
                }
                if (haritaText != null)
                {
                    haritaText.text = nextLevelData.SceneTitleText;
                }
                
                Debug.Log($"Gelecek Harita Bilgisi Yüklendi: {nextLevelData.SceneName} - {nextLevelData.SceneTitleText}");
            }
            else
            {
                // Eğer level kalmadıysa veya PrepareForUpgradeScene çağrılmamışsa
                Debug.LogError("Gelecek level için veri bulunamadı! SceneFlowManager'daki 'NextLevelSceneName' boş veya SceneFlowManager.AllSceneDataList'te karşılığı yok.");
            }
        }
        else
        {
            Debug.LogError("SceneFlowManager örneği bulunamadı!");
        }
        // =========================================================================
    }

    private void ChangeCar(int direction, int player)
    {
        if (player == 1)
        {
            if (isReadyP1 || carSpritesP1 == null || carSpritesP1.Length == 0) return;
            carIndexP1 = Wrap(carIndexP1 + direction, carSpritesP1.Length);
            UpdateCarImage(1);
            LoadCarStatsToPointManager(1, carIndexP1); // Veri akışını tetikle
        }
        else
        {
            if (isReadyP2 || carSpritesP2 == null || carSpritesP2.Length == 0) return;
            carIndexP2 = Wrap(carIndexP2 + direction, carSpritesP2.Length);
            UpdateCarImage(2);
            LoadCarStatsToPointManager(2, carIndexP2); // Veri akışını tetikle
        }
    }
    
    // YENİ METOT: Seçilen arabanın SO verisini PointManager'a yükler.
    private void LoadCarStatsToPointManager(int player, int carIndex)
    {
        GameObject[] carPrefabs = (player == 1) ? carPrefabsP1 : carPrefabsP2;
        PointManager pointManager = (player == 1) ? pointManagerP1 : pointManagerP2;
        
        // Dizi sınır kontrolü (Sprite ve Prefab listeleri aynı uzunlukta olmalı!)
        if (carIndex >= carPrefabs.Length) return;

        GameObject selectedPrefab = carPrefabs[carIndex];
        
        // Prefab'dan CarDataLink bileşenini al
        CarDataLink dataLink = selectedPrefab.GetComponent<CarDataLink>();

        if (dataLink != null && pointManager != null)
        {
            // PointManager'a veriyi yükle (PointManager'ın YükseltmeVerileriniYukle metodu var sayılır)
            pointManager.YükseltmeVerileriniYukle(dataLink.statsData);
        }
    }

    private void PlayerReady(int player)
    {
        if (player == 1)
        {
            isReadyP1 = true;
            SetP1Interactable(false);
        }
        else
        {
            isReadyP2 = true;
            SetP2Interactable(false);
        }

        // 4. Nihai Değerleri Kaydetme
        PlayerSelectionData.player1CarIndex = carIndexP1;
        PlayerSelectionData.player2CarIndex = carIndexP2;
        
        if (pointManagerP1 != null)
        {
            // pointManagerP1'e P1 verilerini kaydetmesini söyle
            pointManagerP1.KaydedilecekDegerleriAyarla(player == 1); 
        }
        if (pointManagerP2 != null)
        {
            // pointManagerP2'ye P2 verilerini kaydetmesini söyle
            pointManagerP2.KaydedilecekDegerleriAyarla(player == 2); 
        }
        
        if (isReadyP1 && isReadyP2)
        {
            // Hazır olunca bir sonraki leveli yükle
            SceneFlowManager.Instance.LoadNextLevelOrEndGame();
        }
    }
    
    // --- YARDIMCI METOTLAR ---
    private void ValidateAndInit(int player)
    {
        // Başlangıçta araba verilerini yüklemek için ChangeCar içindeki mantığı çağırır.
        if (player == 1)
        {
            if (carSpritesP1 == null || carSpritesP1.Length == 0 || carImageP1 == null) { SetP1Interactable(false); return; }
            carIndexP1 = Wrap(carIndexP1, carSpritesP1.Length);
            UpdateCarImage(1);
            SetP1Interactable(true);
            LoadCarStatsToPointManager(1, carIndexP1); // Başlangıç verisini yükle
        }
        else
        {
            if (carSpritesP2 == null || carSpritesP2.Length == 0 || carImageP2 == null) { SetP2Interactable(false); return; }
            carIndexP2 = Wrap(carIndexP2, carSpritesP2.Length);
            UpdateCarImage(2);
            SetP2Interactable(true);
            LoadCarStatsToPointManager(2, carIndexP2); // Başlangıç verisini yükle
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

    private int Wrap(int idx, int len)
    {
        if (len <= 0) return 0;
        idx %= len;
        if (idx < 0) idx += len;
        return idx;
    }
}