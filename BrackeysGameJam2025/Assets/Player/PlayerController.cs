#nullable enable

using Assets;
using Assets.Player.Actions;
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
        private PlayerHealth? playerHealth;

        [SerializeField]
        private PlayerMovement? playerMovement;

        [SerializeField]
        private PlayerVisualsController? playerVisualsController;

        private ParryAction parryAction;
        private AriseAction ariseAction;

        [SerializeField]
        private PlayerActionStats parryStats = new();

        [SerializeField]
        private PlayerActionStats ariseStats = new();

        public GameObject Unit => gameObject;

        private PlayerHealth PlayerHealth => SerializeFieldNotAssignedException.ThrowIfNull(playerHealth);

        private PlayerMovement PlayerMovement => SerializeFieldNotAssignedException.ThrowIfNull(playerMovement);

        private PlayerVisualsController PlayerVisualsController => SerializeFieldNotAssignedException.ThrowIfNull(playerVisualsController);

        private void Start()
        {
            parryAction = new ParryAction(transform, PlayerMovement, PlayerVisualsController);
            ariseAction = new AriseAction(transform, PlayerMovement, PlayerVisualsController);

            var unitsController = FindAnyObjectByType<UnitsController>();
            unitsController.RegisterUnit(this, false);

            UpdateActions();
        }

        private void OnValidate()
        {
            UpdateActions();
        }

        private void UpdateActions()
        {
            if (!Application.isPlaying || parryAction == null || ariseAction == null)
            {
                // Don't care
                return;
            }

            parryAction.Cooldown = parryStats.Cooldown;
            parryAction.Time = parryStats.Time;
            parryAction.Range = parryStats.Range;

            ariseAction.Cooldown = ariseStats.Cooldown;
            ariseAction.Time = ariseStats.Time;
            ariseAction.Range = ariseStats.Range;
        }

        public void GetHit(int damage)
        {
            PlayerHealth.ApplyDamage(damage);
        }

        public void Parry()
        {
            if (parryAction.CanInvoke)
            {
                parryAction.Invoke();
            }
        }

        public void Arise()
        {
            if (ariseAction.CanInvoke)
            {
                ariseAction.Invoke();
            }
        }
    }

    [Serializable]
    public struct PlayerActionStats
    {
        [SerializeField]
        public float Time;

        [SerializeField]
        public float Cooldown;

        [SerializeField]
        public float Range;
    }
}
