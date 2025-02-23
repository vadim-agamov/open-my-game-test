using Core;
using Cysharp.Threading.Tasks;
using Modules.Fsm;
using Modules.UIService;

namespace Meta.PuzzleStartWindow
{
    public class PuzzleStartWindowPresenter : UIPresenter<PuzzleStartWindowView, PuzzleStartWindowModel>
    {
        public const string KEY = "PuzzleStartWindow";

        public PuzzleStartWindowPresenter(PuzzleStartWindowModel model, PuzzleStartWindowView view) : base(model, view)
        {
        }

        public void Play()
        {
            PayAsync().Forget();

            async UniTask PayAsync()
            {
                await Hide(Bootstrapper.SessionToken);
                await Fsm.Enter(new CoreState(Model.PuzzleInfoConfig), Bootstrapper.SessionToken);
            }
        }
    }
}