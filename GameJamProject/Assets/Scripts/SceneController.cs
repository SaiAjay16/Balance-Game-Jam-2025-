using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController:MonoBehaviour {

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
