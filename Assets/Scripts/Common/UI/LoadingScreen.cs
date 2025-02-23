using System;
using UnityEngine;

namespace Loading
{
    public class LoadingScreen : MonoBehaviour, IProgress<float>
    {
        public void Awake() => DontDestroyOnLoad(gameObject);
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        
        void IProgress<float>.Report(float value)
        {
            // TODO: Display progress
        }
    }
}