using GameJam.Player;
using System.Linq;
using UnityEngine;

namespace GameJam.Mob
{
    public class EnemyMobTargetProvider : TargetProviderBase
    {
        public EnemyMobTargetProvider(GameObject[] allies, GameObject[] enemies) : base(allies, enemies)
        {
        }

        public override TargetResult GetTarget(MobBase targetSearcher)
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

            // Find Player
            //var player = Enemies.OfType<PlayerController>().FirstOrDefault();
            var player = Enemies.Select(e => e.GetComponent<PlayerController>()).FirstOrDefault(p => p != null);

            if (player == null)
            {
                return new TargetResult(targetSearcher.gameObject, TargetAction.None, true);
            }

            if (Vector2.Distance(targetSearcher.transform.position, player.transform.position) < targetSearcher.Stats.AttackRange)
            {
                return new TargetResult(player.gameObject, TargetAction.Attack, true);
            }

            return new TargetResult(player.gameObject, TargetAction.Follow, true);
        }
    }
}
