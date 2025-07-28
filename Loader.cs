using BepInEx;
using LockMonitor;
using System;
using UnityEngine;

namespace WalkingDeadFun
{
    [BepInPlugin("null", "null", "1.0")]
    public class Loader : BaseUnityPlugin
    {
        private void Awake()
        {
            CreateObject();
        }

        void OnDestroy()
        {
            CreateObject();
            Console.WriteLine("Ubilsya");
        }

        void CreateObject()
        {
            var WDF = new GameObject("Igor");
            WDF.AddComponent<LockMouseMonitor>();
            DontDestroyOnLoad(WDF);
            Console.WriteLine(WDF.name);
        }
    }
}