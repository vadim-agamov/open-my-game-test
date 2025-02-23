using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Common.Configs
{
    [CreateAssetMenu(menuName = "Create PuzzleInfoConfig", fileName = "PuzzleInfoConfig", order = 0)]
    public class PuzzleInfoConfig : ScriptableObject
    {
        [SerializeField] 
        private string _id;
        
        [SerializeField]
        private string _description;
        
        [SerializeField]
        private string _category;
        
        [SerializeField]
        private AssetReferenceSprite _previewImage;
        
        public string Id => _id;
        public string Description => _description;
        public string Category => _category;
        public AssetReferenceSprite PreviewImage => _previewImage;
        
        // TODO: add reference to config with puzzle data

        private void OnValidate()
        {
            _description = $"Puzzle {_id}";
        }
        
#if UNITY_EDITOR
        public void SetPuzzleInfo(string id, string description, string category, AssetReferenceSprite previewImage)
        {
            _id = id;
            _description = description;
            _category = category;
            _previewImage = previewImage;
        }
#endif
    }
}