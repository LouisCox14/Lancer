using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Lookup Set")]
public class GridLookupSet : ScriptableObject
{
    public readonly Dictionary<Vector2Int, GridObject> objects = new Dictionary<Vector2Int, GridObject>();
    public int count;

    public void RegisterSetObject(GridObject setObject)
    {
        if (!objects.ContainsKey(setObject.position))
        {
            objects.Add(setObject.position, setObject);
        }

        count = objects.Count;
    }

    public void UnregisterObjectAt(Vector2Int position)
    {
        if (objects.ContainsKey(position))
        {
            objects.Remove(position);
        }

        count = objects.Count;
    }

    public GridObject GetObjectAtGridPos(Vector2Int gridPos)
    {
        GridObject value;
        if (objects.TryGetValue(gridPos, out value))
        {
            return value;
        } 
        else 
        {
            return null;
        }
    }

    public List<GridObject> GetNeighbours(Vector2Int gridPos)
    {
        List<GridObject> neighbours = new List<GridObject>();
        
        neighbours.Add(GetObjectAtGridPos(gridPos + new Vector2Int(0 - (Mathf.Abs(gridPos.y + 1) % 2), 1)));
        neighbours.Add(GetObjectAtGridPos(gridPos + new Vector2Int(1 - (Mathf.Abs(gridPos.y + 1) % 2), 1)));
        neighbours.Add(GetObjectAtGridPos(gridPos + new Vector2Int(-1, 0)));
        neighbours.Add(GetObjectAtGridPos(gridPos + new Vector2Int(1, 0)));
        neighbours.Add(GetObjectAtGridPos(gridPos + new Vector2Int(0 - (Mathf.Abs(gridPos.y + 1) % 2), -1)));
        neighbours.Add(GetObjectAtGridPos(gridPos + new Vector2Int(1 - (Mathf.Abs(gridPos.y + 1) % 2), -1)));

        neighbours.RemoveAll(item => item == null);
        return neighbours;
    }
}
