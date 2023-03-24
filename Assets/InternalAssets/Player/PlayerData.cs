using System;
using InternalAssets.Other;
using UnityEngine;

namespace InternalAssets.Player
{
    public class PlayerData : MonoBehaviour, ICharacterData
    {
        private int _healthPoints = 100;
        private int _powerPoints = 50;

        public int HealthPoints
        {
            get => _healthPoints;
            set => _healthPoints = Mathf.Clamp(value, 0, 100);
        }

        public int PowerPoints
        {
            get => _powerPoints;
            set => _powerPoints = Math.Clamp(value, 0, 100);
        }
        
        public int Score { get; set; }
    }
}
