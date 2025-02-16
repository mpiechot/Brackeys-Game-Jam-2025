using NUnit.Framework;
using UnityEngine;

namespace GameJam.Mob
{
    public interface ITargetProvider
    {
        /// <summary>
        ///     Gets a target
        /// </summary>
        /// <returns></returns>
        TargetResult GetTarget(MobBase targetSearcher);
    }
}
