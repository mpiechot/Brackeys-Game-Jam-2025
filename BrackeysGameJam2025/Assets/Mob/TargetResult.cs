using UnityEngine;

namespace GameJam.Mob
{
    public struct TargetResult
    {
        public GameObject Target { get; }

        public TargetAction Action { get; }

        public float TargetDistance { get; }

        public bool CanOverride { get; }

        public TargetResult(GameObject target, TargetAction action, float targetDistance, bool canOverride)
        {
            Target = target;
            Action = action;
            TargetDistance = targetDistance;
            CanOverride = canOverride;
        }
    }
}
