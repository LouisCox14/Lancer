using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    /*
        This class is used to add a "Trigger Event" button to all GameEvents for debugging purposes.
    */
    
    public override void OnInspectorGUI()
    {
        GameEvent gameEvent = (GameEvent)target;
        
        if (GUILayout.Button("Trigger Event"))
        {
            gameEvent.Raise(null, null);
        }
    }
}
