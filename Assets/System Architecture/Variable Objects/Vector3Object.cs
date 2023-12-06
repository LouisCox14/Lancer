using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable Objects/Vector3")]
public class Vector3Object : VariableObject { public Vector3 value; public override object GetValue() { return value; }}