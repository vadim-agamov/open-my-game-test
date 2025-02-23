using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Configs;
using Cysharp.Threading.Tasks;
using Modules.Initializator;
using UnityEngine.AddressableAssets;

namespace Common.PuzzlesManager
{
    public class PuzzlesManager : IInitializable
    {
        private const string CATALOG_KEY = "PuzzleCatalog";
        public bool IsInitialized { get; private set; }

        private PuzzleCatalogConfig _catalog;
        
        async UniTask IInitializable.Initialize(CancellationToken token)
        {
            _catalog = await Addressables.LoadAssetAsync<PuzzleCatalogConfig>(CATALOG_KEY).ToUniTask(cancellationToken: token);
            IsInitialized = true;
        }
        
        public IReadOnlyList<PuzzleInfoConfig> AllPuzzles => _catalog.Puzzles;
    }
}