using System;
using Framework;
using UnityEngine;

namespace Library.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static void ExecuteAfterSeconds(this MonoBehaviour monoBehaviour, float seconds, Action action)
        {
            monoBehaviour.StartCoroutine(CoroutineUtils.AfterSeconds(seconds, action));
        }
    }
}