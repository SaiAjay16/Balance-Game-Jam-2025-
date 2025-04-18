using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string targetSceneName = "TestScene2"; // default next scene

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Only player triggers door
        {
            SceneTransitionManager.LoadScene("TestScene2");

        }
    }
}
