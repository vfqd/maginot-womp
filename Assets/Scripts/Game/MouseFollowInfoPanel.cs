using TMPro;
using UnityEngine;

namespace Game
{
    public class MouseFollowInfoPanel : MonoBehaviour
    {
        public SpriteRenderer icon;
        public TextMeshPro text;

        public void Toggle(bool on)
        {
            gameObject.SetActive(on);
        }
    }
}