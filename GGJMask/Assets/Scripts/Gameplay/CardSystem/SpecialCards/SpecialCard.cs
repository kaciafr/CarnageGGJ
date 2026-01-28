using UnityEngine;

namespace Gameplay.CardSystem.SpecialCards
{
    public class SpecialCard : ICard
    {
        public readonly SpecialCardData data;
        public Sprite Icon => data.Icon;
        
        public Color Color => data.Color;
        
        public SpecialCard(SpecialCardData data)
        {
            this.data = data;
        }

    }
}