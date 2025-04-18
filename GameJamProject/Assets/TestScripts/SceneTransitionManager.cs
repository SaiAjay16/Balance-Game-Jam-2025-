using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    public Image fadeImage;
    [Tooltip("Duration of fade in seconds")]
    public float fadeDuration = 1.5f;

    private bool isFading = false;
    private string targetSceneName = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (fadeImage != null && fadeImage.transform.parent != null)
            {
                DontDestroyOnLoad(fadeImage.transform.parent.gameObject);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadScene(string sceneName)
    {
        if (instance != null && !instance.isFading)
        {
            // 🛠 Check if the target scene is already loaded
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                Debug.LogWarning("SceneTransitionManager: Trying to load the same scene '" + sceneName + "'. Skipping transition.");
                return; // ❌ Cancel loading, already in this scene
            }

            instance.targetSceneName = sceneName;
            instance.StartCoroutine(instance.FadeOutAndLoad());
        }
        else
        {
            // fallback if no instance found
            SceneManager.LoadScene(sceneName);
        }
    }


    private IEnumerator FadeOutAndLoad()
    {
        isFading = true;
        yield return StartCoroutine(Fade(1f)); // Fade to black

        SceneManager.LoadScene(targetSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Fade(0f)); // Fade back to visible
    }

    private IEnumerator Fade(float targetAlpha)
    {
        fadeImage.raycastTarget = true;
        Color color = fadeImage.color;
        float startAlpha = color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;

        if (targetAlpha == 0f)
        {
            fadeImage.raycastTarget = false;
            isFading = false;
        }
    }
}
