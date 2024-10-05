using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    int gameSize = 10;
    public int generation = 0;
    public int population = 0;

    static Tilemap tilemap;
    static Grid grid;

    static Vector3 mouseWorldPos;
    static Vector3Int mouseCellCoords;
    public static bool isGameActive = false;

    [SerializeField]
    Slider gameSpeedSlider;

    [SerializeField]
    TextMeshProUGUI genText;

    [SerializeField]
    TextMeshProUGUI popText;

    [SerializeField]
    TextMeshProUGUI gameSizeInput;

    [SerializeField]
    GameObject uiActiveWhileGameIsPaused;

    [SerializeField]
    GameObject uiActiveWhileGameIsActive;

    CameraMove cam;

    private void Awake()
    {
        tilemap = GameObject.FindWithTag("tilemap").GetComponent<Tilemap>();
        grid = GameObject.FindWithTag("grid").GetComponent<Grid>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraMove>();
        uiActiveWhileGameIsActive.SetActive(false);
        // set up sliders here
    }

    public void SetNewBounds()
    {
        genText.text = "Generation: 0";
        popText.text = "Population: 0";
        // clear out all the existing tiles
        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }

        Vector3Int cellLoc;
        // enforce minimum size of 10x10
        if (gameSizeInput.text.Length < 3)
            gameSize = 10;
        else
            gameSize = int.Parse(gameSizeInput.text[..^1]);
        for (int x = 0; x < gameSize; x++)
        {
            for (int y = 0; y < gameSize; y++)
            {
                cellLoc = new(x, y, 0);
                Cell cell = ScriptableObject.CreateInstance<Cell>();
                tilemap.SetTile(cellLoc, cell);
            }
        }
        cam.CenterCam(
            grid.CellToWorld(
                new(Mathf.FloorToInt(gameSize / 2f), Mathf.FloorToInt(gameSize / 2f), 0)
            )
        );
        Cell.settingBounds = true;
        tilemap.RefreshAllTiles();
        Cell.settingBounds = false;
        isGameActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        // don't allow manual intervention while the game is running
        if (!isGameActive && Input.GetMouseButtonUp(0))
        {
            // mouse-cell detection stuff
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseCellCoords = grid.WorldToCell(new(mouseWorldPos.x, mouseWorldPos.y, 0));

            if (tilemap.HasTile(mouseCellCoords))
            {
                Cell cell = tilemap.GetTile(mouseCellCoords) as Cell;
                if (cell.GetLifeStatus())
                    cell.Die();
                else
                    cell.Life();
                Cell.settingBounds = true;
                tilemap.RefreshTile(mouseCellCoords);
                Cell.settingBounds = false;
            }
        }
    }

    public void StartGameOfLife()
    {
        isGameActive = true;
        uiActiveWhileGameIsPaused.SetActive(false);
        uiActiveWhileGameIsActive.SetActive(true);
        StartCoroutine(ConwaysGameOfLife());
    }

    public void PauseGameOfLife()
    {
        uiActiveWhileGameIsPaused.SetActive(true);
        uiActiveWhileGameIsActive.SetActive(false);
        StopAllCoroutines();
    }

    public void UpdatePopulation(int i)
    {
        population += i;
        popText.text = "Population: " + population;
    }

    IEnumerator ConwaysGameOfLife()
    {
        while (true)
        {
            Cell.phase = false;
            tilemap.RefreshAllTiles();
            Cell.phase = true;
            tilemap.RefreshAllTiles();
            generation++;
            genText.text = "Generation: " + generation;
            yield return new WaitForSeconds(gameSpeedSlider.value);
        }
    }

    public void AdvanceByOneGen()
    {
        isGameActive = true;
        Cell.phase = false;
        tilemap.RefreshAllTiles();
        Cell.phase = true;
        tilemap.RefreshAllTiles();
        generation++;
        genText.text = "Generation: " + generation;
        isGameActive = false;
    }
}
