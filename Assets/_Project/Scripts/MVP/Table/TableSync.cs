using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Table
{
    [Serializable]
    public class TableSync
    {
        [SerializeField] public IntReactiveProperty bank = new();
    }
}