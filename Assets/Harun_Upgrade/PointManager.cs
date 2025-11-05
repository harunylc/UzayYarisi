using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    public int toplamPuanHavuzu = 50; 
    public int kullanilanPuan = 0; 
    public const int BolmePuanDegerı = 1;
    public UpgradeManager[] gelistirmeler = new UpgradeManager[5]; 
    public TMP_Text kalanPuanText; 
    
    
    void Start()
    {
        GuncellePuanUI();
    }
    
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