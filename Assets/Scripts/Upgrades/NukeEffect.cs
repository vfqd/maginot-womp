using System;
using DG.Tweening;
using Map;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    [Serializable]
    public class NukeEffect : UpgradeEffect
    {
        public UpgradePanel upgradePanel;
        public CanvasGroup fullscreenImage;
        public GameObject thankYouText;
        
        public override void Execute(UpgradeNode node)
        {
            upgradePanel.Hide();
            fullscreenImage.alpha = 0;
            fullscreenImage.gameObject.SetActive(true);
            Camera.main.transform.DOShakePosition(5, 10, 20);
            Sequence s = DOTween.Sequence();
            s.Append(fullscreenImage.DOFade(1, 3));
            s.AppendInterval(3);
            s.AppendCallback(() => thankYouText.gameObject.SetActive(true));
            s.Play();
        }
    }
}