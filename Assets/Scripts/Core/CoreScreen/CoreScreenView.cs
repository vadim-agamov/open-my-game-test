using Modules.UIService;

namespace Meta.MetaScreen
{
    public class CoreScreenView : UIView<CoreScreenPresenter>
    {
        public void OnClick() => Presenter.SwitchToMeta();
    }
}