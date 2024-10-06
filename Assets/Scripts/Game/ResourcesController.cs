using System;
using System.Collections.Generic;
using System.Linq;
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

        public ResourcePile CreateResourcePileAt(ResourceType type, Vector3 location, float value, bool collectable)
        {
            print($"Create resource pile {type} {location}");
            var newPile = Instantiate(pilePrefab, location, Quaternion.identity,transform);
            newPile.type = type;
            newPile.spriteRenderer.sprite = resourceSprites[type];
            newPile.value = value;
            newPile.canBeCollected = collectable;
            piles.Add(newPile);
            return newPile;
        }

        public Tile GetHubDropPoint()
        {
            return MapController.Instance.Grid.GetTile(hub.center.X - 3, hub.center.Y - 1);
        }

        public void ReserveResourcePile(Runner runner, ResourcePile pile)
        {
            reservations[runner] = pile;
        }

        public ResourcePile GetHighestValueResourcePile(Vector3 location)
        {
            var reserved = new HashSet<ResourcePile>(reservations.Values);

            var possible = new List<ResourcePile>();
            foreach (var pile in piles)
            {
                if (!pile.canBeCollected) continue;
                if (reserved.Contains(pile)) continue;
                possible.Add(pile);
            }

            if (possible.Count == 0) return null;
            return possible
                .OrderByDescending(pile => (int)pile.type)
                .ThenByDescending(pile => pile.value).First();
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
                    resourceCounts[pile.type] = Mathf.Max(0, resourceCounts[pile.type]+pile.value);
                    Destroy(pile.gameObject);
                });
        }

        public void SetResourceValue(ResourceType resourceType, float value)
        {
            resourceCounts[resourceType] = Mathf.Max(0, value);
        }
        
        public void ChangeResourceValue(ResourceType resourceType, float value)
        {
            resourceCounts[resourceType] = Mathf.Max(0, resourceCounts[resourceType]+value);
        }
    }
}