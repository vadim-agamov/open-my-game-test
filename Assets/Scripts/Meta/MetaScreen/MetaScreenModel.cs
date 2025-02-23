using System.Collections.Generic;
using Common.Configs;
using Common.PuzzlesManager;
using Modules.DiContainer;
using Modules.UIService;

namespace Meta.MetaScreen
{
    public class MetaScreenModel : UIModel
    {
        private PuzzlesManager PuzzlesManager { get; set; }
        
        public IReadOnlyList<PuzzleInfoConfig> AllPuzzles => PuzzlesManager.AllPuzzles;

        
        [Inject]
        private void Initialize(PuzzlesManager puzzlesManager)
        {
            PuzzlesManager = puzzlesManager;
        }
    }
}