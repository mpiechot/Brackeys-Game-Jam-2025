﻿#nullable enable

using Cysharp.Threading.Tasks;
using GameJam.Exceptions;
using GameJam.Player;
using GameJam.Util;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace GameJam.Mob
{
    public class Mob : MonoBehaviour, IUnit
    {
        [SerializeField]
        private MobStats? mobStats;

        [SerializeField]
        private NavMeshAgent? agent;

        private CancellableTaskCollection taskCollection = new();

        private GameObject? target;

        private ITargetProvider? targetProvider;

        private int currentCooldownInMilliSec = 0;

        private int currentHealth;

        public ITargetProvider? TargetProvider
        {
            get => targetProvider;
            set
            {
                targetProvider = value;
            }
        }

        public void Initialize(ITargetProvider? targetProviderInstance)
        {
            targetProvider = targetProviderInstance;
            taskCollection.CancelExecution();
            taskCollection.StartExecution(MobBrainAsync);
        }

        public MobStats Stats => SerializeFieldNotAssignedException.ThrowIfNull(mobStats, nameof(mobStats));

        private NavMeshAgent Agent => SerializeFieldNotAssignedException.ThrowIfNull(agent, nameof(agent));

        private void Start()
        {
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
                    currentCooldownInMilliSec = Mathf.Max(0, currentCooldownInMilliSec - 100);
                    continue;
                }

                var targetResult = targetProvider.GetTarget(this);
                target = targetResult.Target;

                if(target)
                {
                    HandleTargetResult(targetResult.Action);
                }

                currentCooldownInMilliSec = Mathf.Max(0, currentCooldownInMilliSec - 1);
                await UniTask.NextFrame(cancellationToken);
            }
        }

        protected virtual void HandleTargetResult(TargetAction action)
        {
            Agent.SetDestination(target!.transform.position);
            if (action == TargetAction.Attack && currentCooldownInMilliSec == 0)
            {
                Attack();
            }
        }

        private void Attack()
        {
            if (target == null)
            {
                return;
            }

            currentCooldownInMilliSec = Stats.AttackCooldownInMilliSecs;

            if (target.TryGetComponent<IUnit>(out var unitTarget))
            {
                // Perform the attack
                unitTarget.GetHit(Stats.AttackDamage);
            }
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
    }
}
