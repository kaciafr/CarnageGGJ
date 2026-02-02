using Gameplay.CardSystem.PointCards;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "GameMetrics", menuName = "Game Metrics")]
    public class GameMetrics : ScriptableObject
    {
        [field: SerializeField] 
        public ScoreCardLibrary ScoreCardLibrary { get; private set; }
        [field: SerializeField] 
        public int HandSize { get; private set; } = 5;
        [field: SerializeField] 
        public int AttackSize { get; private set; } = 3;
        [field: SerializeField] 
        public int DefenseSize { get; private set; } = 2;
        [field: SerializeField] 
        public int RiverSize { get; private set; } = 5;
        [field: SerializeField] 
        public int NumberSpecial { get; private set; } = 2;


        [field: SerializeField] public GameObject CardPrefab { get; private set; }
    }
}