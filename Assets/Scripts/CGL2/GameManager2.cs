using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    bool isGameActive = false;

    [SerializeField]
    Slider gameSpeedSlider;

    [SerializeField]
    TextMeshProUGUI genText;

    [SerializeField]
    TextMeshProUGUI popText;

    [SerializeField]
    Transform cellParent;

    static Vector3 mouseWorldPos;
    static Vector3Int mouseCellCoords;
    GameObject white;

    int generation = 0;

    // GameObject black;
    // public static List<Vector3Int> toAdd = new();
    public static Dictionary<Vector3Int, Cell2> toDie = new();
    public static Dictionary<Vector3Int, Cell2> allCells = new();

    public static List<Vector3Int> frontier = new();

    // public static List<Vector3Int> frontierNoDupes = new();

    public static Dictionary<Vector3Int, int> countedFrontier = new();

    // static V3IComparator v3IComparator = new V3IComparator();

    GameObject newCellObject;
    Cell2 newCell;

    // Start is called before the first frame update
    void Start()
    {
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
            // if (allCells.ContainsKey(mouseCellCoords))
            //     return;

            if (allCells.ContainsKey(mouseCellCoords))
                Destroy(allCells[mouseCellCoords].gameObject);
            else
                CreateNewCell(mouseCellCoords);
        }
    }

    public void NextGen()
    {
        foreach (KeyValuePair<Vector3Int, Cell2> cell in allCells)
            cell.Value.NextGeneration();

        // frontier.Sort(v3IComparator);
        frontier.BuildTrueFrontier();

        foreach (KeyValuePair<Vector3Int, int> front in countedFrontier)
        {
            // Debug.Log("pos: " + front.Key + ", count: " + front.Value);
            if (front.Value == 3)
                CreateNewCell(front.Key);
        }

        foreach (KeyValuePair<Vector3Int, Cell2> cell in toDie)
            Destroy(cell.Value.gameObject);

        toDie.Clear();
        // toAdd.Clear();
        generation++;
        genText.text = "Generation: " + generation;
        popText.text = "Population: " + allCells.Count;
    }

    public void CreateNewCell(Vector3Int loc)
    {
        newCellObject = Instantiate(white, loc, Quaternion.identity, cellParent);
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

    IEnumerator Simulate()
    {
        while (true)
        {
            NextGen();
            yield return new WaitForSeconds(gameSpeedSlider.value);
        }
    }

    public void StartSimulation()
    {
        StartCoroutine(Simulate());
        isGameActive = true;
    }

    public void PauseSimulation()
    {
        StopAllCoroutines();
        isGameActive = false;
    }

    public void ClearSimulation()
    {
        foreach (Transform child in cellParent)
            Destroy(child.gameObject);
        genText.text = "Generation: 0";
        popText.text = "Population: 0";
        generation = 0;
    }
}
