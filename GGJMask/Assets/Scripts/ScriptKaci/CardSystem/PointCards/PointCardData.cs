using UnityEngine;

namespace ScriptKaci
{
    [CreateAssetMenu( menuName = "Card/cardData", order = 0)]
    public class PointCardData : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }
        [field: SerializeField]
        public int Value { get; private set; }
        [field: SerializeField]
        public CardSuit Suit { get; private set; }
        
        
    }
}