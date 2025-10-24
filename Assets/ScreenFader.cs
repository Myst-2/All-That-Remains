using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    // -------- Singleton --------
    private static SceneFader _instance;
    public static SceneFader Instance
    {
        get
        {
            if (_instance) return _instance;

            
            var go = new GameObject("~SceneFader");
            _instance = go.AddComponent<SceneFader>();
            _instance.BuildCanvas();
            DontDestroyOnLoad(go);
            return _instance;
        }
    }

    [Header("Defaults")]
    [SerializeField] float defaultFadeOut = 0.35f;
    [SerializeField] float defaultFadeIn  = 0.35f;

    Canvas canvas;
    CanvasGroup cg;
    Image black;

    void Awake()
    {
        // Enforce single instance
        if (_instance && _instance != this)
        {
            // Ensure the old instance unsubscribes (in case it didn't)
            _instance.UnhookSceneLoaded();
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (!canvas) BuildCanvas();
        HookSceneLoaded();
    }

    void OnDestroy()
    {
        if (_instance == this) UnhookSceneLoaded();
    }

    // Subscribe/unsubscribe safely
    void HookSceneLoaded()
    {
        UnhookSceneLoaded(); // avoid double-subscribe
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void UnhookSceneLoaded()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Auto-fade IN on every scene load
    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        if (!cg) return;
        StopAllCoroutines();
        StartCoroutine(Fade(1f, 0f, defaultFadeIn));
    }

    void BuildCanvas()
    {
        // Canvas
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;

        // CanvasGroup (controls alpha)
        cg = gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = true; // block clicks while faded out
        cg.alpha = 0f; // start clear

        // Black full-screen Image
        black = new GameObject("Black").AddComponent<Image>();
        black.transform.SetParent(canvas.transform, false);
        black.color = Color.black;

        var rt = black.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        if (!cg) yield break;
        cg.alpha = from;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / Mathf.Max(0.0001f, duration));
            yield return null;
        }
        cg.alpha = to;
        cg.blocksRaycasts = to >= 0.99f; // only block when fully black
    }

    IEnumerator FadeAndSwitch(string sceneName, float outDur, float inDur)
    {
        yield return Fade(0f, 1f, outDur);
        yield return SceneManager.LoadSceneAsync(sceneName);
        // OnSceneLoaded will run and fade back in with defaultFadeIn.
        
        if (Mathf.Abs(inDur - defaultFadeIn) > 0.0001f)
            yield return Fade(1f, 0f, inDur);
    }

    // -------- Public API --------
    public static void LoadScene(string sceneName, float fadeOut = -1f, float fadeIn = -1f)
    {
        var f = Instance;
        if (fadeOut < 0f) fadeOut = f.defaultFadeOut;
        if (fadeIn  < 0f) fadeIn  = f.defaultFadeIn;
        f.StopAllCoroutines();
        f.StartCoroutine(f.FadeAndSwitch(sceneName, fadeOut, fadeIn));
    }

    public static void FadeIn(float duration = -1f)
    {
        var f = Instance;
        if (duration < 0f) duration = f.defaultFadeIn;
        f.StopAllCoroutines();
        f.StartCoroutine(f.Fade(1f, 0f, duration));
    }

    public static void FadeOut(float duration = -1f)
    {
        var f = Instance;
        if (duration < 0f) duration = f.defaultFadeOut;
        f.StopAllCoroutines();
        f.StartCoroutine(f.Fade(0f, 1f, duration));
    }
}
