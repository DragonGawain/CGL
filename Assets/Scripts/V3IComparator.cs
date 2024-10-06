using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3IComparator : IComparer<Vector3Int>
{
    // protected Vector3Int vec;

    public int Compare(Vector3Int vec1, Vector3Int other)
    {
        // throw new NotImplementedException();

        if (vec1.x == other.x && vec1.y == other.y)
            return 0;
        if (vec1.x < other.x)
            return -1;
        if (vec1.y < other.y)
            return -1;
        return 1;
    }

    // public int CompareTo(Vector3Int other)
    // {
    //     // throw new NotImplementedException();

    //     if (vec.Equals(other))
    //         return 0;
    //     if (vec.x < other.x)
    //         return -1;
    //     if (vec.y < other.y)
    //         return -1;
    //     return 1;
    // }
}
