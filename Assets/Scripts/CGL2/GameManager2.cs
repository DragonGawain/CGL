using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    bool isGameActive = false;

    static Vector3 mouseWorldPos;
    static Vector3Int mouseCellCoords;
    GameObject white;
    GameObject black;
    public static Dictionary<Vector3Int, Cell2> toAdd = new();
    public static Dictionary<Vector3Int, Cell2> toDie = new();
    public Dictionary<Vector3Int, Cell2> allCells = new();

    GameObject newCellObject;
    Cell2 newCell;

    // Start is called before the first frame update
    void Start()
    {
        Cell2[] cellArray = FindObjectsOfType<Cell2>();
        foreach (Cell2 cell in cellArray)
        {
            allCells.Add(cell.position, cell);
        }
        // for (int i = 0; i < allCells.Count; i++)
        // {
        //     if (i != 0)
        //         allCells[i].AddNeighbor(allCells[i - 1].position, allCells[i - 1]);
        //     if (i != allCells.Count - 1)
        //         allCells[i].AddNeighbor(allCells[i + 1].position, allCells[i + 1]);
        // }
        foreach (KeyValuePair<Vector3Int, Cell2> cellA in allCells)
        {
            foreach (KeyValuePair<Vector3Int, Cell2> cellB in allCells)
            {
                if (cellA.Value == cellB.Value)
                    continue;
                if (
                    Mathf.Abs(cellA.Value.position.x - cellB.Value.position.x) <= 1
                    && Mathf.Abs(cellA.Value.position.y - cellB.Value.position.y) <= 1
                )
                    cellA.Value.AddNeighbor(cellB.Value.position, cellB.Value);
            }
        }

        white = Resources.Load<GameObject>("White");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        // don't allow manual intervention while the game is running
        if (!isGameActive && Input.GetMouseButtonUp(1))
        {
            // mouse-cell detection stuff
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // mouseCellCoords = new(
            //     mouseWorldPos.x > 0
            //         ? Mathf.FloorToInt(mouseWorldPos.x)
            //         : Mathf.CeilToInt(mouseWorldPos.x),
            //     mouseWorldPos.y > 0
            //         ? Mathf.FloorToInt(mouseWorldPos.y)
            //         : Mathf.CeilToInt(mouseWorldPos.y),
            //     0
            // );

            mouseCellCoords = new(
                Mathf.RoundToInt(mouseWorldPos.x),
                Mathf.RoundToInt(mouseWorldPos.y),
                0
            );

            // if a cell already exists in that spot, do nothing (should kill the cell, but not yet...)
            if (allCells.ContainsKey(mouseCellCoords))
                return;

            newCellObject = Instantiate(white, mouseCellCoords, Quaternion.identity);
            newCell = newCellObject.GetComponent<Cell2>();
            foreach (KeyValuePair<Vector3Int, Cell2> cell in allCells)
            {
                if (
                    Mathf.Abs(newCell.position.x - cell.Value.position.x) <= 1
                    && Mathf.Abs(newCell.position.y - cell.Value.position.y) <= 1
                )
                {
                    newCell.AddNeighbor(cell.Value.position, cell.Value);
                    cell.Value.AddNeighbor(newCell.position, newCell);
                }
            }
            allCells.Add(newCell.position, newCell);
        }
    }

    public void NextGen()
    {
        foreach (KeyValuePair<Vector3Int, Cell2> cell in allCells)
        {
            cell.Value.NextGeneration();
        }

        foreach (KeyValuePair<Vector3Int, Cell2> cell in toDie)
        {
            allCells.Remove(cell.Key);
            Destroy(cell.Value.gameObject);
        }
        toDie.Clear();
    }
}
