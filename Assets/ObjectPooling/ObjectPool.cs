using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object Pool")]
public class ObjectPool : ScriptableObject
{
    /*
        This class is used as a container to keep track of object pools. Thanks to the scriptable object approach to object pooling, bjects set up here can be carried across scenes, and this container can be referenced in prefab inspectors.
    */

    [SerializeField] private PoolableObject prefab; // Used to pick the object this pool stores.
    [SerializeField] private int count; // Used to select the amount of objects avaliable.
    
    private List<PoolableObject> pooledObjects = new List<PoolableObject>(); // Used to keep track of inactive objects avaliable for use.
    private List<PoolableObject> activeObjects = new List<PoolableObject>(); // Used to keep track of active objects in the scene.

    void Awake()
    {
        pooledObjects = new List<PoolableObject>();
        activeObjects = new List<PoolableObject>();
    }

    // This function returns an object from the pool for use in a scene.
    public GameObject RequestFromPool()
    {
        if (pooledObjects.Count > 0) // If objects are avaliable from the pool, return one.
        {
            PoolableObject targetObject = pooledObjects[0];
            targetObject.Reset();

            activeObjects.Add(targetObject);
            pooledObjects.Remove(targetObject);

            return targetObject.gameObject;
        }
        else if (activeObjects.Count > 0) // If none are avaliable in the pool, move the oldest active object to the back of the queue and reassign it.
        {
            PoolableObject targetObject = activeObjects[0];
            targetObject.Reset();

            activeObjects.Remove(targetObject);
            activeObjects.Add(targetObject);

            return targetObject.gameObject;
        }
        else // And if no objects are avaliable at all, fill the pool and try again.
        {
            InitialisePool();
            return RequestFromPool();
        }
    }

    public void InitialisePool()
    {
        int objectsNeeded = Mathf.Max(count - (pooledObjects.Count + activeObjects.Count), 0); // Calculate how many objects are needed to fill the pool.
        for (int i = 0; i < objectsNeeded; i++)
        {
            GameObject tempObject = Instantiate(prefab.gameObject); // Instantiate a new object of the pool's type.
            DontDestroyOnLoad(tempObject); // Set the object to persist across scenes.

            PoolableObject tempPoolComponent = tempObject.GetComponent<PoolableObject>();
            tempPoolComponent.currentPool = this; // Give the object a reference to this pool.

            pooledObjects.Add(tempPoolComponent); // And add it to the pool list.
            tempObject.SetActive(false);
        }
    }

    // This function iterates backwards through both lists and destroys all objects in the pool.
    public void ClearPool()
    {
        for (int i = pooledObjects.Count - 1; i >= 0; i--)
        {
            Destroy(pooledObjects[i]);
        }

        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            Destroy(activeObjects[i]);
        }
    }

    // This function iterates backwards through the active list and recalls all objects to the pool.
    public void RecallAll()
    {
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            activeObjects[i].gameObject.SetActive(false);
        }
    }

    // This function is called from a poolable object when it is set to inactive, and returns it back to the pool if it exists in the active list.
    public void ReturnToPool(PoolableObject target)
    {
        if (activeObjects.Contains(target))
        {
            target.gameObject.SetActive(false);

            activeObjects.Remove(target);
            pooledObjects.Add(target);
        }
    }

    // This function permenantly removes an object for either list. It is called when an object is deleted to avoid errors.
    public void RemoveFromPool(PoolableObject target)
    {
        if (pooledObjects.Contains(target))
        {
            pooledObjects.Remove(target);
        }
        else if (activeObjects.Contains(target))
        {
            activeObjects.Remove(target);
        }
    }
}