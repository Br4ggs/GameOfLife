using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public bool isAlive;
    public MeshRenderer rnd;

    public Vector2 myPos;
    public Grid gm;

    public int aliveNBTiles;
	// Use this for initialization
	void Start ()
    {
        rnd = GetComponent<MeshRenderer>();
	}

	//checks wether the state of the tile should be alive or dead according to its surrounding neighbours
    public void CheckTile()
    {
        aliveNBTiles = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; //don't check myself
                int nbx = Mathf.CeilToInt(myPos.x + x);
                int nby = Mathf.CeilToInt(myPos.y + y);
                if (nbx >= 0 && nby >= 0 && nbx < gm.grid.GetLength(0) && nby < gm.grid.GetLength(1))
                {
                   if (gm.grid[nbx, nby].GetComponent<Tile>().isAlive) aliveNBTiles++;
                }
            }
        }
    }

    //updates the tile to the desired state decided in the Checktile command
    public void UpdateTile()
    {
        if      (aliveNBTiles == 3) isAlive = true;
        else if (aliveNBTiles <  2) isAlive = false;
        else if (aliveNBTiles >  3) isAlive = false;

        rnd.material.color = (isAlive ? Color.black : Color.white);
    }

    //sets the tile to the "dead" state
    public void ClearTile()
    {
        if (isAlive) rnd.material.color = Color.white;
        isAlive = false;
    }

    //sets the state to "alive" when there's input from the left mousebutton, "dead" if there's input from the right
    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Mouse0)) { isAlive = true;  rnd.material.color = Color.black; }
        if (Input.GetKey(KeyCode.Mouse1)) { isAlive = false; rnd.material.color = Color.white; }
    }
}
