using GameJam.Player;
using UnityEngine;

namespace Assets.Porjectile
{
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField]
        private bool isFollowingTarget = false;

        [SerializeField]
        private float bulletSpeed = 1.0f;

        private int bulletLifetime;
        private int initialLifetime;
        private int bulletDamage;
        private Transform bulletSender;
        private Transform bulletTarget;
        private Vector3 bulletMoveVector;

        private bool isDestroyed = false;

        public void Fire(Transform sender, Transform targetInstance, int damage, int lieftime)
        {
            bulletSender = sender;
            bulletMoveVector = (bulletTarget.position - transform.position).normalized;
            bulletTarget = targetInstance;
            bulletDamage = damage;
            bulletLifetime = lieftime;
            initialLifetime = bulletLifetime;
        }

        public void Parry(Transform newSender)
        {
            bulletTarget = bulletSender;
            bulletSender = newSender;
            bulletMoveVector = (bulletTarget.position - transform.position).normalized;
            bulletLifetime = initialLifetime;
        }

        private void Update()
        {
            if (!isDestroyed && bulletLifetime <= 0)
            {
                ProjectileDie();
                return;
            }

            if (isFollowingTarget)
            {
                bulletMoveVector = (bulletTarget.position - transform.position).normalized;
            }

            transform.position += bulletSpeed * Time.deltaTime * bulletMoveVector;
            bulletLifetime--;
        }

        private void ProjectileDie()
        {
            Destroy(this.gameObject);
            isDestroyed = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Player>(out var player))
            {
                player.GetHit(bulletDamage);
            }
        }
    }
}
