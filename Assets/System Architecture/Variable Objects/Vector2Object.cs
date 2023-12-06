using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable Objects/Vector2")]
public class Vector2Object : VariableObject { public Vector2 value; public override object GetValue() { return value; }}