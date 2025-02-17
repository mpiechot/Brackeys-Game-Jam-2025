#nullable enable

using GameJam.Mob;
using UnityEngine;

public class RangeMob : MobBase
{

    protected override void HandleTargetResult(TargetResult targetResult)
    {
        Target = targetResult.Target;
        if (Target)
        {
            Agent.SetDestination(Target.transform.position);
        }

        var performedAction = targetResult.Action switch
        {
            TargetAction.Attack => "Bogenschuss",
            _ => "Ungültig"
        };
    }
}
