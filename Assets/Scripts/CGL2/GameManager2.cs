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
    public List<Cell2> allCells = new();

    // Start is called before the first frame update
    void Start()
    {
        allCells = FindObjectsOfType<Cell2>().ToList();
        // for (int i = 0; i < allCells.Count; i++)
        // {
        //     if (i != 0)
        //         allCells[i].AddNeighbor(allCells[i - 1].position, allCells[i - 1]);
        //     if (i != allCells.Count - 1)
        //         allCells[i].AddNeighbor(allCells[i + 1].position, allCells[i + 1]);
        // }
        foreach (Cell2 cellA in allCells)
        {
            foreach (Cell2 cellB in allCells)
            {
                if (cellA == cellB)
                    continue;
                if (
                    Mathf.Abs(cellA.position.x - cellB.position.x) <= 1
                    && Mathf.Abs(cellA.position.y - cellB.position.y) <= 1
                )
                    cellA.AddNeighbor(cellB.position, cellB);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        // don't allow manual intervention while the game is running
        if (!isGameActive && Input.GetMouseButtonUp(0))
        {
            // mouse-cell detection stuff
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
        }
    }

    public void NextGen()
    {
        foreach (Cell2 cell in allCells)
        {
            cell.NextGeneration();
        }

        foreach (KeyValuePair<Vector3Int, Cell2> cell in toDie)
        {
            allCells.Remove(cell.Value);
            Destroy(cell.Value.gameObject);
        }
        toDie.Clear();
    }
}
