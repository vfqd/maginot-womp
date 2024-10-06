using System.Collections;
using DG.Tweening;
using Library;
using Library.Sprites;
using UI;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game
{
    public class CutsceneController : MonoSingleton<CutsceneController>
    {
        public Transform wave;
        public GameObject sandcastle;
        public ShowUpgradesButton button;
        public GameObject canvas;
        public UnityEngine.Rendering.Universal.PixelPerfectCamera ppc;
        public SpriteAnimator[] womps;
        public SpriteAnimation sad;
        
        public void PlayCutscene()
        {
            // ppc.transform.position = new Vector3(48, 64, -10);
            ppc.assetsPPU = 20;
            canvas.SetActive(false);
            
            wave.transform
                .DOMoveX(65, 3)
                .SetSpeedBased()
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    if (wave.position.x > sandcastle.transform.position.x)
                    {
                        sandcastle.SetActive(false);
                        foreach (var spriteAnimator in womps)
                        {
                            spriteAnimator.Play(sad);
                        }
                    }
                })
                .OnComplete(EndCutscene);
        }
        
        public void EndCutscene()
        {
            // ppc.transform.position = new Vector3(48, 64, -10);
            ppc.assetsPPU = 8;
            canvas.SetActive(true);
            button.GetComponent<Button>().onClick.Invoke();
            gameObject.SetActive(false);
        }
    }
}