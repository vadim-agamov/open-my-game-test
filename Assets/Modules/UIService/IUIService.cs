using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Initializator;

namespace Modules.UIService
{
    public interface IUIService : IInitializable
    {
        UniTask<TPresenter> Create<TModel, TUIView, TPresenter>(TModel model, string key, CancellationToken token)
            where TModel : UIModel
            where TUIView : UIView
            where TPresenter : UIPresenter;

        void Release(UIPresenter presenter);
    }
}