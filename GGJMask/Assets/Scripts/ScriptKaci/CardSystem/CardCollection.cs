using System;
using System.Collections.Generic;

namespace ScriptKaci
{
    public class CardCollection
    {
        private readonly HashSet<ICard> cards;
        
        public event Action<ICard> cardAdded;
        public event Action<ICard> cardRemoved;
        public IReadOnlyCollection<ICard> Cards => this.cards;
        
        public CardCollection()
        {
            cards = new HashSet<ICard>();
        }

        public bool AddCard(ICard card)
        {
            if (cards.Add(card))
            {
                cardAdded?.Invoke(card);
                return true;
            }

            return false;
        }

        public bool RemoveCard(ICard card)
        {
            if (cards.Remove(card))
            {
                cardRemoved?.Invoke(card);
                return true;
            }

            return false; 
        }
        
    }
}