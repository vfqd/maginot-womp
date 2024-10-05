using UnityEngine;

namespace Effects
{
    public class ScrollingSprite : MonoBehaviour
    {
        public float speed;
        public float currentScroll;
        private Material _material;
        void Start()
        {
            _material = GetComponent<Renderer>().material;
            _material.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        void Update()
        {
            currentScroll += speed * Time.deltaTime;
            _material.mainTextureOffset = new Vector2(currentScroll, currentScroll);
        }
    }
}