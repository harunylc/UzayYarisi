// PointManager.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    // PUAN VERİLERİ
    public int toplamPuanHavuzu = 50; 
    public int kullanilanPuan = 0; 
    public const int BolmePuanDegerı = 1;
    
    // YÜKSELTME KATEGORİLERİ (Inspector'da atanmalı: 0=Hız, 1=Fren, vb.)
    public UpgradeManager[] gelistirmeler = new UpgradeManager[5]; 
    public TMP_Text kalanPuanText; 
    
    void Start()
    {
        GuncellePuanUI();
    }
    
    // 1. METOT: Lobi Manager'dan gelen SO verisini UpgradeManager'lara dağıt.
    public void YükseltmeVerileriniYukle(CarStatsData data)
    {
    //     Header("Temel Özellikler")]
    // public float temelHızlanma;
    // public float temelFren;
    // public float temelNitro;
    // public float temelYoltutus;
    // public float temelAgırlık;
    //
    // [Header("Yükseltme Etkisi")]
    // public float hızlanmaIncreasePerLevel = 2f;
    // public float frenIncreasePerLevel = 1.5f;
    // public float nitroIncreasePerLevel = 0.5f;
    // public float yoltutusIncreasePerLevel = 0.5f;
    // public float agırlıkIncreasePerLevel = 0.5f;
    
        kullanilanPuan = 0; 

        
        if (gelistirmeler.Length > 0 && gelistirmeler[0] != null)
        {
            gelistirmeler[0].OzellikDegerleriniAyarla(data.temelHızlanma, data.hızlanmaIncreasePerLevel);
        }
        
        if (gelistirmeler.Length > 1 && gelistirmeler[1] != null)
        {
            gelistirmeler[1].OzellikDegerleriniAyarla(data.temelFren, data.frenIncreasePerLevel);
        }
        
        if (gelistirmeler.Length > 1 && gelistirmeler[1] != null)
        {
            gelistirmeler[1].OzellikDegerleriniAyarla(data.temelNitro, data.nitroIncreasePerLevel);
        }
        
        if (gelistirmeler.Length > 1 && gelistirmeler[1] != null)
        {
            gelistirmeler[1].OzellikDegerleriniAyarla(data.temelYoltutus, data.yoltutusIncreasePerLevel);
        }
        
        if (gelistirmeler.Length > 1 && gelistirmeler[1] != null)
        {
            gelistirmeler[1].OzellikDegerleriniAyarla(data.temelAgırlık, data.agırlıkIncreasePerLevel);
        }
        
        
        GuncellePuanUI();
    }
    
    // 2. METOT: Oyuncu Hazır'a bastığında nihai değerleri statik veriye kaydet.
    public void KaydedilecekDegerleriAyarla(bool isP1)
    {
        // HIZ (Index 0)
        if (gelistirmeler.Length > 0 && gelistirmeler[0] != null)
        {
            float finalSpeed = gelistirmeler[0].GuncelDegeriGetir();
            if (isP1)
            {
                PlayerSelectionData.player1FinalHız = finalSpeed;
            }
            else
            {
                PlayerSelectionData.player2FinalHız = finalSpeed;
            }
        }

        // FREN (Index 1)
        if (gelistirmeler.Length > 1 && gelistirmeler[1] != null)
        {
            float finalBrake = gelistirmeler[1].GuncelDegeriGetir();
            if (isP1)
            {
                PlayerSelectionData.player1FinalFren = finalBrake;
            }
            else
            {
                PlayerSelectionData.player2FinalFren = finalBrake;
            }
        }
        
        if (gelistirmeler.Length > 2 && gelistirmeler[2] != null)
        {
            float finalNitro = gelistirmeler[2].GuncelDegeriGetir();
            if (isP1)
            {
                PlayerSelectionData.player1FinalNitro = finalNitro;
            }
            else
            {
                PlayerSelectionData.player2FinalNitro = finalNitro;
            }
        }
        
        if (gelistirmeler.Length > 3 && gelistirmeler[3] != null)
        {
            float finalYoltutus = gelistirmeler[3].GuncelDegeriGetir();
            if (isP1)
            {
                PlayerSelectionData.player1FinalYoltutus = finalYoltutus;
            }
            else
            {
                PlayerSelectionData.player2FinalYoltutus = finalYoltutus;
            }
        }
        
        if (gelistirmeler.Length > 4 && gelistirmeler[4] != null)
        {
            float finalAgırlık = gelistirmeler[4].GuncelDegeriGetir();
            if (isP1)
            {
                PlayerSelectionData.player1FinalAgırlık = finalAgırlık;
            }
            else
            {
                PlayerSelectionData.player2FinalAgırlık = finalAgırlık;
            }
        }
        
        
    }
    
    // --- PUAN YÖNETİMİ METOTLARI ---
    
    private void GuncellePuanUI()
    {
        int kalanPuan = toplamPuanHavuzu - kullanilanPuan;
        if (kalanPuanText != null)
        {
            kalanPuanText.text = "Kalan Puan: " + kalanPuan.ToString();
        }
        else
        {
            Debug.Log("Kalan Puan: " + kalanPuan);
        }
    }
    
    public void PuanDagitmayaCalis(int gelistirmeIndexi)
    {
        int kalanPuan = toplamPuanHavuzu - kullanilanPuan;
        
        if (kalanPuan >= BolmePuanDegerı && gelistirmeIndexi >= 0 && gelistirmeIndexi < gelistirmeler.Length)
        {
            int harcanan = gelistirmeler[gelistirmeIndexi].SeviyeArttirma();
            
            if (harcanan > 0)
            {
                kullanilanPuan += harcanan; 
                GuncellePuanUI();
            }
        }
        else if (kalanPuan < BolmePuanDegerı)
        {
            Debug.Log("Yeterli puan yok!");
        }
    }
    
    public void PuanGeriAlmayaCalis(int gelistirmeIndexi)
    {
        if (gelistirmeIndexi >= 0 && gelistirmeIndexi < gelistirmeler.Length)
        {
            int geriAlinan = gelistirmeler[gelistirmeIndexi].SeviyeAzaltma(); 
            
            if (geriAlinan < 0) 
            {
                kullanilanPuan += geriAlinan;
                GuncellePuanUI();
            }
        }
    }
}