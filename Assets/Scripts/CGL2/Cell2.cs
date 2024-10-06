using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell2 : MonoBehaviour
{
    public Vector3Int position;

    // int nbNeighbors;
    public Dictionary<Vector3Int, Cell2> neighbors = new();
    public Dictionary<Vector3Int, Cell2> nonNeighbors = new();
    public int nbNeighbors;

    // Start is called before the first frame update
    void Awake()
    {
        this.position = new(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            0
        );

        // Add all neighbors to the frontier

        // if (!GameManager2.allCells.ContainsKey(position + new Vector3Int(1, 1, 0)))
        GameManager2.frontier.Add(position + new Vector3Int(1, 1, 0));

        // if (!GameManager2.allCells.ContainsKey(position + new Vector3Int(1, 0, 0)))
        GameManager2.frontier.Add(position + new Vector3Int(1, 0, 0));

        // if (!GameManager2.allCells.ContainsKey(position + new Vector3Int(1, -1, 0)))
        GameManager2.frontier.Add(position + new Vector3Int(1, -1, 0));

        // if (!GameManager2.allCells.ContainsKey(position + new Vector3Int(0, 1, 0)))
        GameManager2.frontier.Add(position + new Vector3Int(0, 1, 0));

        // if (!GameManager2.allCells.ContainsKey(position + new Vector3Int(0, -1, 0)))
        GameManager2.frontier.Add(position + new Vector3Int(0, -1, 0));

        // if (!GameManager2.allCells.ContainsKey(position + new Vector3Int(-1, 1, 0)))
        GameManager2.frontier.Add(position + new Vector3Int(-1, 1, 0));

        // if (!GameManager2.allCells.ContainsKey(position + new Vector3Int(-1, 0, 0)))
        GameManager2.frontier.Add(position + new Vector3Int(-1, 0, 0));

        // if (!GameManager2.allCells.ContainsKey(position + new Vector3Int(-1, -1, 0)))
        GameManager2.frontier.Add(position + new Vector3Int(-1, -1, 0));

        // Remove self from frontier
        // GameManager2.frontier.RemoveAll(RemoveAllCondition);
    }

    public void AddNeighbor(Vector3Int position, Cell2 cell) => neighbors.Add(position, cell);

    public bool RemoveNeighbor(Vector3Int position) =>
        neighbors.ContainsKey(position) && neighbors.Remove(position);

    private void OnDestroy()
    {
        // // return this to the frontier based on the number of neighbors that it had
        // for (int i = 0; i < neighbors.Count; i++)
        //     GameManager2.frontier.Add(position);

        GameManager2.frontier.Remove(position + new Vector3Int(1, 1, 0));
        GameManager2.frontier.Remove(position + new Vector3Int(1, 0, 0));
        GameManager2.frontier.Remove(position + new Vector3Int(1, -1, 0));
        GameManager2.frontier.Remove(position + new Vector3Int(0, 1, 0));
        GameManager2.frontier.Remove(position + new Vector3Int(0, -1, 0));
        GameManager2.frontier.Remove(position + new Vector3Int(-1, 1, 0));
        GameManager2.frontier.Remove(position + new Vector3Int(-1, 0, 0));
        GameManager2.frontier.Remove(position + new Vector3Int(-1, -1, 0));

        foreach (KeyValuePair<Vector3Int, Cell2> cell in neighbors)
            cell.Value.RemoveNeighbor(position);
        GameManager2.allCells.Remove(position);
    }

    private bool RemoveAllCondition(Vector3Int pos)
    {
        return pos.Equals(position);
    }
}
