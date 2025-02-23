using System.Collections.Generic;
using Modules.UIService;

namespace Meta.MetaScreen
{
    public class MetaScreenModel : UIModel
    {
        public IReadOnlyList<string> PuzzleIds { get; }
    }
}