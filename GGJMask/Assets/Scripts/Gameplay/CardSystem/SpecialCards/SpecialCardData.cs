using UnityEngine;

namespace Gameplay.CardSystem.SpecialCards
{
    [CreateAssetMenu( menuName = "Card/SpecialcardData", order = 0)]
    public class SpecialCardData : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }

        [field: SerializeField] 
        public SpecialCardType Type { get; private set; } = SpecialCardType.Joker;
        
        [field: SerializeField]
        public Color Color { get; private set; } = Color.black;
    }
}