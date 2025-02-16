using GameJam.Mob;
using System.Linq;
using UnityEngine;

public class MobInitializer : MonoBehaviour
{
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var allies = GameObject.FindGameObjectsWithTag("EMob").Append(player).ToArray();
        var enemies = GameObject.FindGameObjectsWithTag("PMob");
        foreach (var mob in GameObject.FindGameObjectsWithTag("EMob"))
        {
            mob.GetComponent<MobBase>().Initialize(new EnemyMobTargetProvider(allies, enemies));
        }
    }
}
