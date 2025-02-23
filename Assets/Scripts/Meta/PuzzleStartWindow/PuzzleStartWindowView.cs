using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.UIService;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Meta.PuzzleStartWindow
{
    public class PuzzleStartWindowView : UIView<PuzzleStartWindowPresenter>
    {
        [SerializeField] 
        private Image _puzzleImage;

        [SerializeField] 
        private GameObject _playFreeButton;

        [SerializeField]
        private GameObject _playByCoinsButton;

        [SerializeField] 
        private TMP_Text _playByCoinsText;
        
        [SerializeField] 
        private TMP_Text _descriptionText;

        public override async UniTask Show(CancellationToken token)
        {
            await base.Show(token);
            _descriptionText.text = Presenter.Model.PuzzleInfoConfig.Description;
            _playFreeButton.SetActive(Presenter.Model.IsFree);
            _playByCoinsButton.SetActive(!Presenter.Model.IsFree);
            _playByCoinsText.text = Presenter.Model.Cost.ToString();
            _puzzleImage.sprite = await Addressables.LoadAssetAsync<Sprite>(Presenter.Model.PuzzleInfoConfig.PreviewImage)
                .ToUniTask(cancellationToken: token);
        }

        public void CloseClicked() => Presenter.Hide(Bootstrapper.SessionToken).Forget();

        public void PlayClicked() => Presenter.Play();
    }
}