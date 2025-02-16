#nullable enable

using Cysharp.Threading.Tasks;
using GameJam.Exceptions;
using GameJam.Player;
using GameJam.Util;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace GameJam.Mob
{
    public class MobBase : MonoBehaviour, IUnit
    {
        [SerializeField]
        private MobStats? mobStats;

        [SerializeField]
        private NavMeshAgent? agent;

        [SerializeField]
        private Animator visualAnimator;

        private CancellableTaskCollection taskCollection = new();

        private ITargetProvider? targetProvider;

        protected int currentHealth;

        private bool canAttack = true;

        private Coroutine? attacking;

        public bool IsAttacking { get; private set; }

        public bool IsDead => currentHealth <= 0;

        public GameObject? Target { get; private set; }

        public GameObject Unit => gameObject;

        public ITargetProvider? TargetProvider
        {
            get => targetProvider;
            set
            {
                targetProvider = value;
            }
        }
        protected int CurrentCooldownInMilliSec { get; private set; } = 0;

        public void Initialize(ITargetProvider? targetProviderInstance)
        {
            targetProvider = targetProviderInstance;
            taskCollection.CancelExecution();
            taskCollection.StartExecution(MobBrainAsync);
        }

        public MobStats Stats => SerializeFieldNotAssignedException.ThrowIfNull(mobStats, nameof(mobStats));

        protected NavMeshAgent Agent => SerializeFieldNotAssignedException.ThrowIfNull(agent, nameof(agent));

        private void Start()
        {
            visualAnimator.StartPlayback();
            currentHealth = Stats.MaxHealth;
            var player = FindAnyObjectByType<PlayerController>().gameObject;
            Initialize(new EnemyMobTargetProvider(Array.Empty<GameObject>(), new GameObject[] { player }));
        }

        public void GetHit(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                Die();
            }
        }


        private async UniTask MobBrainAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (targetProvider == null)
                {
                    await UniTask.Delay(100, cancellationToken: cancellationToken);
                    CurrentCooldownInMilliSec = Mathf.Max(0, CurrentCooldownInMilliSec - 100);
                    continue;
                }

                var targetResult = targetProvider.GetTarget(this);

                HandleTargetResult(targetResult.Target, targetResult.Action);

                CurrentCooldownInMilliSec = Mathf.Max(0, CurrentCooldownInMilliSec - 1);
                await UniTask.NextFrame(cancellationToken);
            }
        }

        private void Update()
        {
            var direction = Agent.destination.x - transform.position.x;
            visualAnimator.SetFloat("XDirection", direction);
        }

        protected virtual void HandleTargetResult(GameObject? target, TargetAction action)
        {
            Target = target;
            if (target)
            {
                Agent.SetDestination(target.transform.position);
            }

            if (action == TargetAction.Attack && CurrentCooldownInMilliSec == 0)
            {
                Attack(target);
            }
        }

        private void Attack(GameObject? target)
        {
            if (!canAttack || target == null)
            {
                return;
            }

            if (target.TryGetComponent<IUnit>(out var unitTarget))
            {
                attacking = StartCoroutine(PerformAttack(unitTarget));
            }
        }

        private IEnumerator PerformAttack(IUnit attackTarget)
        {
            canAttack = false;
            IsAttacking = true;
            Agent.isStopped = true;
            yield return new WaitForSeconds(Stats.AttackTimeSeconds);
            if (Vector2.Distance(attackTarget.Unit.transform.position, transform.position) < Stats.AttackRange)
            {
                attackTarget.GetHit(Stats.AttackDamage);
            }
            Agent.isStopped = false;
            IsAttacking = false;
            yield return new WaitForSeconds(Stats.AttackCooldownSeconds);
            canAttack = true;
        }

        private IEnumerator AttackCooldown()
        {
            canAttack = false;
            Agent.isStopped = false;
            yield return new WaitForSeconds(Stats.AttackCooldownSeconds);
            canAttack = true;
        }

        private void Die()
        {
            // Make this unit arisable

            taskCollection.CancelExecution();
        }

        private void OnDestroy()
        {
            taskCollection.Dispose();
        }

        public virtual void GetParried()
        {
            if (attacking != null)
            {
                StopCoroutine(attacking);
                StartCoroutine(AttackCooldown());
                GetHit(Stats.AttackDamage);
            }
        }

        internal void Arise()
        {
            throw new NotImplementedException();
        }
    }
}
