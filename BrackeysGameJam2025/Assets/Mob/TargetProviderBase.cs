using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Mob
{
    public abstract class TargetProviderBase : ITargetProvider
    {
        public TargetProviderBase(GameObject[] allies, GameObject[] enemies)
        {
            Allies = allies;
            Enemies = enemies;
        }

        protected IReadOnlyList<GameObject> Allies { get; }

        protected IReadOnlyList<GameObject> Enemies { get; }

        public abstract TargetResult GetTarget(Mob targetSearcher);
    }
}
