using UnityEngine;

namespace GameJam.Mob
{
    [CreateAssetMenu(fileName = "MobStats", menuName = "GameJam/Mob Stats")]
    public sealed class MobStats : ScriptableObject
    {
        [field: SerializeField]
        public int MaxHealth { get; private set; } = 5;

        [field: SerializeField]
        public int AttackDamage { get; private set; } = 5;

        [field: SerializeField]
        public float AttackTimeSeconds { get; private set; } = 1;

        [field: SerializeField]
        public float AttackRange { get; private set; } = 2;

        [field: SerializeField]
        public int AttackCooldownSeconds { get; private set; } = 1;

        [field: SerializeField]
        public float TargetingRange { get; private set; } = 5;

        [field: SerializeField]
        public MobType MobType { get; private set; } = MobType.None;

        [field: SerializeField]
        public float FollowRange { get; private set; } = 2f;
    }
}
