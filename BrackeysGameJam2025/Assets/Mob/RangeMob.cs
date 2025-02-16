#nullable enable

using GameJam.Mob;
using UnityEngine;

public class RangeMob : MobBase
{

    protected override void HandleTargetResult(GameObject? target, TargetAction action)
    {
        if (target) Agent.SetDestination(target.transform.position);

        var performedAction = action switch
        {
            TargetAction.Attack => "Bogenschuss",
            _ => "Ungültig"
        };
    }
}
