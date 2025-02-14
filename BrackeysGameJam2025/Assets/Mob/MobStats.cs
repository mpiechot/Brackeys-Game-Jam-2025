using UnityEngine;

namespace GameJam.Mob
{
    [CreateAssetMenu(fileName = "MobStats", menuName = "GameJam/Mob Stats")]
    public sealed class MobStats : ScriptableObject
    {
        [field: SerializeField]
        public int MaxHealth { get; private set; }

        [field: SerializeField]
        public float AttackRange { get; private set; }

        [field: SerializeField]
        public float TargetingRange { get; private set; }

        [field: SerializeField]
        public MobType MobType { get; private set; }

        [field: SerializeField]
        public int AttackCooldownInMilliSecs { get; private set; }

        [field: SerializeField]
        public int AttackDamage { get; private set; }
    }
}
