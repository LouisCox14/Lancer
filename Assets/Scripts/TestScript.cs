using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestScript : MonoBehaviour
{
    public Entity player;
    public GridLookupSet terrain;
    public ComponentObject grid;
    public int mouseButton;

    void Update()
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            Grid _grid = (Grid)grid.value;
            player.FollowPath(Pathfinder.FindPath((GameplayTileBase)terrain.GetObjectAtGridPos(player.position), (GameplayTileBase)terrain.GetObjectAtGridPos((Vector2Int)_grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)))));
        }
    }
}
