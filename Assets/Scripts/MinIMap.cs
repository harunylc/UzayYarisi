using UnityEngine;
using UnityEngine.UI;

public class RaceProgressUI : MonoBehaviour
{
    [Header("Oyun Dünyası Referansları")]
    public Transform flag;

    [Header("UI Referansları")]
    public RectTransform trackPath;
    public RectTransform player1Icon;
    public RectTransform player2Icon;

    private Transform player1Car;
    private Transform player2Car;

    private float initialDistanceP1 = -1f;
    private float initialDistanceP2 = -1f;
    private float pathStart, pathMiddle, pathEnd;

    void Start()
    {
        float pathWidth = trackPath.rect.width;
        pathStart = 0;
        pathMiddle = pathWidth / 2f;
        pathEnd = pathWidth;

        // İkonları başlangıç pozisyonlarına ayarla
        if(player1Icon != null) player1Icon.anchoredPosition = new Vector2(pathStart, player1Icon.anchoredPosition.y);
        if(player2Icon != null) player2Icon.anchoredPosition = new Vector2(pathEnd, player2Icon.anchoredPosition.y);
    }

    void Update()
    {
        // --- Oyuncu 1'i Yönet ---
        if (player1Car == null)
        {
            // Arabayı sahnede Tag'i ile ara.
            GameObject p1Object = GameObject.FindWithTag("Player");
            if (p1Object != null)
            {
                player1Car = p1Object.transform;
                
                // Başlangıç mesafesini SADECE BİR KERE (initialDistance < 0 iken) hesapla.
                if (initialDistanceP1 < 0f && flag != null) 
                {
                    initialDistanceP1 = Vector2.Distance(player1Car.position, flag.position);
                }
            }
        }
        else // Eğer araba zaten biliniyorsa...
        {
            // ...ikonunun pozisyonunu güncelle.
            UpdatePlayerIcon(player1Car, player1Icon, initialDistanceP1, pathStart, pathMiddle);
        }

        // --- Oyuncu 2'yi Yönet (Aynı mantık) ---
        if (player2Car == null)
        {
            GameObject p2Object = GameObject.FindWithTag("Player2");
            if (p2Object != null)
            {
                player2Car = p2Object.transform;
                if (initialDistanceP2 < 0f && flag != null)
                {
                    initialDistanceP2 = Vector2.Distance(player2Car.position, flag.position);
                }
            }
        }
        else
        {
            UpdatePlayerIcon(player2Car, player2Icon, initialDistanceP2, pathEnd, pathMiddle);
        }
    }

    private void UpdatePlayerIcon(Transform car, RectTransform icon, float initialDistance, float mapStart, float mapEnd)
    {
        // initialDistance hala hesaplanmamışsa bir şey yapma.
        if (initialDistance < 0f) return; 

        float currentDistance = Vector2.Distance(car.position, flag.position);
        
        // İlerleme oranını hesapla (0 = hiç ilerlemedi, 1 = bayrağa ulaştı)
        float progress = 1.0f - (currentDistance / initialDistance);
        progress = Mathf.Clamp01(progress); // Değerin 0 ile 1 arasında kalmasını garanti et

        // UI ikonunun yeni X pozisyonunu hesapla
        float newX = Mathf.Lerp(mapStart, mapEnd, progress);

        // İkonun pozisyonunu güncelle
        Vector2 pos = icon.anchoredPosition;
        pos.x = newX;
        icon.anchoredPosition = pos;
    }
}