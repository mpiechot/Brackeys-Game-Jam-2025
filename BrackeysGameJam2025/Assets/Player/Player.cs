using Assets.Porjectile;
using System.Linq;
using UnityEngine;

namespace GameJam.Player
{
    public class Player : MonoBehaviour, IUnit
    {
        [SerializeField]
        private float parryRange = 1f;

        [SerializeField]
        private float parryCooldown = 1f;

        private bool canParry = true;
        private bool isParrying = false;
        private float cooldown = 0.0f;
        private float parryTime = 0.2f;

        public void GetHit(int damage)
        {
            throw new System.NotImplementedException();
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
