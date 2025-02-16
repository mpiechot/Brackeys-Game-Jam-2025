#nullable enable

using GameJam.Exceptions;
using GameJam.Mob;
using GameJam.Porjectile;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GameJam.Player
{
    [RequireComponent(typeof(PlayerHealth), typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour, IUnit
    {
        [SerializeField]
        private float parryRange = 1f;

        [SerializeField]
        private float parryCooldown = 1f;

        [SerializeField]
        private PlayerHealth? playerHealth;

        [SerializeField]
        private PlayerMovement? playerMovement;

        [SerializeField]
        private PlayerVisualsController? playerVisualsController;

        private bool canParry = true;
        private float parryTime = 0.2f;

        public GameObject Unit => gameObject;

        private PlayerHealth PlayerHealth => SerializeFieldNotAssignedException.ThrowIfNull(playerHealth);

        private PlayerMovement PlayerMovement => SerializeFieldNotAssignedException.ThrowIfNull(playerMovement);

        private PlayerVisualsController PlayerVisualsController => SerializeFieldNotAssignedException.ThrowIfNull(playerVisualsController);

        public void GetHit(int damage)
        {
            PlayerHealth.ApplyDamage(damage);
        }

        public void Parry()
        {
            if (canParry)
            {
                StartCoroutine(ExecuteParry());
            }
        }

        private IEnumerator ExecuteParry()
        {
            canParry = false;
            PlayerMovement.DisableMovement();

            var objectsInParryRange = Physics2D.OverlapCircleAll(transform.position, parryRange);

            // Parry all bullets, which are targeting the player
            var bulletsInRange = objectsInParryRange.Select(obj => obj.GetComponent<ProjectileBase>()).Where(projectile => projectile != null && projectile.Target.gameObject == gameObject);

            foreach (var bullet in bulletsInRange)
            {
                bullet.Parry(transform);
            }

            // Parry all close-combat, which are targeting the player
            var enemiesInRange = objectsInParryRange.Select(obj => obj.GetComponent<MobBase>()).Where(mob => mob != null && mob.Target == gameObject && mob.IsAttacking);
            foreach (var enemy in enemiesInRange)
            {
                enemy.GetParried();
            }


            PlayerVisualsController.ShowParryState();

            yield return new WaitForSeconds(parryTime);
            PlayerMovement.EnableMovement();
            PlayerVisualsController.ShowNormalState();
            yield return new WaitForSeconds(parryCooldown);
            canParry = true;
        }
    }
}
