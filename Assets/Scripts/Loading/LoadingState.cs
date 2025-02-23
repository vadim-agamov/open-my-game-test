using System;
using System.Threading;
using Common.PuzzlesManager;
using Cysharp.Threading.Tasks;
using Meta;
using Modules.DiContainer;
using Modules.Fsm;
using Modules.Initializator;
using Modules.UIService;
using Modules.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Loading
{
    public class LoadingState : IState
    {
        private readonly LoadingScreen _loadingScreen = Container.Bind(GameObject.Find("LoadingScreen").GetComponent<LoadingScreen>());

        async UniTask IState.OnEnter(CancellationToken token)
        {
            _loadingScreen.Show();
            using var _ = new Disposable(() => _loadingScreen.Hide());
            
            var uiService = Container.Bind<IUIService>(new UIService(new Vector2Int(900, 1600)));
            var puzzlesManager = Container.Bind(new PuzzlesManager());
            
            SetupEventSystem();

            await new Initializator(uiService, puzzlesManager).Do(token, _loadingScreen);
            
            await Fsm.Enter(new MetaState(), token);
        }

        UniTask IState.OnExit(CancellationToken cancellationToken) => UniTask.CompletedTask;

        private static void SetupEventSystem()
        {
            var eventSystemPrefab = Resources.Load("EventSystem");
            var eventSystem = Object.Instantiate(eventSystemPrefab);
            eventSystem.name = "[EventSystem]";
            Object.DontDestroyOnLoad(eventSystem);
        }
    }
}