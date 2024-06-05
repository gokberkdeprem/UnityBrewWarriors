using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(fileName = "NewTroop", menuName = "Game/TroopConfig")]
    public class TroopConfig : ScriptableObject
    {
        [SerializeField] public List<WarriorType> warriorTypes;
        public float TroopDelay;
    }
}