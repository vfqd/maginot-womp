using System;
using System.Collections.Generic;
using Library.Extensions;
using UnityEngine;

namespace Library.Sprites
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RandomSprite : MonoBehaviour
    {
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private bool randomizeOrthogonalRotation;

        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = sprites.GetRandomElement();
            
            if (randomizeOrthogonalRotation)
            {
                transform.rotation = Quaternion.Euler(0,0,new int[]{0,90,180,270}.GetRandomElement());
            }
        }
    }
}