using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade_Manager : MonoBehaviour
{
    [Header("Fade Ayarları")]
    public Image fadeImage;           // FadePanel'deki Image component
    public float fadeDuration = 1f; // Geçiş hızı (saniye)

    private void Awake()
    {
        // Başta tamamen görünmez hale getir
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(0f, 1f));
    }

    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }

            yield return null;
        }
    }
}