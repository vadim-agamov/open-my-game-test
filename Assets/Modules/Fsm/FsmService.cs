using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.Fsm
{
    public class Fsm
    {
        private IState State { get; set; }
        private static Fsm _instance;
        private static Fsm Instance => _instance ??= new Fsm();

        public static async UniTask Enter(IState state, CancellationToken cancellationToken)
        {
            if (Instance.State != null)
            {
                Debug.Log($"[{nameof(Fsm)}] Exit {Instance.State}");
                await Instance.State.OnExit(cancellationToken);
            }

            Instance.State = state;
            Debug.Log($"[{nameof(Fsm)}] Enter {Instance.State}");
            await Instance.State.OnEnter(cancellationToken);
        }
    }
}