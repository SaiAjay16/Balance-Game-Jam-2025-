using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static bool isPuzzleOpen = false;

    public CanvasGroup puzzleCanvasGroup; // ✅ New: CanvasGroup reference
    public Transform puzzleBoard;
    public List<GameObject> tiles = new List<GameObject>();
    public GameObject emptyTile;
    public float fadeDuration = 0.5f; // ✅ Adjustable fade time

    private int gridSize = 3; // 3x3 puzzle

    private void Update()
    {
        if (!isPuzzleOpen) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) Move(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) Move(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Move(Vector2.left);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }
    }

    private void Move(Vector2 dir)
    {
        Vector2 emptyPos = emptyTile.transform.localPosition;
        Vector2 targetPos = emptyPos + new Vector2(dir.x * 110f, dir.y * 110f);

        foreach (var tile in tiles)
        {
            if (tile == null) continue;
            if (Vector2.Distance(tile.transform.localPosition, targetPos) < 10f)
            {
                Vector2 temp = tile.transform.localPosition;
                tile.transform.localPosition = emptyTile.transform.localPosition;
                emptyTile.transform.localPosition = temp;
                return;
            }
        }
    }

    public void OpenPuzzle()
    {
        StartCoroutine(FadeCanvas(1f)); // Fade in
        isPuzzleOpen = true;
    }

    public void ClosePuzzle()
    {
        StartCoroutine(FadeCanvas(0f)); // Fade out
        isPuzzleOpen = false;
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = puzzleCanvasGroup.alpha;
        float timer = 0f;

        if (targetAlpha == 1f)
        {
            puzzleCanvasGroup.blocksRaycasts = true;
            puzzleCanvasGroup.interactable = true;
        }

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            puzzleCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            timer += Time.deltaTime;
            yield return null;
        }

        puzzleCanvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0f)
        {
            puzzleCanvasGroup.blocksRaycasts = false;
            puzzleCanvasGroup.interactable = false;
        }
    }
}
