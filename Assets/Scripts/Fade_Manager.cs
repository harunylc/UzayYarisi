using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // SceneManager için bu satır önemli
[RequireComponent(typeof(CanvasGroup))]
public class Fade_Manager : MonoBehaviour
{
public static Fade_Manager Instance { get; private set; }

[Header("Fade Ayarları")]
public float fadeDuration = 0.4f;

private CanvasGroup canvasGroup;

private void Awake()
{
    Debug.Log("Fade_Manager::Awake");
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    canvasGroup = GetComponent<CanvasGroup>();
}

// YENİ EKLENEN KISIM: Oyun ilk başladığında ekranı aydınlatmak için
private void Start()
{
    // Başlangıçta ekranı yavaşça aç
    StartCoroutine(FadeIn());
}

private void OnEnable()
{
    // Bir sahne yüklendiğinde OnSceneLoaded fonksiyonunu çağır
    SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnDisable()
{
    // Script kapandığında dinlemeyi bırak
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

// Yeni bir sahne yüklendiğinde bu fonksiyon otomatik çalışır
void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // Ve yeni sahneyi yavaşça açar (FadeIn)
    StartCoroutine(FadeIn());
}

// --- DIŞARIDAN ÇAĞRILACAK FONKSİYONLAR ---

public IEnumerator FadeOut()
{
    yield return StartCoroutine(Fade(0f, 1f));
}

public IEnumerator FadeIn()
{
    yield return StartCoroutine(Fade(1f, 0f));
}

public void StartFadeOutAndLoadScene(string sceneName)
{
    StartCoroutine(FadeOutAndLoad(sceneName));
}


// --- İÇ (PRIVATE) MANTIK ---

private IEnumerator FadeOutAndLoad(string sceneName)
{
    yield return FadeOut();
    SceneManager.LoadScene(sceneName);
}

private IEnumerator Fade(float startAlpha, float endAlpha)
{
    canvasGroup.blocksRaycasts = true;

    float elapsedTime = 0f;
    while (elapsedTime < fadeDuration)
    {
        elapsedTime += Time.deltaTime;
        canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
        yield return null;
    }

    canvasGroup.alpha = endAlpha;
    
    if (endAlpha == 0)
    {
        canvasGroup.blocksRaycasts = false;
    }
}
}