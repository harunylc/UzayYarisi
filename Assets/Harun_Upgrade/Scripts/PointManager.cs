using UnityEngine;
using TMPro;
using System.Collections.Generic; 

public class PointManager : MonoBehaviour
{
    public List<int> bolumPuanlari = new List<int>(); 
    
    public int toplamPuanHavuzu = 0; 
    public int kullanilanPuan = 0; 
    
    public UpgradeManager[] gelistirmeler = new UpgradeManager[5]; 
    public TMP_Text kalanPuanText; 
    
    
    public void YükseltmeVerileriniYukle(CarStatsData data)
    {
        
        int suankiBolum = PlayerSelectionData.currentMapIndex;

        if (suankiBolum < bolumPuanlari.Count)
        {
            toplamPuanHavuzu = bolumPuanlari[suankiBolum];
        }
        else
        {
            toplamPuanHavuzu = 3; 
            Debug.LogWarning("Bu bölüm için puan ayarlanmamış, varsayılan 3 verildi.");
        }

        kullanilanPuan = 0; 

        bool isNewGame = (suankiBolum == 0);
        
        float currentHiz = isNewGame ? data.temelHızlanma : PlayerSelectionData.player1FinalHız;
        float currentFren = isNewGame ? data.temelFren : PlayerSelectionData.player1FinalFren;
        float currentNitro = isNewGame ? data.temelNitro : PlayerSelectionData.player1FinalNitro;
        float currentYol = isNewGame ? data.temelYoltutus : PlayerSelectionData.player1FinalYoltutus;
        float currentAgir = isNewGame ? data.temelAgırlık : PlayerSelectionData.player1FinalAgırlık;

        if (!isNewGame && currentHiz < 1f) currentHiz = data.temelHızlanma;

        if(CheckIndex(0)) gelistirmeler[0].OzellikDegerleriniAyarla(currentHiz, CarStatsData.MinHiz, CarStatsData.MaxHiz);
        if(CheckIndex(1)) gelistirmeler[1].OzellikDegerleriniAyarla(currentFren, CarStatsData.MinFren, CarStatsData.MaxFren);
        if(CheckIndex(2)) gelistirmeler[2].OzellikDegerleriniAyarla(currentNitro, CarStatsData.MinNitro, CarStatsData.MaxNitro);
        if(CheckIndex(3)) gelistirmeler[3].OzellikDegerleriniAyarla(currentYol, CarStatsData.MinYol, CarStatsData.MaxYol);
        if(CheckIndex(4)) gelistirmeler[4].OzellikDegerleriniAyarla(currentAgir, CarStatsData.MinAgir, CarStatsData.MaxAgir);
        
        GuncellePuanUI();
    }
    
    
    public void PuanDagitmayaCalis(int gelistirmeIndexi)
    {
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
        if(CheckIndex(0)) SetVal(isP1, 0, ref PlayerSelectionData.player1FinalHız, ref PlayerSelectionData.player2FinalHız);
        if(CheckIndex(1)) SetVal(isP1, 1, ref PlayerSelectionData.player1FinalFren, ref PlayerSelectionData.player2FinalFren);
        if(CheckIndex(2)) SetVal(isP1, 2, ref PlayerSelectionData.player1FinalNitro, ref PlayerSelectionData.player2FinalNitro);
        if(CheckIndex(3)) SetVal(isP1, 3, ref PlayerSelectionData.player1FinalYoltutus, ref PlayerSelectionData.player2FinalYoltutus);
        if(CheckIndex(4)) SetVal(isP1, 4, ref PlayerSelectionData.player1FinalAgırlık, ref PlayerSelectionData.player2FinalAgırlık);
        
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