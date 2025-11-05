using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public List<Image> bolmeGorselleri = new List<Image>();
    public Sprite doluSprite;
    public Sprite bosSprite;

    private int mevcutSeviye = 0;
    private const int MaksımumSevıye = 10;
    private const int BolmePuanDegerı = 1; 

    void Start()
    {
        GelistirmeyiGuncelle();
    }
    
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

    private void GelistirmeyiGuncelle()
    {
        for (int i = 0; i < MaksımumSevıye; i++)
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
}