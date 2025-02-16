using Assets.Porjectile;
using GameJam.Exceptions;
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

        private bool canParry = true;
        private bool isParrying = false;
        private float cooldown = 0.0f;
        private float parryTime = 0.2f;

        private PlayerHealth PlayerHealth => SerializeFieldNotAssignedException.ThrowIfNull(playerHealth);

        public void GetHit(int damage)
        {
            PlayerHealth.ApplyDamage(damage);
        }

        public void Parry()
        {
            var bulletsInRange = Physics2D.OverlapCircleAll(transform.position, parryRange).OfType<ProjectileBase>();

            foreach (var bullet in bulletsInRange)
            {
                bullet.Parry(transform);
            }
        }
    }
}
