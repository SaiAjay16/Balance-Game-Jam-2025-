
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    public int gridSize = 3;
    public GameObject tilePrefab;
    public Sprite fullImage;
    public RectTransform boardParent;

    private Vector2 emptySlot;
    private GameObject[,] tiles;

    void Start()
    {
        CreatePuzzle();
    }

    void CreatePuzzle()
    {
        Texture2D texture = fullImage.texture;
        float tileSize = 100f;
        tiles = new GameObject[gridSize, gridSize];
        emptySlot = new Vector2(gridSize - 1, gridSize - 1);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (x == gridSize - 1 && y == gridSize - 1)
                    continue;

                GameObject tile = Instantiate(tilePrefab, boardParent);
                tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * tileSize, -y * tileSize);
                tile.name = $"{x},{y}";

                int index = y * gridSize + x;
                Sprite tileSprite = Sprite.Create(texture,
                    new Rect((texture.width / gridSize) * x, texture.height - (texture.height / gridSize) * (y + 1),
                    texture.width / gridSize, texture.height / gridSize),
                    new Vector2(0.5f, 0.5f));

                tile.GetComponent<Image>().sprite = tileSprite;
                tile.GetComponent<Button>().onClick.AddListener(() => OnTileClicked(tile));

                tiles[x, y] = tile;
            }
        }

        Shuffle();
    }

    void OnTileClicked(GameObject tile)
    {
        Vector2 pos = GetTilePosition(tile);
        if (IsAdjacent(pos, emptySlot))
        {
            MoveTile(pos, emptySlot);
            emptySlot = pos;

            if (IsSolved())
            {
                Debug.Log("PUZZLE SOLVED!");
            }
        }
    }

    Vector2 GetTilePosition(GameObject tile)
    {
        string[] parts = tile.name.Split(',');
        return new Vector2(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    bool IsAdjacent(Vector2 a, Vector2 b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) ||
               (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }

    void MoveTile(Vector2 from, Vector2 to)
    {
        GameObject tile = tiles[(int)from.x, (int)from.y];
        tiles[(int)to.x, (int)to.y] = tile;
        tiles[(int)from.x, (int)from.y] = null;

        tile.name = $"{(int)to.x},{(int)to.y}";

        StartCoroutine(AnimateTileMove(tile.GetComponent<RectTransform>(), from, to));
    }

    IEnumerator AnimateTileMove(RectTransform tile, Vector2 from, Vector2 to)
    {
        Vector2 startPos = new Vector2(from.x * 100f, -from.y * 100f);
        Vector2 endPos = new Vector2(to.x * 100f, -to.y * 100f);

        float duration = 0.2f;
        float time = 0;

        while (time < duration)
        {
            tile.anchoredPosition = Vector2.Lerp(startPos, endPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        tile.anchoredPosition = endPos;
    }

    void Shuffle()
    {
        for (int i = 0; i < 100; i++)
        {
            List<Vector2> neighbors = GetNeighbors(emptySlot);
            Vector2 move = neighbors[Random.Range(0, neighbors.Count)];
            MoveTile(move, emptySlot);
            emptySlot = move;
        }
    }

    List<Vector2> GetNeighbors(Vector2 pos)
    {
        List<Vector2> neighbors = new List<Vector2>();

        if (pos.x > 0) neighbors.Add(new Vector2(pos.x - 1, pos.y));
        if (pos.x < gridSize - 1) neighbors.Add(new Vector2(pos.x + 1, pos.y));
        if (pos.y > 0) neighbors.Add(new Vector2(pos.x, pos.y - 1));
        if (pos.y < gridSize - 1) neighbors.Add(new Vector2(pos.x, pos.y + 1));

        return neighbors;
    }

    bool IsSolved()
    {
        int count = 0;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (x == gridSize - 1 && y == gridSize - 1)
                    continue;

                GameObject tile = tiles[x, y];
                if (tile.name != $"{x},{y}")
                    return false;

                count++;
            }
        }
        return true;
    }
}
