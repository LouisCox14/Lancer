using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class VariableObject : ScriptableObject
{
    /*
        Variable objects are used to share information between scripts. For example, both the Healthbar and Player scripts could need access to playerHealth, and so this could act as a ponter to a shared variable.
    */

    public virtual object GetValue() { return null; }
}