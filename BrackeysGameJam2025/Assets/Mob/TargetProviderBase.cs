using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Mob
{
    public abstract class TargetProviderBase : ITargetProvider
    {
        public TargetProviderBase(GameObject player, GameObject[] allies, GameObject[] enemies)
        {
            Player = player;
            Allies = allies;
            Enemies = enemies;
        }

        protected GameObject Player { get; }

        protected IReadOnlyList<GameObject> Allies { get; }

        protected IReadOnlyList<GameObject> Enemies { get; }

        public abstract TargetResult GetTarget(Mob targetSearcher);
    }
}
