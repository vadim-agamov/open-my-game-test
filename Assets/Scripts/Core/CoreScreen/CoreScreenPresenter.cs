using Cysharp.Threading.Tasks;
using Modules.Fsm;
using Modules.UIService;

namespace Meta.MetaScreen
{
    public class CoreScreenPresenter : UIPresenter<CoreScreenView, CoreScreenModel>
    {
        
        public const string KEY = "CoreScreen";

        public CoreScreenPresenter(CoreScreenModel model, CoreScreenView view) : base(model, view)
        {
        }

        public void SwitchToMeta() => Fsm.Enter(new MetaState(), Bootstrapper.SessionToken).Forget();
    }
}