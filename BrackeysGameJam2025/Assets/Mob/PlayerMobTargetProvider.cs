using GameJam.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GameJam.Mob
{
    public class PlayerMobTargetProvider : ITargetProvider
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

            foreach (var ally in allies)
            {
                if (Vector2.Distance(ally.Unit.transform.position, targetSearcher.transform.position) < targetSearcher.Stats.TargetingRange)
                {
                    return new TargetResult(ally.Unit, TargetAction.Follow, true);
                }
            }

            return new TargetResult(targetSearcher.gameObject, TargetAction.None, true);
        }
    }
}
