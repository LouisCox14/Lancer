using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ComponentObjectSetter : MonoBehaviour
{
    [SerializeField] private Component component;
    [SerializeField] private ComponentObject componentObject;

    void Awake()
    {
        componentObject.value = component;
    }
}
