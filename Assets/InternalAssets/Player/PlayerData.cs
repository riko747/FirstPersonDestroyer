using System;
using InternalAssets.Other;
using UnityEngine;

namespace InternalAssets.Player
{
    public class PlayerData : MonoBehaviour, ICharacterData
    {
        [SerializeField] private int powerPoints = 50;
        public int HealthPoints { get; set; } = 1;

        public int PowerPoints
        {
            get => powerPoints;
            set => powerPoints = Math.Clamp(value, 0, 100);
        }
    }
}
