using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    class Node
    {
        public int gCost; // The steps from the start node to this one.
        public int hCost; // The distance from this node to the target.

        public Node connection; // The node just before this one in the path.
        public GameplayTileBase tile; // The tile this node is referencing.

        public float GetFCost()
        {
            return gCost + hCost + ((float)hCost / 100);
        }

        public Node(GameplayTileBase _tile, Node _connection, int _gCost, int _hCost)
        {
            tile = _tile;
            connection = _connection;
            gCost = _gCost;
            hCost = _hCost;
        }
    }

    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : System.IComparable
    {
        // This class is used with sorted lists to ensure that duplicate keys are allowed.

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;
            else
                return result;
        }
    }
    
    static class Pathfinder
    {
        public static List<GameplayTileBase> FindPath(GameplayTileBase startTile, GameplayTileBase targetTile)
        {
            Node startNode = new Node(startTile, null, 0, HexDistance((Vector2Int)startTile.position, (Vector2Int)targetTile.position));

            SortedList<float, Node> searchTargets = new SortedList<float, Node>(new DuplicateKeyComparer<float>()); // Sorted by the combined g and h costs of each node in order to lookup the cheapest faster.
            List<GameplayTileBase> searchedTiles = new List<GameplayTileBase>(); // Keeps track of nodes already searched.

            searchTargets.Add(startNode.GetFCost(), startNode);

            while (searchTargets.Count > 0)
            {
                Node currentNode = searchTargets.Values[0];
                searchTargets.RemoveAt(0);
                searchedTiles.Add(currentNode.tile);

                if (currentNode.hCost == 0)
                {
                    List<GameplayTileBase> path = new List<GameplayTileBase> { currentNode.tile };

                    while (currentNode.connection != null)
                    {
                        currentNode = currentNode.connection;
                        path.Insert(0, currentNode.tile);
                    }

                    return path;
                }

                foreach (GameplayTileBase neighbour in currentNode.tile.neighbours)
                {
                    if (searchedTiles.Contains(neighbour) || !neighbour.walkable)
                    {
                        continue;
                    }

                    Node newNode = new Node(neighbour, currentNode, currentNode.gCost + 1, HexDistance((Vector2Int)neighbour.position, (Vector2Int)targetTile.position));
                    searchTargets.Add(newNode.GetFCost(), newNode);
                }
            }

            return null;
        }

        public static int HexDistance(Vector2Int start, Vector2Int end)
        {
            Vector2Int axialStartCoord = HexCoordsToAxial(start);
            Vector2Int axialEndCoord = HexCoordsToAxial(end);
            Vector2Int difference = axialStartCoord - axialEndCoord;

            return (Mathf.Abs(difference.x) + Mathf.Abs(difference.x + difference.y) + Mathf.Abs(difference.y)) / 2;
        }

        public static Vector2Int HexCoordsToAxial(Vector2Int hex)
        {
            return new Vector2Int(hex.y, hex.x - (hex.y - (Mathf.Abs(hex.y) % 2)) / 2);
        }
    }
}