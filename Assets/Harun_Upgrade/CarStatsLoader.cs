using UnityEngine;

public class CarStatsLoader : MonoBehaviour
{
    [Header("Kimlik Ayarı")]
    public bool isPlayer1 = true; // P1 prefabında TİK AT, P2'de BOŞ BIRAK

    [Header("Otomatik Bulunacak Bileşenler")]
    public Rigidbody2D carRb; 
    private DriveMyCar controllerP1;
    private DriveMyCar_Player2 controllerP2;

    void Start()
    {
        // 1. Araba üzerindeki bileşenleri bul
        carRb = GetComponent<Rigidbody2D>();
        controllerP1 = GetComponent<DriveMyCar>();
        controllerP2 = GetComponent<DriveMyCar_Player2>();

        // 2. Verileri yükle ve uygula
        VerileriUygula();
    }

    void VerileriUygula()
    {
        // --- HAFIZADAKİ VERİLERİ ÇEK ---
        // Eğer Player 1 isek P1 verilerini, değilse P2 verilerini al
        float finalHiz = isPlayer1 ? PlayerSelectionData.player1FinalHız : PlayerSelectionData.player2FinalHız;
        float finalFren = isPlayer1 ? PlayerSelectionData.player1FinalFren : PlayerSelectionData.player2FinalFren;
        float finalNitro = isPlayer1 ? PlayerSelectionData.player1FinalNitro : PlayerSelectionData.player2FinalNitro;
        float finalYol = isPlayer1 ? PlayerSelectionData.player1FinalYoltutus : PlayerSelectionData.player2FinalYoltutus;
        float finalAgirlik = isPlayer1 ? PlayerSelectionData.player1FinalAgırlık : PlayerSelectionData.player2FinalAgırlık;

        // --- 1. AĞIRLIK (Weight) ---
        // Rigidbody'nin kütlesini değiştirir.
        if (carRb != null && finalAgirlik > 1f) 
        {
            carRb.mass = finalAgirlik;
        }

        // --- 2. FREN (Brake) ---
        // Senin isteğin üzerine: Fren gücünü "Drag" (Hava/Teker Sürtünmesi) olarak ayarlıyoruz.
        // Fren puanı yüksekse (örn: 40), araba gazı bırakınca hemen yavaşlar.
        if (carRb != null && finalFren > 0.1f)
        {
            // Formül: Puanı 20'ye bölüp Drag uygula. (Deneme yanılma ile 20'yi değiştirebilirsin)
            // Örn: Puan 20 ise Drag = 1 (Normal), Puan 40 ise Drag = 2 (Sıkı Fren)
            carRb.linearDamping = finalFren / 20f; 
        }

        // --- PLAYER 1 İÇİN YÜKLEME ---
        if (isPlayer1 && controllerP1 != null)
        {
            // Hızlanma -> Speed
            if(finalHiz > 1f) controllerP1.speed = finalHiz;
            
            // Nitro -> Nitro Boost
            if(finalNitro > 1f) controllerP1.nitroBoost = finalNitro;
            
            // Yol Tutuş -> Car Rotation Speed (Havada dönüş hızı)
            if(finalYol > 1f) controllerP1.carRotationSpeed = finalYol;
            
            Debug.Log($"P1 Statlar Yüklendi -> Hız:{finalHiz}, Fren(Drag):{carRb.linearDamping}, Ağırlık:{finalAgirlik}");
        }

        // --- PLAYER 2 İÇİN YÜKLEME ---
        else if (!isPlayer1 && controllerP2 != null)
        {
            // Hızlanma -> Speed
            if(finalHiz > 1f) controllerP2.speed = finalHiz;
            
            // Nitro -> Nitro Boost
            if(finalNitro > 1f) controllerP2.nitroBoost = finalNitro;
            
            // Yol Tutuş -> Car Rotation Speed
            if(finalYol > 1f) controllerP2.carRotationSpeed = finalYol;

            Debug.Log($"P2 Statlar Yüklendi -> Hız:{finalHiz}, Fren(Drag):{carRb.linearDamping}, Ağırlık:{finalAgirlik}");
        }
    }
}