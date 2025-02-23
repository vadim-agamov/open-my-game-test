using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Common.Configs
{
    [CreateAssetMenu(menuName = "Create PuzzleCatalogConfig", fileName = "PuzzleCatalogConfig", order = 0)]
    public class PuzzleCatalogConfig : ScriptableObject
    {
        [SerializeField] 
        private PuzzleInfoConfig[] _puzzles;
        
        public IReadOnlyList<PuzzleInfoConfig> Puzzles => _puzzles;
        
#if UNITY_EDITOR
        public void Validate() => OnValidate();
        
        private void OnValidate()
        {
            _puzzles = AssetDatabase.FindAssets($"t:{nameof(PuzzleInfoConfig)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<PuzzleInfoConfig>)
                .ToArray();
        }
#endif
    }
}