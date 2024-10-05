using System;
using Game;
using Library.Grid;
using UnityEngine;

namespace Map
{
    public class TileHealth : MonoBehaviour
    {
        public double maxHp;
        public double hp;

        private Tile tile;

        private void Awake()
        {
            tile = GetComponent<Tile>();
        }

        public void SetMaxHp(double newMax)
        {
            maxHp = hp = newMax;
        }
        
        public void DealDamage(float amount)
        {
            hp -= amount;

            if (hp <= 0)
            {
                MiningController.Instance.RemoveTileFromMiningList(tile);
                tile.ChangeTileTo(TileType.Air);
            }
        }
    }
}