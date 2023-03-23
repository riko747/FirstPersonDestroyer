using UnityEngine;

namespace InternalAssets.Other
{
    internal interface ICharacterData
    {
        public int HealthPoints { get; set; }
    }
    public class CharacterData : MonoBehaviour, ICharacterData
    {
        public int HealthPoints { get; set; }
        public int PowerPoints { get; set; }

        public int IncreaseHealth(int healthPoints) => HealthPoints += healthPoints;

        public int DecreaseHealth(int healthPoints) => HealthPoints -= healthPoints;
    }
}
