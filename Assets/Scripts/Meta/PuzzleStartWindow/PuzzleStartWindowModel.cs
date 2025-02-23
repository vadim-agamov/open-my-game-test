using Common.Configs;
using Modules.UIService;
using UnityEngine;

namespace Meta.PuzzleStartWindow
{
    public class PuzzleStartWindowModel : UIModel
    {
        public PuzzleInfoConfig PuzzleInfoConfig { get; }
        
        public bool IsFree => PuzzleInfoConfig.GetHashCode() % 3 == 0; // TODO: some business logic
        
        public int Cost => Random.Range(0, 100); // TODO: some business logic

        public PuzzleStartWindowModel(PuzzleInfoConfig config) => PuzzleInfoConfig = config;

        public static implicit operator PuzzleStartWindowModel(PuzzleInfoConfig config) => new PuzzleStartWindowModel(config);
    }
}