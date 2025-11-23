using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("UI Ayarları")]
    public List<Image> bolmeGorselleri = new List<Image>(); 
    public Sprite doluSprite;
    public Sprite bosSprite;
    
    public string ozellikAdi = "Ozellik"; 
    
    // Matematik Değişkenleri
    private float minDeger = 0f;
    private float maxDeger = 100f;
    private float seviyeBasinaArtis = 0f;
    
    private int mevcutSeviye = 0;
    
    // --- GÜVENLİK DEĞİŞKENİ ---
    // Oyuncunun bu sahneye girdiğindeki seviyesi.
    // Bunun altına düşmesine izin vermeyeceğiz.
    private int kilitliBaslangicSeviyesi = 0; 
    // ---------------------------
    
    private const int ToplamBarSayisi = 10; 
    private const int PuanMaliyeti = 1; 

    // KURULUM
    public void OzellikDegerleriniAyarla(float arabaDegeri, float oyunMin, float oyunMax)
    {
        minDeger = oyunMin;
        maxDeger = oyunMax;
        
        seviyeBasinaArtis = (maxDeger - minDeger) / ToplamBarSayisi;
        
        float fark = arabaDegeri - minDeger;
        
        if (seviyeBasinaArtis > 0.001f)
        {
            mevcutSeviye = Mathf.RoundToInt(fark / seviyeBasinaArtis);
        }
        else
        {
            mevcutSeviye = 0;
        }

        mevcutSeviye = Mathf.Clamp(mevcutSeviye, 0, ToplamBarSayisi);

        // --- KİLİT NOKTASI ---
        // Sahne açıldığında hesaplanan bu seviye, artık bizim tabanımızdır.
        kilitliBaslangicSeviyesi = mevcutSeviye;
        // ---------------------

        GorselleriGuncelle();
    }
    
    public float GuncelDegeriGetir()
    {
        return minDeger + (mevcutSeviye * seviyeBasinaArtis);
    }
    
    public int SeviyeArttirma() 
    {
        if (mevcutSeviye < ToplamBarSayisi)
        {
            mevcutSeviye++; 
            GorselleriGuncelle();
            return PuanMaliyeti; 
        }
        return 0; 
    }
    
    public int SeviyeAzaltma() 
    {
        // DEĞİŞİKLİK BURADA:
        // Eskiden "mevcutSeviye > 0" diyorduk.
        // Artık "mevcutSeviye > kilitliBaslangicSeviyesi" diyoruz.
        
        if (mevcutSeviye > kilitliBaslangicSeviyesi)
        {
            mevcutSeviye--; 
            GorselleriGuncelle();
            return -PuanMaliyeti; // Puanı iade et
        }
        
        // Eğer kilitli seviyeye geldiyse azaltma yapma ve 0 döndür (Para iadesi yok)
        return 0;
    }

    private void GorselleriGuncelle()
    {
        for (int i = 0; i < ToplamBarSayisi; i++)
        {
            if (i < bolmeGorselleri.Count && bolmeGorselleri[i] != null) 
            {
                if (i < mevcutSeviye)
                    bolmeGorselleri[i].sprite = doluSprite;
                else
                    bolmeGorselleri[i].sprite = bosSprite;
            }
        }
    }
}