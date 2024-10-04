using System;
using System.Linq;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Library.Sprites
{
    [CreateAssetMenu(menuName = "Sprites/Sprite Animation")]
    public class SpriteAnimation : ScriptableObject
    {
        [TableList]
        public Frame[] frames;
        [InfoBox("$GetAnimLength")]
        public bool loop;
        public bool startAtRandomIndex;
        public int startingFrame = 0;

        private float _cachedAnimLength = -1;


        public float GetStartTimeOffset(int index)
        {
            if (startAtRandomIndex)             
                return Random.Range(0, frames[index].duration);
            // return frames.Sum(f => f.duration) ... something to do with startingFrame
            return 0;
        }

        public int GetStartIndex()
        {
            return startAtRandomIndex ? Random.Range(0, frames.Length) : startingFrame;
        }

        public float GetAnimLength()
        {
            if (_cachedAnimLength > 0) return _cachedAnimLength;
            _cachedAnimLength = frames.Sum(frame => frame.duration);
            return _cachedAnimLength;
        }

        private void OnValidate()
        {
            _cachedAnimLength = -1;
        }

        [Serializable]
        public struct Frame
        {
            [HorizontalGroup("Duration"),HideLabel]
            public float duration;
            [HorizontalGroup("Sprite"),PreviewField,HideLabel]
            public Sprite sprite;
            [HideLabel,VerticalGroup("Event")]
            public bool hasEvent;
            [HideLabel,VerticalGroup("Event"),/*ShowIf(nameof(hasEvent))*/HideInInspector]
            public string eventClassName;
            [HideLabel,VerticalGroup("Event"),/*ShowIf(nameof(hasEvent))*/HideInInspector]
            public string animationEvent;
            
/*#if UNITY_EDITOR
            private string[] GetAllScripts()
            {
                var guids = UnityEditor.AssetDatabase.FindAssets("t: Script",new[] {"Assets/Scripts"});
                List<string> names = new List<string> { "" };
                foreach (var guid in guids)
                {
                    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    names.Add(path);
                }
                return names.ToArray();
            }

            private void UpdateClassName()
            {
                var last = eventClassPath.LastIndexOf("/", StringComparison.Ordinal);
                var stop = eventClassPath.LastIndexOf(".cs", StringComparison.Ordinal);
                eventClassName = eventClassPath.Substring(last+1, stop - last - 1);
            }

            private string[] GetMethodNames()
            {
                if (eventClassPath.IsNullOrEmpty()) return Array.Empty<string>();
                var script = UnityEditor.AssetDatabase.LoadAssetAtPath<MonoBehaviour>(eventClassPath);
            }
#endif*/
        }

#if UNITY_EDITOR
        [FoldoutGroup("Editor"),Button]
        private void BatchCreateFrames(float duration, Sprite[] sprites)
        {
            frames = new Frame[sprites.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i].sprite = sprites[i];
                frames[i].duration = duration;
            }
        }
        
        [FoldoutGroup("Editor"),Button]
        private void ReplaceFramesWithNewSprites(Sprite[] originalSprites, Sprite[] newSprites)
        {
            for (var i = 0; i < frames.Length; i++)
            {
                int index = originalSprites.IndexOf(frames[i].sprite);
                frames[i].sprite = newSprites[index];
            }
        }
#endif
    }
}