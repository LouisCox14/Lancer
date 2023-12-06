using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSetObject : MonoBehaviour
{
    /*
        This component is used for objects to subscribe to RuntimeSets.
    */

    public RuntimeSet[] sets; // This array keeps track of the sets the object will subscribe to.

    // Registers to chosen RuntimeSets when activated.
    void OnEnable()
    {
        foreach (RuntimeSet set in sets)
        {
            set.RegisterSetObject(gameObject);
        }
    }

    // Registers to chosen RuntimeSets when deactivated to avoid errors.
    void OnDisable()
    {
        foreach (RuntimeSet set in sets)
        {
            set.UnregisterSetObject(gameObject);
        }
    }
}
