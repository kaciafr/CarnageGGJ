using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Gameplay.CardSystem.Collections
{
    public class CardCollection
    {
        public event Action<ICard> CardAdded;
        public event Action<ICard> CardRemoved;

        public List<ICard> cards = new ();
        public List<ICard> Cards => cards;

        public int Count => cards.Count;

        public virtual bool AddCard(ICard card)
        {
            if (!cards.Contains(card))
            {
                cards.Add(card);
                CardAdded?.Invoke(card);
                return true;
            }

            return false;
        }

        public virtual bool RemoveCard(ICard card)
        {
            if (cards.Remove(card))
            {
                CardRemoved?.Invoke(card);
                return true;
            }

            return false; 
        }

        public ICard GetCard(int index)
        {
            if(index < 0 || index >= cards.Count)
                return null;
            
            return cards[index];
        }

        public void Clear()
        {
            using (ListPool<ICard>.Get(out var temp))
            {
                temp.AddRange(cards);
                foreach (var card in temp)
                    RemoveCard(card);
            }
        }
    }
}