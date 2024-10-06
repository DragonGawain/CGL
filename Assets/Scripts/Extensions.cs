using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void NextGeneration(this Cell2 cell)
    {
        if (cell.neighbors.Count < 2 || cell.neighbors.Count > 3)
            GameManager2.toDie.Add(cell.position, cell);
    }

    public static void CountDuplicates(this List<Vector3Int> list)
    {
        GameManager2.countedFrontier.Clear();
        foreach (Vector3Int vec in list)
        {
            if (GameManager2.countedFrontier.ContainsKey(vec))
                GameManager2.countedFrontier[vec]++;
            else
                GameManager2.countedFrontier.Add(vec, 1);
        }
    }

    // public static bool AddToFrontier(this List<Vector3Int> list, Vector3Int item)
    // {
    //     list.Add(item);
    //     return true;
    // }
}
