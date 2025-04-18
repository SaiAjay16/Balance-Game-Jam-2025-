using UnityEngine;

public class SceneButtonLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneTransitionManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneButtonLoader: No scene name provided!");
        }
    }
}
