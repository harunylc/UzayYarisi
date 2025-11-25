using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PU_DarkScreen : MonoBehaviour
{

    [Header("Panel Ayarları (Eğer Panel ise)")]
    public Image panelImage;

    public IEnumerator DarkenScreenRoutine(float duration)
    {
        
        Color color = panelImage.color;

        color.a = 242.25f / 255f;

        panelImage.color = color;

        yield return new WaitForSeconds(duration);

        color.a = 0f;

        panelImage.color = color;
    }
}