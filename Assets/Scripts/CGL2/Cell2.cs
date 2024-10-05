using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell2 : MonoBehaviour
{
    public Vector3Int position;

    // int nbNeighbors;
    public Dictionary<Vector3Int, Cell2> neighbors = new();

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void AddNeighbor(Vector3Int position, Cell2 cell) => neighbors.Add(position, cell);

    public bool RemoveNeighbor(Vector3Int position) =>
        neighbors.ContainsKey(position) && neighbors.Remove(position);

    private void OnDestroy()
    {
        foreach (KeyValuePair<Vector3Int, Cell2> cell in neighbors)
        {
            cell.Value.RemoveNeighbor(position);
        }
    }
}
