using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.DiContainer;
using Modules.Initializator;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Modules.UIService
{
    public class UIService : IUIService
    {
        private Canvas _canvas;
        private Vector2Int ReferenceResolution { get; }

        public bool IsInitialized { get; private set; }

        public UIService(Vector2Int referenceResolution) => ReferenceResolution = referenceResolution;

        UniTask IInitializable.Initialize(CancellationToken cancellationToken)
        {
            SetupCanvas();
            IsInitialized = true;
            return UniTask.CompletedTask;
        }

        private void SetupCanvas()
        {
            var rootGameObject = new GameObject(
                "[RootCanvas]",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

            var canvasScaler = rootGameObject.GetComponent<CanvasScaler>();
            canvasScaler.referenceResolution = ReferenceResolution;
            canvasScaler.matchWidthOrHeight = 0;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            _canvas = rootGameObject.GetComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            Object.DontDestroyOnLoad(rootGameObject);
        }

        async UniTask<TPresenter> IUIService.Create<TModel, TUIView, TPresenter>(TModel model, string key,
            CancellationToken token)
        {
            var op = Addressables.InstantiateAsync(key, _canvas.transform);
            await op.ToUniTask(cancellationToken: token);
            if (op.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Unable to instantiate asset from location {key}:\r\n{op.OperationException}");
                throw op.OperationException;
            }

            var viewGameObject = op.Result;
            viewGameObject.name = key;
            viewGameObject.SetActive(false);
            var view = viewGameObject.GetComponent<TUIView>();
            var presenter = System.Activator.CreateInstance(typeof(TPresenter), model, view) as TPresenter;
            Container.Inject(presenter);
            return presenter;
        }

        void IUIService.Release(UIPresenter presenter)
        {
            presenter.Dispose();
            Addressables.ReleaseInstance(presenter.ViewBase.gameObject);
        }
    }
}