// UpgradeManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    // ARAYÜZ ELEMANLARI
    public List<Image> bolmeGorselleri = new List<Image>();
    public Sprite doluSprite;
    public Sprite bosSprite;
    
    // UI/Debug Amaçlı
    public string ozellikAdi = "Hız"; 
    
    // YÜKSELTME DEĞERLERİ (PointManager tarafindan ayarlanacak)
    private float temelDeger = 0f;
    private float seviyeBasinaArtis = 0f;
    
    // SEVİYE TAKİBİ
    private int mevcutSeviye = 0;
    private const int MaksımumSevıye = 10;
    private const int BolmePuanDegerı = 1; 

    void Start()
    {
        GelistirmeyiGuncelle();
    }
    
    // METOT 1: PointManager'ın çağırdığı, temel verileri ayarlayan metot.
    public void OzellikDegerleriniAyarla(float temel, float artis)
    {
        temelDeger = temel;
        seviyeBasinaArtis = artis;
        mevcutSeviye = 0; // Yeni araba seçildiğinde seviyeyi sıfırla
        GelistirmeyiGuncelle();
    }

    // METOT 2: PointManager'ın nihai değeri almak için çağırdığı metot.
    public float GuncelDegeriGetir()
    {
        return temelDeger + (mevcutSeviye * seviyeBasinaArtis);
    }
    
    // --- OYUNCU İŞLEMLERİ (PointManager tarafından çağrılır) ---

    public int SeviyeArttirma() 
    {
        if (mevcutSeviye < MaksımumSevıye)
        {
            mevcutSeviye += 1; 
            GelistirmeyiGuncelle();
            return BolmePuanDegerı; 
        }
        return 0; 
    }
    
    public int SeviyeAzaltma() 
    {
        if (mevcutSeviye > 0)
        {
            mevcutSeviye -= 1; 
            GelistirmeyiGuncelle();
            return -BolmePuanDegerı; 
        }
        return 0;
    }

    // --- ARAYÜZ GÜNCELLEME ---

    private void GelistirmeyiGuncelle()
    {
        for (int i = 0; i < MaksımumSevıye; i++)
        {
            if (i < bolmeGorselleri.Count) 
            {
                if (i < mevcutSeviye)
                {
                    bolmeGorselleri[i].sprite = doluSprite;
                }
                else
                {
                    bolmeGorselleri[i].sprite = bosSprite;
                }
            }
        }
            Debug.Log(ozellikAdi + ": " + GuncelDegeriGetir().ToString("F1"));  
        
    }
}