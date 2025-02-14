using System.Linq;
using UnityEngine;

namespace GameJam.Mob
{
    public class PlayerMobTargetProvider : TargetProviderBase
    {
        public PlayerMobTargetProvider(GameObject player, GameObject[] allies, GameObject[] enemies) : base(player, allies, enemies)
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

            return new TargetResult(Player, TargetAction.Follow, true);
        }
    }
}
