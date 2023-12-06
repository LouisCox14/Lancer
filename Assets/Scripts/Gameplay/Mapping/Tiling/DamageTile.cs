using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTile : GameplayTileBase
{
    [SerializeField] Damage damage;

    public override void OnEntityEnter(Entity target) { target.TakeDamage(this, damage); base.OnEntityEnter(target); }
    public override void OnEntityLeave(Entity target) { base.OnEntityLeave(target); }
    public override void OnStartTurn() { }
    public override void OnEndTurn() { standingEntity.TakeDamage(this, damage); }
    public override void OnStartRound() { }
}
