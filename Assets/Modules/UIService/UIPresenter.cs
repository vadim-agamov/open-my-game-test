using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.UIService
{
    public abstract class UIPresenter : IDisposable
    {
        public UIView ViewBase { get; }
        
        protected UIPresenter(UIView view)
        {
            ViewBase = view;
        }

        public abstract void Dispose();
    }

    public abstract class UIPresenter<TView, TModel> : UIPresenter
        where TView : UIView
        where TModel : UIModel
    {
        public TView View => (TView) ViewBase;
        public TModel Model { get; private set; }

        protected UIPresenter(TModel model, TView view) : base(view)
        {
            Model = model;
            View.Attach(this);
        }
        
        public void UpdateModel(TModel model) => Model = model;
        public virtual UniTask Show(CancellationToken cancellationToken) => View.Show(cancellationToken);
        public virtual UniTask Hide(CancellationToken cancellationToken) => View.Hide(cancellationToken);
        public override void Dispose() { }
    }
}