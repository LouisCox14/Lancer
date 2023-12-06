using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PoolableObject : MonoBehaviour
{
    /*
        This abstract class is used to create poolable objects. It handles the logic for returning to the pool, and resetting the object to its default state.
    */

    [HideInInspector] public ObjectPool currentPool; // Used to keep track of the pool the object is from.

    // This function is used to set an object to its default state ready for use, and is called when a new object is requested. Each poolable object will differ slightly here, and so it can be overriden.
    virtual public void Reset() 
    { 
        gameObject.SetActive(true);
    }

    // Whilst active, listen to the scene manager to tell if the scene is changed.
    protected virtual void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // When set to inactive, stop listening to the scene manager, and return to the object pool.
    protected virtual void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        currentPool.ReturnToPool(this);
    }

    // On destroy remove from pool. This should never be triggered, and is only here for error handling.
    protected virtual void OnDestroy()
    {
        currentPool.RemoveFromPool(this);
    }

    // If the scene an object is in is unloaded, deactivate. Deactivating will automatically return to the object to the pool.
    protected virtual void OnSceneUnloaded(Scene current)
    {
        gameObject.SetActive(false);
    }
}