using GameJam.Player;
using UnityEngine;

namespace GameJam.Porjectile
{
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField]
        private bool isFollowingTarget = false;

        [SerializeField]
        private float speed = 1.0f;

        private int lifetime;
        private int initialLifetime;
        private int damage;
        private Transform sender;
        private Transform target;
        private Vector3 moveVector;

        private bool isDestroyed = false;

        public Transform Target => target;

        public void Fire(Transform sender, Transform targetInstance, int damage, int lieftime)
        {
            this.sender = sender;
            moveVector = (target.position - transform.position).normalized;
            target = targetInstance;
            this.damage = damage;
            lifetime = lieftime;
            initialLifetime = lifetime;
        }

        public void Parry(Transform newSender)
        {
            target = sender;
            sender = newSender;
            moveVector = (target.position - transform.position).normalized;
            lifetime = initialLifetime;
        }

        private void Update()
        {
            if (!isDestroyed && lifetime <= 0)
            {
                ProjectileDie();
                return;
            }

            if (isFollowingTarget)
            {
                moveVector = (target.position - transform.position).normalized;
            }

            transform.position += speed * Time.deltaTime * moveVector;
            lifetime--;
        }

        private void ProjectileDie()
        {
            Destroy(this.gameObject);
            isDestroyed = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                player.GetHit(damage);
            }
        }
    }
}
