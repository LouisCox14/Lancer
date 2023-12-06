using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Set")]
public class RuntimeSet : ScriptableObject
{
    /*
        Runtime sets are used to keep track of active objects without having to search the current scene. For instance, all enemies could be added to a set so that they can communicate easily.
    */

    public readonly List<GameObject> objects = new List<GameObject>(); // A list is kept of all registered objects. Readonly is used so that it can be accessed but not changed directly.

    // Adds an object to the list if it isn't already on it.
    public void RegisterSetObject(GameObject setObject)
    {
        if (!objects.Contains(setObject))
        {
            objects.Add(setObject);
        }
    }

    // Removes an object to the list if it is on it.
    public void UnregisterSetObject(GameObject setObject)
    {
        if (objects.Contains(setObject))
        {
            objects.Remove(setObject);
        }
    }
}
