using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Configs;
using Common.PuzzlesManager;
using Cysharp.Threading.Tasks;
using Meta.PuzzleStartWindow;
using Modules.DiContainer;
using Modules.UIService;

namespace Meta.MetaScreen
{
    public class MetaScreenPresenter : UIPresenter<MetaScreenView, MetaScreenModel>
    {
        public const string KEY = "MetaScreen";
        private const int PAGE_SIZE = 10;
        private int _currentPage = 0;
        private int TotalPages => Model.AllPuzzles.Count / PAGE_SIZE;
        private IUIService UIService { get; set; }
        
        private PuzzleStartWindowPresenter _puzzleStartWindowPresenter;

        public MetaScreenPresenter(MetaScreenModel model, MetaScreenView view) : base(model, view)
        {
        }
        
        [Inject]
        private void Initialize(UIService uiService)
        {
            UIService = uiService;
        }

        public override async UniTask Show(CancellationToken token)
        {
            await base.Show(token);
            View.AddPuzzles(GetPage(_currentPage));
        }
        
        public IReadOnlyList<PuzzleInfoConfig> NextPage() => 
            _currentPage >= TotalPages ? Array.Empty<PuzzleInfoConfig>() : GetPage(++_currentPage);

        public void PuzzleSelected(PuzzleInfoConfig puzzle) => ShowPuzzleStartWindow(puzzle).Forget();

        private async UniTask ShowPuzzleStartWindow(PuzzleInfoConfig puzzle)
        {
            if (_puzzleStartWindowPresenter == null)
            {
                _puzzleStartWindowPresenter = await UIService.Create<PuzzleStartWindowModel, PuzzleStartWindowView, PuzzleStartWindowPresenter>(puzzle, PuzzleStartWindowPresenter.KEY, Bootstrapper.SessionToken );
            }
            else
            {
                _puzzleStartWindowPresenter.UpdateModel(new PuzzleStartWindowModel(puzzle));
            }

            await _puzzleStartWindowPresenter.Show(View.GetCancellationTokenOnDestroy());
        }

        private IReadOnlyList<PuzzleInfoConfig> GetPage(int index) =>
            Model.AllPuzzles
                .Skip(index * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();

        public override void Dispose() => UIService.Release(_puzzleStartWindowPresenter);
    }
}