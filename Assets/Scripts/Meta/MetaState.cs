using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.MetaScreen;
using Modules.Fsm;
using Modules.UIService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Meta
{
    public class MetaState : IState
    {
        private MetaScreenPresenter _metaScreenPresenter;
        private IUIService UIService { get; } = Modules.DiContainer.Container.Resolve<IUIService>();

        async UniTask IState.OnEnter(CancellationToken token)
        {
            await SceneManager.LoadSceneAsync("Scenes/Meta").ToUniTask(cancellationToken: token);

            _metaScreenPresenter = await UIService.Create<MetaScreenModel, MetaScreenView, MetaScreenPresenter>(new MetaScreenModel(), MetaScreenPresenter.KEY, token);
            await _metaScreenPresenter.Show(token);
        } 

        async UniTask IState.OnExit(CancellationToken token)
        {
            await _metaScreenPresenter.Hide(token);
            UIService.Release(_metaScreenPresenter);
        }
    }
}