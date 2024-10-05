using System;
using System.Collections.Generic;
using DG.Tweening;
using Library;
using Library.Extensions;
using Map;
using UnityEngine;
using Womps;

namespace Game
{
    public class ResourcesController : SerializedMonoSingleton<ResourcesController>
    {
        public Dictionary<ResourceType, float> resourceCounts;
        public Dictionary<ResourceType, Sprite> resourceSprites;
        
        public ResourcePile pilePrefab;

        public Building hub;

        public List<ResourcePile> piles;
        private Dictionary<Runner, ResourcePile> reservations;

        protected override void Awake()
        {
            base.Awake();
            piles = new List<ResourcePile>();
            reservations = new Dictionary<Runner, ResourcePile>();
            resourceCounts = new Dictionary<ResourceType, float>();

            foreach (var resourceType in Enum.GetNames(typeof(ResourceType)))
            {
                var value = Enum.Parse<ResourceType>(resourceType, true);
                resourceCounts[value] = 0;
            }
        }

        public void CreateResourcePileAt(ResourceType type, Vector3 location, float value)
        {
            print($"Create resource pile {type} {location}");
            var newPile = Instantiate(pilePrefab, location, Quaternion.identity,transform);
            newPile.type = type;
            newPile.spriteRenderer.sprite = resourceSprites[type];
            newPile.value = value;
            piles.Add(newPile);
        }

        public Tile GetHubDropPoint()
        {
            return MapController.Instance.Grid.GetTile(hub.center.X - 3, hub.center.Y - 1);
        }

        public void ReserveResourcePile(Runner runner, ResourcePile pile)
        {
            reservations[runner] = pile;
        }

        public ResourcePile GetNearestResourcePileTo(Vector3 location)
        {
            ResourcePile closest = null;
            float dist = Single.MaxValue;
            var reserved = new HashSet<ResourcePile>(reservations.Values);

            foreach (var pile in piles)
            {
                if (reserved.Contains(pile)) continue;
                var d = Vector3.Distance(pile.transform.position, location);
                if (d < dist)
                {
                    closest = pile;
                    dist = d;
                }
            }
            return closest;
        }

        public void DepositResourcePile(Runner runner, ResourcePile pile)
        {
            if (piles.Contains(pile)) piles.Remove(pile);
            if (reservations.ContainsKey(runner)) reservations.Remove(runner);

            var start = runner.transform.position;
            var end = hub.transform.position + new Vector3(-1.05f,1.1f);
            var mid = new Vector3((start.x + end.x) / 2, end.y + 3);
            pile.transform.DOPath(new[] { start, mid, end }, 0.5f, PathType.CatmullRom)
                .OnComplete(() =>
                {
                    resourceCounts[pile.type] += pile.value;
                    Destroy(pile.gameObject);
                });
        }
    }
}