using GameJam.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJam.Mob
{
    public class EnemyMobTargetProvider : ITargetProvider
    {
        public TargetResult GetTarget(MobBase targetSearcher, IEnumerable<IUnit> allies, IEnumerable<IUnit> enemies)
        {
            foreach (var target in enemies)
            {
                if (Vector2.Distance(target.Unit.transform.position, targetSearcher.transform.position) < targetSearcher.Stats.AttackRange)
                {
                    return new TargetResult(target.Unit, TargetAction.Attack, false);
                }

                if (Vector2.Distance(target.Unit.transform.position, targetSearcher.transform.position) < targetSearcher.Stats.TargetingRange)
                {
                    return new TargetResult(target.Unit, TargetAction.Follow, false);
                }
            }

            return new TargetResult(targetSearcher.gameObject, TargetAction.None, true);
        }
    }
}
