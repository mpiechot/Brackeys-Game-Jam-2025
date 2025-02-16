using GameJam.Mob;
using GameJam.Player;
using GameJam.Porjectile;
using System.Linq;
using UnityEngine;

namespace Assets.Player.Actions
{
    public class ParryAction : PlayerActionBase
    {
        private readonly Transform caller;
        private readonly PlayerMovement playerMovement;
        private readonly PlayerVisualsController playerVisuals;

        public ParryAction(Transform caller, PlayerMovement playerMovement, PlayerVisualsController playerVisuals)
        {
            this.caller = caller;
            this.playerMovement = playerMovement;
            this.playerVisuals = playerVisuals;
        }

        public float Range { get; set; }

        protected override void PerformAction()
        {
            var objectsInParryRange = Physics2D.OverlapCircleAll(caller.position, Range);

            // Parry all bullets, which are targeting the player
            var bulletsInRange = objectsInParryRange.Select(obj => obj.GetComponent<ProjectileBase>()).Where(projectile => projectile != null && projectile.Target.gameObject == caller.gameObject);

            foreach (var bullet in bulletsInRange)
            {
                bullet.Parry(caller);
            }

            // Parry all close-combat, which are targeting the player
            var enemiesInRange = objectsInParryRange.Select(obj => obj.GetComponent<MobBase>()).Where(mob => mob != null && mob.Target == caller.gameObject && mob.IsAttacking);
            foreach (var enemy in enemiesInRange)
            {
                enemy.GetParried();
            }

            playerMovement.EnableMovement();
            playerVisuals.ShowNormalState();
        }

        protected override void PerformAfterAction()
        {
            // Nothing to do
        }

        protected override void PerformBeforeAction()
        {
            playerMovement.DisableMovement();
            playerVisuals.ShowParryState();
        }
    }
}
