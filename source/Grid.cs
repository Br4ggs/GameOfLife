using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class Grid : MonoBehaviour
{
    public GameObject tileSelector;
    public Dropdown tileSelectorScript;

    public GameObject tile;
    public GameObject[,] grid;
    public Vector2 fieldsize;

    public bool isPlaying;
    public bool calculating;

    public Camera mainCam;
    public Vector3 lastPos;
    public Vector3 targetPos;
    public float startTime;

    public float nextCalcTime;
    public GameObject timeSlider;
    public Slider timeSliderScript;

    void Start ()
    {
        tileSelector        = GameObject.Find("TileMenu");
        tileSelectorScript  = tileSelector.GetComponent<Dropdown>();
        timeSlider          = GameObject.Find("TimeSlider");
        timeSliderScript    = timeSlider.GetComponent<Slider>();

        GridSize();

        calculating = true;
        GenerateGrid();
        SetCam();
	}
	
	void Update ()
    {
        SetCam();

        //makes sure that every tile runs the CheckTile command if the grid is in calculating state
        if (Time.time <= nextCalcTime) return;
        nextCalcTime = Time.time + timeSliderScript.value;

        if (!isPlaying || grid[0, 0] == null) return;

        for (int x = 0; x < fieldsize.x; x++) for (int y = 0; y < fieldsize.y; y++)
        {
            if (calculating) grid[x, y].GetComponent<Tile>().CheckTile();
            else             grid[x, y].GetComponent<Tile>().UpdateTile();
        }
        calculating = !calculating;
    }

    //creates a new gameobject tile for every space in the array
    public void GenerateGrid()
    {
        grid = new GameObject[Mathf.CeilToInt(fieldsize.x), Mathf.CeilToInt(fieldsize.y)];
        Vector3 pos = new Vector3(0, 0, 0);
        for (int x = 0; x < fieldsize.x; x++)
            for (int y = 0; y < fieldsize.y; y++)
            {
                pos.x = x;
                pos.y = y;
                GameObject newTile = (GameObject)Instantiate(tile, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
                newTile.name = x + "," + y;
                newTile.GetComponent<Tile>().myPos = pos;
                newTile.GetComponent<Tile>().gm = GetComponent<Grid>();
                newTile.tag = "Tile";
                grid[x, y]  = newTile;
            }
    }

    //clears all the gameobject that are stored inside the grid
    public void DeleteGrid()
    {
        for (int x = 0; x < fieldsize.x; x++)
            for (int y = 0; y < fieldsize.y; y++)
                Destroy(grid[x, y]);
    }

    //switches between starting and stopping
    public void SwitchPlaying()
    {
        isPlaying   = !isPlaying;
        calculating = true;
    }

    //issues the ClearTiles command for all tiles in the array
    public void ClearTiles()
    {
        for (int x = 0; x < fieldsize.x; x++)
            for (int y = 0; y < fieldsize.y; y++)
                grid[x, y].GetComponent<Tile>().ClearTile();
    }

    //sets the gridsize according to the filled in value
    public void GridSize()
    {
        switch (tileSelectorScript.value)
        {
            case 0: fieldsize = new Vector2(10, 10); break;
            case 1: fieldsize = new Vector2(20, 20); break;
            case 2: fieldsize = new Vector2(30, 30); break;
            case 3: fieldsize = new Vector2(40, 40); break;
        }

        startTime = Time.time;
        lastPos   = mainCam.transform.position;
        targetPos = new Vector3(fieldsize.x / 2, fieldsize.y / 2, -fieldsize.x);
    }

    public void SetCam()
    {       
        mainCam.transform.position = Vector3.Lerp(lastPos, targetPos, 0.5f * (Time.time - startTime));
    }

    public void StopPlaying()
    {
        isPlaying = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

}
