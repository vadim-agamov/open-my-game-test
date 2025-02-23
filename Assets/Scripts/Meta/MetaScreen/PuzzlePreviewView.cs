using System;
using Common.Configs;
using Cysharp.Threading.Tasks;
using Modules.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Meta.MetaScreen
{
    public class PuzzlePreviewView : MonoBehaviour, IPooledGameObject
    {
        [SerializeField]
        private Image _previewImage;
        
        [SerializeField]
        private GameObject _preloadIcon;
        
        [SerializeField]
        private GameObject _lockIcon;
        
        private Action<PuzzleInfoConfig> _onClick;
        private PuzzleInfoConfig _puzzleInfoConfig;
    
        string IPooledGameObject.Id => nameof(PuzzlePreviewView);

        GameObject IPooledGameObject.GameObject => gameObject;

        void IPooledGameObject.Release() => Addressables.Release(_previewImage.sprite);

        public void Setup(PuzzleInfoConfig puzzleInfoConfig, Action<PuzzleInfoConfig> onClick)
        {
            _puzzleInfoConfig = puzzleInfoConfig;
            _onClick = onClick;
            LoadIcon(puzzleInfoConfig).Forget();
        }

        private async UniTaskVoid LoadIcon(PuzzleInfoConfig puzzleInfoConfig)
        {
            using var _ = ShowPreloader();
            
            await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(0.5f, 2f))); // emulate loading from remote location
            _previewImage.sprite = await Addressables.LoadAssetAsync<Sprite>(puzzleInfoConfig.PreviewImage)
                .ToUniTask(cancellationToken: destroyCancellationToken);
        }
        
        public void OnClick() => _onClick.Invoke(_puzzleInfoConfig);
        
        private IDisposable ShowPreloader()
        {
            _preloadIcon.SetActive(true);
            return new Disposable(() => _preloadIcon.SetActive(false));
        }
    }
}