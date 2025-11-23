using UnityEngine;
using TMPro;
using System.Collections.Generic; // List kullanmak için gerekli

public class PointManager : MonoBehaviour
{
    [Header("Bölüm Puan Ayarları")]
    // Inspector'da buraya eleman ekle. 
    // Element 0: İlk giriş puanı (Örn: 5)
    // Element 1: İlk yarışı bitirince gelen puan (Örn: 3)
    // Element 2: İkinci yarışı bitirince gelen puan...
    public List<int> bolumPuanlari = new List<int>(); 

    [Header("Sistem Verileri")]
    public int toplamPuanHavuzu = 0; 
    public int kullanilanPuan = 0; 
    
    [Header("Bağlantılar")]
    // 0:Hız, 1:Fren, 2:Nitro, 3:Yol Tutuş, 4:Ağırlık
    public UpgradeManager[] gelistirmeler = new UpgradeManager[5]; 
    public TMP_Text kalanPuanText; 
    
    void Start()
    {
        // Start'ta çağırmıyoruz çünkü veriyi dışarıdan (SceneFlowManager vb.) yükleyeceğiz.
        // Ama test için açık kalabilir.
        // GuncellePuanUI();
    }
    
    public void YükseltmeVerileriniYukle(CarStatsData data)
    {
        // --- 1. YENİ PUAN SİSTEMİ (BÖLÜM HARÇLIĞI) ---
        
        int suankiBolum = PlayerSelectionData.currentMapIndex;

        // Listede bu bölüm için ayarlanmış bir puan var mı?
        if (suankiBolum < bolumPuanlari.Count)
        {
            toplamPuanHavuzu = bolumPuanlari[suankiBolum];
        }
        else
        {
            // Eğer liste bittiyse (Örn: 10. bölüme geldin ama listede 5 tane var)
            // Varsayılan olarak son elemanı veya sabit bir puanı verelim.
            toplamPuanHavuzu = 3; 
            Debug.LogWarning("Bu bölüm için puan ayarlanmamış, varsayılan 3 verildi.");
        }

        // Kullanılan puanı sıfırlıyoruz çünkü yeni bir harcama hakkı verdik.
        kullanilanPuan = 0; 


        // --- 2. ARABA VERİLERİNİ YÜKLEME (KALDIĞI YERDEN DEVAM) ---
        
        // Eğer ilk bölümdeysek (0), araba fabrika ayarlarında (ScriptableObject) gelir.
        // Değilse, önceki yarıştan kazandığımız güçlerle (PlayerSelectionData) gelir.
        bool isNewGame = (suankiBolum == 0);
        
        float currentHiz = isNewGame ? data.temelHızlanma : PlayerSelectionData.player1FinalHız;
        float currentFren = isNewGame ? data.temelFren : PlayerSelectionData.player1FinalFren;
        float currentNitro = isNewGame ? data.temelNitro : PlayerSelectionData.player1FinalNitro;
        float currentYol = isNewGame ? data.temelYoltutus : PlayerSelectionData.player1FinalYoltutus;
        float currentAgir = isNewGame ? data.temelAgırlık : PlayerSelectionData.player1FinalAgırlık;

        // Güvenlik: Veri hatası varsa fabrika ayarlarına dön
        if (!isNewGame && currentHiz < 1f) currentHiz = data.temelHızlanma;

        // Verileri Bar Yöneticilerine Gönder (CarStatsData'daki limitleri kullanarak)
        if(CheckIndex(0)) gelistirmeler[0].OzellikDegerleriniAyarla(currentHiz, CarStatsData.MinHiz, CarStatsData.MaxHiz);
        if(CheckIndex(1)) gelistirmeler[1].OzellikDegerleriniAyarla(currentFren, CarStatsData.MinFren, CarStatsData.MaxFren);
        if(CheckIndex(2)) gelistirmeler[2].OzellikDegerleriniAyarla(currentNitro, CarStatsData.MinNitro, CarStatsData.MaxNitro);
        if(CheckIndex(3)) gelistirmeler[3].OzellikDegerleriniAyarla(currentYol, CarStatsData.MinYol, CarStatsData.MaxYol);
        if(CheckIndex(4)) gelistirmeler[4].OzellikDegerleriniAyarla(currentAgir, CarStatsData.MinAgir, CarStatsData.MaxAgir);
        
        GuncellePuanUI();
    }
    
    // --- DİĞER FONKSİYONLAR (AYNEN KALIYOR) ---
    
    public void PuanDagitmayaCalis(int gelistirmeIndexi)
    {
        // Puan hesabında bir değişiklik yok, mantık aynı.
        int kalan = toplamPuanHavuzu - kullanilanPuan;
        
        if (kalan >= 1 && CheckIndex(gelistirmeIndexi))
        {
            int harcanan = gelistirmeler[gelistirmeIndexi].SeviyeArttirma();
            if (harcanan > 0)
            {
                kullanilanPuan += harcanan; 
                GuncellePuanUI();
            }
        }
    }
    
    public void PuanGeriAlmayaCalis(int gelistirmeIndexi)
    {
        // DİKKAT: Burada oyuncu önceki bölümde kazandığı özellikleri de geri alıp
        // bu bölümdeki puana katabilir. Eğer bunu istemiyorsan (Lock sistemi) ekstra kod gerekir.
        // Şimdilik serbest bırakıyoruz.
        if (CheckIndex(gelistirmeIndexi))
        {
            int iade = gelistirmeler[gelistirmeIndexi].SeviyeAzaltma(); 
            if (iade < 0) 
            {
                kullanilanPuan += iade;
                GuncellePuanUI();
            }
        }
    }
    
    public void KaydedilecekDegerleriAyarla(bool isP1)
    {
        // Son oluşan değerleri (Eski Güç + Yeni Eklenenler) kaydet
        if(CheckIndex(0)) SetVal(isP1, 0, ref PlayerSelectionData.player1FinalHız, ref PlayerSelectionData.player2FinalHız);
        if(CheckIndex(1)) SetVal(isP1, 1, ref PlayerSelectionData.player1FinalFren, ref PlayerSelectionData.player2FinalFren);
        if(CheckIndex(2)) SetVal(isP1, 2, ref PlayerSelectionData.player1FinalNitro, ref PlayerSelectionData.player2FinalNitro);
        if(CheckIndex(3)) SetVal(isP1, 3, ref PlayerSelectionData.player1FinalYoltutus, ref PlayerSelectionData.player2FinalYoltutus);
        if(CheckIndex(4)) SetVal(isP1, 4, ref PlayerSelectionData.player1FinalAgırlık, ref PlayerSelectionData.player2FinalAgırlık);
        
        // Not: Artık "Kalan Puanı" kaydetmiyoruz çünkü her tur yeni puan veriyoruz.
    }

    private void SetVal(bool isP1, int idx, ref float p1, ref float p2)
    {
        float val = gelistirmeler[idx].GuncelDegeriGetir();
        if (isP1) p1 = val; else p2 = val;
    }

    private void GuncellePuanUI()
    {
        int kalan = toplamPuanHavuzu - kullanilanPuan;
        if (kalanPuanText != null) kalanPuanText.text = "Kalan Puan: " + kalan;
    }
    
    private bool CheckIndex(int i)
    {
        return (gelistirmeler != null && i >= 0 && i < gelistirmeler.Length && gelistirmeler[i] != null);
    }
}