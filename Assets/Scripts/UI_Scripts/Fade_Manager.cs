using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

[RequireComponent(typeof(CanvasGroup))]
public class Fade_Manager : MonoBehaviour
{
    public static Fade_Manager Instance { get; private set; }

    [Header("Fade Ayarları")] 
    public float fadeDuration = 0.4f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }


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

    public IEnumerator FadeOutThen(System.Action afterFade)
    {
        yield return StartCoroutine(FadeOut());
        if (afterFade != null)
            afterFade.Invoke();
    }
}
