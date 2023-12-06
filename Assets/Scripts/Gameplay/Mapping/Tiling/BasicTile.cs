using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile : GameplayTileBase
{
    public override void OnEntityEnter(Entity target) { base.OnEntityEnter(target); }
    public override void OnEntityLeave(Entity target) { base.OnEntityLeave(target); }
    public override void OnStartTurn() { }
    public override void OnEndTurn() { }
    public override void OnStartRound() { }
}
