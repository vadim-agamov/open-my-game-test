using System.Collections.Generic;
using System.Threading;
using Common.Configs;
using Cysharp.Threading.Tasks;
using Modules.Extensions;
using Modules.UIService;
using Modules.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.MetaScreen
{
    // TODO: Unload off-screen puzzles (GameObjectsPool.Release)
    public class MetaScreenView : UIView<MetaScreenPresenter>
    {
        [SerializeField]
        private Transform _puzzleContainer;
        
        [SerializeField]
        private ScrollRect _scrollRect;
        
        [SerializeField]
        private PuzzlePreviewView _puzzlePreviewPrefab;

        public override async UniTask Show(CancellationToken token)
        {
            await base.Show(token);
            _scrollRect.onValueChanged.AddListener(OnScroll);
        }

        public override async UniTask Hide(CancellationToken token)
        {
            await base.Hide(token);
            _scrollRect.onValueChanged.RemoveListener(OnScroll);
        }

        private void OnScroll(Vector2 position)
        {
            if (position.y <= 0)
            {
                var puzzles = Presenter.NextPage();
                AddPuzzles(puzzles);
            }
        }

        public void AddPuzzles(IReadOnlyList<PuzzleInfoConfig> puzzles) => 
            puzzles.ForEach(puzzle => GameObjectsPool.Get(_puzzlePreviewPrefab, _puzzleContainer).Setup(puzzle, Presenter.PuzzleSelected));
    }
}