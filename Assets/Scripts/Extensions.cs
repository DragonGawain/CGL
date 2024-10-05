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
}
