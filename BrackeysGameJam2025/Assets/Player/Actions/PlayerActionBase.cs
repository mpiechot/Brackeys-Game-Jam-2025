using Cysharp.Threading.Tasks;
using GameJam.Mob;
using GameJam.Player;
using GameJam.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Player.Actions
{
    public abstract class PlayerActionBase : IDisposable
    {
        public bool CanInvoke { get; private set; } = true;

        public float Cooldown { get; set; } = 1f;

        public float Time { get; set; } = 0.2f;

        private CancellableTaskCollection taskCollection = new();


        public void Invoke()
        {
            if (CanInvoke)
            {
                taskCollection.StartExecution(ExecuteInvokeAsync);
            }
        }

        public async UniTask ExecuteInvokeAsync(CancellationToken cancellationToken)
        {
            CanInvoke = false;
            PerformBeforeAction();
            await UniTask.Delay((int)(1000 * Time));
            PerformAction();
            await UniTask.Delay((int)(1000 * Cooldown));
            PerformAfterAction();
            CanInvoke = true;
        }

        protected abstract void PerformBeforeAction();

        protected abstract void PerformAfterAction();

        protected abstract void PerformAction();

        public void Dispose()
        {
            taskCollection.Dispose();
        }
    }
}
