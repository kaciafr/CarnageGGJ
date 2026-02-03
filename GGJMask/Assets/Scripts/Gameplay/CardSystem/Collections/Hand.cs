using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.CardSystem.Collections
{
    public class Hand : CardCollection
    {
        public int MaxCards { get; private set; }
        public Hand(int maxCards = 5)
        {
            MaxCards = maxCards;
        }
        
        public ICard RemoveCardAtIndex(int index)
        {
            var card = GetCard(index);
            if (card == null)
            {
                Debug.LogError($"no card found at index {index}");
                return null;
            }
            RemoveCard(card);
            return card;
        }

        public override bool AddCard(ICard card)
        {
            if(Count >= MaxCards)
                return false;
            
            return base.AddCard(card);
        }
        
        public List<ICard> GetCards()
        {
            return cards; 
        }
    }
}