using GameJam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class UnitsController : MonoBehaviour
    {
        private List<IUnit> enemies = new List<IUnit>();

        private List<IUnit> allies = new List<IUnit>();

        public IEnumerable<IUnit> PlayerEnemies => enemies;

        public IEnumerable<IUnit> PlayerAllies => allies;

        public void RegisterUnit(IUnit unit, bool enemy)
        {
            if (enemy && !enemies.Contains(unit))
            {
                enemies.Add(unit);
            }
            else if (!enemy && !allies.Contains(unit))
            {
                allies.Add(unit);
            }
        }

        public void UnregisterUnit(IUnit unit, bool enemy)
        {
            if (enemy && enemies.Contains(unit))
            {
                enemies.Remove(unit);
            }
            else if (!enemy && allies.Contains(unit))
            {
                allies.Remove(unit);
            }
        }
    }
}
