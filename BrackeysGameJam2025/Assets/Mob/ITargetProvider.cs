using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Mob
{
    public interface ITargetProvider
    {
        /// <summary>
        ///     Gets a target
        /// </summary>
        /// <returns></returns>
        TargetResult GetTarget(MobBase targetSearcher, IEnumerable<IUnit> allies, IEnumerable<IUnit> enemies);
    }
}
