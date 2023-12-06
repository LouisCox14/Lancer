using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable Objects/Component")]
public class ComponentObject : VariableObject { public Component value; public override object GetValue() { return value; }}