using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.Fsm
{
    public interface IState
    {
        UniTask OnEnter(CancellationToken cancellationToken = default);
        UniTask OnExit(CancellationToken cancellationToken = default);
    }
}