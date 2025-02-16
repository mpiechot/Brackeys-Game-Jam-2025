using GameJam.Mob;
using GameJam.Player;
using System.Linq;
using UnityEngine;

namespace Assets.Player.Actions
{
    public class AriseAction : PlayerActionBase
    {
        private readonly Transform caller;
        private readonly PlayerMovement playerMovement;
        private readonly PlayerVisualsController playerVisuals;

        public AriseAction(Transform caller, PlayerMovement playerMovement, PlayerVisualsController playerVisuals)
        {
            this.caller = caller;
            this.playerMovement = playerMovement;
            this.playerVisuals = playerVisuals;
        }

        public float Range { get; set; }

        protected override void PerformAction()
        {
            playerMovement.DisableMovement();

            var objectsInParryRange = Physics2D.OverlapCircleAll(caller.position, Range);

            // Parry all close-combat, which are targeting the player
            var enemiesInRange = objectsInParryRange.Select(obj => obj.GetComponent<MobBase>()).Where(mob => mob != null && mob.IsDead);
            foreach (var enemy in enemiesInRange)
            {
                enemy.Arise();
            }


            playerVisuals.ShowParryState();
        }

        protected override void PerformAfterAction()
        {
            playerMovement.EnableMovement();
            playerVisuals.ShowNormalState();
        }

        protected override void PerformBeforeAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
