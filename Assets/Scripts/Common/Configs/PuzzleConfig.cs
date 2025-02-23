using UnityEngine;

namespace Common.Configs
{
    [CreateAssetMenu(menuName = "Create PuzzleConfig", fileName = "PuzzleConfig", order = 0)]
    public class PuzzleConfig : ScriptableObject
    {
        [SerializeField] 
        private string _id;
        
        public string Id => _id;
    }
}