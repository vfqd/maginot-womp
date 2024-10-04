using Framework;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Library.Shapes
{
    [RequireComponent(typeof(ShapeRenderer),typeof(RectTransform))]
    public class ShapeRenderFollowRectTransform : MonoBehaviour
    {
        [SerializeField] private bool updateOnStart = true;
    
        private void Start()
        {
            if (updateOnStart) UpdateShape();
        }

        [Button]
        public void UpdateShape()
        {
            var rectT = GetComponent<RectTransform>();
            var rect = GetComponent<Rectangle>();

            if (rect)
            {
                rect.Width = rectT.GetLocalWidth();
                rect.Height = rectT.GetLocalHeight();
            }
        }
    }
}