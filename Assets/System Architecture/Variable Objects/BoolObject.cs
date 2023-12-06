using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable Objects/Bool")]
public class BoolObject : VariableObject { public bool value; public override object GetValue() { return value; }}