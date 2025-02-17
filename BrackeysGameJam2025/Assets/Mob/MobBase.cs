#nullable enable

using Assets;
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
        private UnitsController unitsController;
        private bool canAttack = true;

        private Coroutine? attacking;

        public bool IsAttacking { get; private set; }

        public bool IsDead => currentHealth <= 0;

        public GameObject? Target { get; protected set; }

        public GameObject Unit => gameObject;

        public ITargetProvider? TargetProvider
        {
            get => targetProvider;
            set
            {
                targetProvider = value;
            }
        }

        public bool IsEnemyMob { get; private set; } = true;

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
            unitsController = FindAnyObjectByType<UnitsController>();
            unitsController.RegisterUnit(this, IsEnemyMob);
            Initialize(new EnemyMobTargetProvider());
        }

        public void GetHit(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
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

                TargetResult targetResult = default(TargetResult);
                if (IsEnemyMob)
                {
                    targetResult = targetProvider.GetTarget(this, unitsController.PlayerEnemies, unitsController.PlayerAllies);
                }
                else
                {
                    targetResult = targetProvider.GetTarget(this, unitsController.PlayerAllies, unitsController.PlayerEnemies);
                }

                HandleTargetResult(targetResult);

                CurrentCooldownInMilliSec = Mathf.Max(0, CurrentCooldownInMilliSec - 1);
                await UniTask.NextFrame(cancellationToken);
            }
        }

        private void Update()
        {
            var direction = Agent.destination.x - transform.position.x;
            visualAnimator.SetFloat("XDirection", direction);
        }

        protected virtual void HandleTargetResult(TargetResult targetResult)
        {
            Target = targetResult.Target;
            if (Target)
            {
                Vector3 direction = (Target.transform.position - transform.position).normalized;
                float stopDistance = targetResult.TargetDistance; // Abstand, den der Agent vor dem Spieler halten soll

                Vector3 targetPosition = Target.transform.position - (direction * stopDistance);
                Agent.SetDestination(targetPosition);
            }

            if (targetResult.Action == TargetAction.Attack && CurrentCooldownInMilliSec == 0)
            {
                Attack(Target);
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
            Agent.isStopped = true;
            unitsController.UnregisterUnit(this, IsEnemyMob);
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

        public void Arise()
        {
            var player = FindAnyObjectByType<PlayerController>().gameObject;
            IsEnemyMob = false;
            Agent.isStopped = false;
            currentHealth = Stats.MaxHealth;
            targetProvider = new PlayerMobTargetProvider();
            unitsController.RegisterUnit(this, IsEnemyMob);
            taskCollection.CancelExecution();
            taskCollection.StartExecution(MobBrainAsync);
        }
    }
}
