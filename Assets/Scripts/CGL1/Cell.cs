using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Cell : TileBase
{
    public int nbLiveNeighbors = 0;
    public bool isLive = false;

    static Sprite black;
    static Sprite white;
    static Tilemap tilemapRef;
    static GameManager gameManager;

    Cell n = null;
    Cell ne = null;
    Cell nw = null;
    Cell s = null;
    Cell se = null;
    Cell sw = null;
    Cell e = null;
    Cell w = null;

    bool hasAllNeighbors = true;

    // 0: Die()
    // 1: Life()
    // 2: neither
    int willLive = 0;

    public static bool phase = false;
    public static bool settingBounds = false;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (black == null)
            black = Resources.Load<Sprite>("Solid_black");
        if (white == null)
            white = Resources.Load<Sprite>("Solid_white");
        if (tilemapRef == null)
            tilemapRef = GameObject.FindWithTag("tilemap").GetComponent<Tilemap>();
        if (gameManager == null)
            gameManager = GameObject.FindWithTag("gameManager").GetComponent<GameManager>();

        // N
        if (tilemapRef.HasTile(position + new Vector3Int(0, 1, 0)))
            n = tilemapRef.GetTile(position + new Vector3Int(0, 1, 0)) as Cell;
        else
            hasAllNeighbors = false;
        // NE
        if (tilemapRef.HasTile(position + new Vector3Int(1, 1, 0)))
            ne = tilemapRef.GetTile(position + new Vector3Int(1, 1, 0)) as Cell;
        else
            hasAllNeighbors = false;
        // NW
        if (tilemapRef.HasTile(position + new Vector3Int(-1, 1, 0)))
            nw = tilemapRef.GetTile(position + new Vector3Int(-1, 1, 0)) as Cell;
        else
            hasAllNeighbors = false;
        // S
        if (tilemapRef.HasTile(position + new Vector3Int(0, -1, 0)))
            s = tilemapRef.GetTile(position + new Vector3Int(0, -1, 0)) as Cell;
        else
            hasAllNeighbors = false;
        // SE
        if (tilemapRef.HasTile(position + new Vector3Int(1, -1, 0)))
            se = tilemapRef.GetTile(position + new Vector3Int(1, -1, 0)) as Cell;
        else
            hasAllNeighbors = false;
        // SW
        if (tilemapRef.HasTile(position + new Vector3Int(-1, -1, 0)))
            sw = tilemapRef.GetTile(position + new Vector3Int(-1, -1, 0)) as Cell;
        else
            hasAllNeighbors = false;
        // E
        if (tilemapRef.HasTile(position + new Vector3Int(1, 0, 0)))
            e = tilemapRef.GetTile(position + new Vector3Int(1, 0, 0)) as Cell;
        else
            hasAllNeighbors = false;
        // W
        if (tilemapRef.HasTile(position + new Vector3Int(-1, 0, 0)))
            w = tilemapRef.GetTile(position + new Vector3Int(-1, 0, 0)) as Cell;
        else
            hasAllNeighbors = false;

        return true;
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        Debug.Log("refresh");
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        Debug.Log("get tile data");

        if (settingBounds)
        {
            Debug.Log("setting bounds");
            if (isLive)
                tileData.sprite = white;
            else
                tileData.sprite = black;
        }

        // 0: Die()
        // 1: Life()
        // 2: neither
        if (GameManager.isGameActive && !phase)
        {
            Debug.Log("phase 1");
            willLive = 2;
            // 1. Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            // 3. Any live cell with more than three live neighbours dies, as if by overpopulation.
            if ((nbLiveNeighbors < 2 || nbLiveNeighbors > 3) && isLive)
                willLive = 0;
            // 4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
            else if (nbLiveNeighbors == 3 && !isLive)
                willLive = 1;
            // else if (nbLiveNeighbors > 3 && isLive)
            //     willLive = 0;
            // 2. Any live cell with two or three live neighbours lives on to the next generation.
            // else if ((nbLiveNeighbors == 2 || nbLiveNeighbors == 3) && isLive)
            //     willLive = 2;
        }

        if (GameManager.isGameActive && phase)
        {
            Debug.Log("phase 2");
            if (willLive == 0)
                Die();
            else if (willLive == 1)
                Life();

            if (isLive)
                tileData.sprite = white;
            else
                tileData.sprite = black;
        }
    }

    public void IncrementNeighborCount() => nbLiveNeighbors++;

    public void DecrementNeighborCount() => nbLiveNeighbors--;

    public void Life()
    {
        Debug.Log("life");
        isLive = true;
        if (hasAllNeighbors)
        {
            n.IncrementNeighborCount();
            ne.IncrementNeighborCount();
            nw.IncrementNeighborCount();
            s.IncrementNeighborCount();
            se.IncrementNeighborCount();
            sw.IncrementNeighborCount();
            e.IncrementNeighborCount();
            w.IncrementNeighborCount();
        }
        else
        {
            if (n != null)
                n.IncrementNeighborCount();
            if (ne != null)
                ne.IncrementNeighborCount();
            if (nw != null)
                nw.IncrementNeighborCount();
            if (s != null)
                s.IncrementNeighborCount();
            if (se != null)
                se.IncrementNeighborCount();
            if (sw != null)
                sw.IncrementNeighborCount();
            if (e != null)
                e.IncrementNeighborCount();
            if (w != null)
                w.IncrementNeighborCount();
        }
        gameManager.UpdatePopulation(1);
    }

    public void Die()
    {
        Debug.Log("death");
        isLive = false;
        if (hasAllNeighbors)
        {
            n.DecrementNeighborCount();
            ne.DecrementNeighborCount();
            nw.DecrementNeighborCount();
            s.DecrementNeighborCount();
            se.DecrementNeighborCount();
            sw.DecrementNeighborCount();
            e.DecrementNeighborCount();
            w.DecrementNeighborCount();
        }
        else
        {
            if (n != null)
                n.DecrementNeighborCount();
            if (ne != null)
                ne.DecrementNeighborCount();
            if (nw != null)
                nw.DecrementNeighborCount();
            if (s != null)
                s.DecrementNeighborCount();
            if (se != null)
                se.DecrementNeighborCount();
            if (sw != null)
                sw.DecrementNeighborCount();
            if (e != null)
                e.DecrementNeighborCount();
            if (w != null)
                w.DecrementNeighborCount();
        }
        gameManager.UpdatePopulation(-1);
    }

    public bool GetLifeStatus() => isLive;
}
