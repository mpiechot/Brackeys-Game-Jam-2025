using UnityEngine;

namespace GameJam.Mob
{
    public struct TargetResult
    {
        public GameObject Target { get; }

        public TargetAction Action { get; }

        public bool CanOverride { get; }

        public TargetResult(GameObject target, TargetAction action, bool canOverride)
        {
            Target = target;
            Action = action;
            CanOverride = canOverride;
        }
    }
}
