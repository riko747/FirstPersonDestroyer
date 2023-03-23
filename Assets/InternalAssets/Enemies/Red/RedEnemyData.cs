using InternalAssets.Other;
using UnityEngine;

namespace InternalAssets.Enemies.Red
{
    public class RedEnemyData : MonoBehaviour, ICharacterData
    {
        public int HealthPoints { get; set; } = 50;
    }
}
