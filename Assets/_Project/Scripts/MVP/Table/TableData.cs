using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.MVP.Table
{
    [Serializable]
    public class TableData
    {
        [SerializeField] public Transform cardsParent;
        [SerializeField] public List<Transform> cardPoints;
    }
}