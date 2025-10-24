using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image overlay;           // full-screen Image on your Canvas
    [SerializeField] Color flashColor = new Color(1, 0, 0, 0.35f);
    [SerializeField] float inTime  = 0.06f;
    [SerializeField] float outTime = 0.20f;

    void Reset()
    {
        // Try to auto-grab an Image on the same object
        overlay = GetComponent<Image>();
        if (overlay) overlay.raycastTarget = false;
    }

    public void Flash()
    {
        if (!overlay) return;
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(CoFlash());
    }

    IEnumerator CoFlash()
    {
        // Fade in
        float t = 0;
        while (t < inTime)
        {
            t += Time.unscaledDeltaTime;
            overlay.color = Color.Lerp(Color.clear, flashColor, t / inTime);
            yield return null;
        }

        // Fade out
        t = 0;
        while (t < outTime)
        {
            t += Time.unscaledDeltaTime;
            overlay.color = Color.Lerp(flashColor, Color.clear, t / outTime);
            yield return null;
        }

        overlay.color = Color.clear;
    }
}
