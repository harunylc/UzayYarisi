using UnityEngine;

public class CarStatsLoader : MonoBehaviour
{
    public bool isPlayer1 = true;
    public Rigidbody2D carRb; 
    private DriveMyCar _controllerP1;
    private DriveMyCar_Player2 _controllerP2;

    void Start()
    {
        carRb = GetComponent<Rigidbody2D>();
        _controllerP1 = GetComponent<DriveMyCar>();
        _controllerP2 = GetComponent<DriveMyCar_Player2>();
        VerileriUygula();
    }

    void VerileriUygula()
    {
        float finalHiz = isPlayer1 ? PlayerSelectionData.player1FinalHız : PlayerSelectionData.player2FinalHız;
        float finalFren = isPlayer1 ? PlayerSelectionData.player1FinalFren : PlayerSelectionData.player2FinalFren;
        float finalNitro = isPlayer1 ? PlayerSelectionData.player1FinalNitro : PlayerSelectionData.player2FinalNitro;
        float finalYol = isPlayer1 ? PlayerSelectionData.player1FinalYoltutus : PlayerSelectionData.player2FinalYoltutus;
        float finalAgirlik = isPlayer1 ? PlayerSelectionData.player1FinalAgırlık : PlayerSelectionData.player2FinalAgırlık;

        if (carRb != null && finalAgirlik > 1f) 
        {
            carRb.mass = finalAgirlik;
        }

        if (carRb != null && finalFren > 0.1f)
        {
            carRb.linearDamping = finalFren / 20f; 
        }
        if (isPlayer1 && _controllerP1 != null)
        {
            if(finalHiz > 1f) _controllerP1.speed = finalHiz;
            
            if(finalNitro > 1f) _controllerP1.nitroBoost = finalNitro;
            
            if(finalYol > 1f) _controllerP1.carRotationSpeed = finalYol;
        }

        else if (!isPlayer1 && _controllerP2 != null)
        {
            if(finalHiz > 1f) _controllerP2.speed = finalHiz;
            
            if(finalNitro > 1f) _controllerP2.nitroBoost = finalNitro;
            
            if(finalYol > 1f) _controllerP2.carRotationSpeed = finalYol;

        }
    }
}