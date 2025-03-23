using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Table
{
    [Serializable]
    public class TableSync
    {
        [SerializeField] public bool isSyncData = true;
        
        [SerializeField] public IntReactiveProperty bank = new();
    }
}