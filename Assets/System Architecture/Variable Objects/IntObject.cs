using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable Objects/Int")]
public class IntObject : VariableObject { public int value; public override object GetValue() { return value; }}
