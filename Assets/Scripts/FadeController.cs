using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Canvas fadeCanvasController; // Optional: Canvas for fade effects
    [SerializeField] private CanvasGroup fadeCanvas; // Canvas for fade effects
    [SerializeField] private float fadeDuration = 5.0f; // Duration of the fade

    private void Awake()
    {
        if (fadeCanvas == null)
        {
            Debug.LogError("FadeController: Fade Canvas is not assigned!");
        }
        else
        {
            fadeCanvasController.sortingOrder = 0;
        }
    }
  
    public void FadeIn()
    {
        if (fadeCanvas != null)
        {
            Debug.Log("Fading in...");
            StartCoroutine(FadeCanvasGroup(fadeCanvas, fadeCanvas.alpha, 0, fadeDuration));
        }
    }

    public void FadeOut()
    {
        if (fadeCanvas != null)
        {
            Debug.Log("Fading out...");
            fadeCanvasController.sortingOrder = 100;
            StartCoroutine(FadeCanvasGroup(fadeCanvas, fadeCanvas.alpha, 1, fadeDuration));
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha; // Ensure final alpha is set
        
    }
}
