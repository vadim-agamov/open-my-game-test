using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.UIService
{
    public abstract class UIView: MonoBehaviour
    {
        public GameObject GameObject => gameObject;
        protected UIPresenter PresenterBase { get; private set; }
        public void Attach(UIPresenter presenter) => PresenterBase = presenter;

        public virtual UniTask Show(CancellationToken token)
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide(CancellationToken token)
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
    
    public abstract class UIView<TPresenter> : UIView 
        where TPresenter : UIPresenter
    {
        protected TPresenter Presenter => (TPresenter)PresenterBase;
    }
}