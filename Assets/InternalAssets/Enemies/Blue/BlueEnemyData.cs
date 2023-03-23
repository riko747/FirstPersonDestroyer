using System;
using InternalAssets.Other;
using UnityEngine;

namespace InternalAssets.Enemies.Blue
{
    public class BlueEnemyData : MonoBehaviour, ICharacterData
    {
        public int HealthPoints { get; set; }

        private void OnEnable() => HealthPoints = 100;
    }
}
