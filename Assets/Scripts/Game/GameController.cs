using System;
using Library;
using Map;
using UnityEngine;
using Womps;

namespace Game
{
    public class GameController : MonoSingleton<GameController>
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        public bool CanAffordLadder(Tile tile)
        {
            return true;
        }
        
        public bool CanAffordCastle(Tile tile)
        {
            return true;
        }
    }
}