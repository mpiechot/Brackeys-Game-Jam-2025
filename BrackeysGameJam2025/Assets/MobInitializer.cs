using GameJam.Mob;
using System.Linq;
using UnityEngine;

public class MobInitializer : MonoBehaviour
{
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var allies = GameObject.FindGameObjectsWithTag("EMob");
        var enemies = GameObject.FindGameObjectsWithTag("PMob");
        foreach (var mob in GameObject.FindGameObjectsWithTag("EMob"))
        {
            mob.GetComponent<Mob>().Initialize(new EnemyMobTargetProvider(player, allies, enemies));
        }
    }
}
