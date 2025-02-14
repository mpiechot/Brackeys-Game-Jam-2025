using System.Linq;
using UnityEngine;

namespace GameJam.Mob
{
    public class EnemyMobTargetProvider : TargetProviderBase
    {
        public EnemyMobTargetProvider(GameObject player, GameObject[] allies, GameObject[] enemies) : base(player, allies, enemies)
        {
        }

        public override TargetResult GetTarget(Mob targetSearcher)
        {
            var enemyNearBy = Physics.OverlapSphere(targetSearcher.transform.position, targetSearcher.Stats.AttackRange)
                .FirstOrDefault(mob => Enemies.Any(enemy => enemy == mob));

            if (enemyNearBy != null)
            {
                return new TargetResult(enemyNearBy.gameObject, TargetAction.Attack, false);
            }

            var nextTarget = Physics.OverlapSphere(targetSearcher.transform.position, targetSearcher.Stats.TargetingRange)
                .FirstOrDefault(mob => Enemies.Any(enemy => enemy == mob));

            if (nextTarget != null)
            {
                return new TargetResult(enemyNearBy.gameObject, TargetAction.Follow, false);
            }

            if (Vector2.Distance(targetSearcher.transform.position, Player.transform.position) < targetSearcher.Stats.AttackRange)
            {
                return new TargetResult(Player, TargetAction.Attack, true);
            }

            return new TargetResult(Player, TargetAction.Follow, true);
        }
    }
}
