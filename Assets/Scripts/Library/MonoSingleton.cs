﻿using UnityEngine;

namespace Library
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        public static bool HasInstance => _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"{typeof(T)} is missing.");
                }

                return _instance;
            }
        }



        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning($"Second instance of {typeof(T)} created. Automatic self-destruct triggered.");
                Destroy(gameObject);
                return;
            }
            _instance = this as T;
        }


        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}