using System.Threading;
using Common.Configs;
using Cysharp.Threading.Tasks;
using Meta.MetaScreen;
using Modules.Fsm;
using Modules.UIService;
using UnityEngine.SceneManagement;

namespace Core
{
    public class CoreState : IState
    {
        private CoreScreenPresenter _coreScreenPresenter;
        private IUIService UIService { get; } = Modules.DiContainer.Container.Resolve<IUIService>();
        public CoreState(PuzzleInfoConfig puzzleInfoConfig)
        {
        }

        async UniTask IState.OnEnter(CancellationToken token)
        {
            await SceneManager.LoadSceneAsync("Scenes/Core").ToUniTask(cancellationToken: token);

            _coreScreenPresenter = await UIService.Create<CoreScreenModel, CoreScreenView, CoreScreenPresenter>(new CoreScreenModel(), CoreScreenPresenter.KEY, token);
            await _coreScreenPresenter.Show(token);
        }

        UniTask IState.OnExit(CancellationToken cancellationToken)
        {
            UIService.Release(_coreScreenPresenter);
            return UniTask.CompletedTask;
        }
    }
}