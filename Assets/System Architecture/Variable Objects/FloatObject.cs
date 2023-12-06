using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable Objects/Float")]
public class FloatObject : VariableObject { public float value; public override object GetValue() { return value; }}