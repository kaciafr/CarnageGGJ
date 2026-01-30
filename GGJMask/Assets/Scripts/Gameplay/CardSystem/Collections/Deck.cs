using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.CardSystem.Collections
{
    public class Deck : CardCollection
    {
        private class ShuffleComparer : IComparer<ICard>
        {
            public int Compare(ICard x, ICard y)
            {
                float a = Random.value;
                float b = Random.value;

                return a.CompareTo(b);
            }
        }


       
        public void Shuffle()
        {
            using (ListPool<ICard>.Get(out var list))
            {
                list.AddRange(Cards);
                list.Sort(new ShuffleComparer());
                
                Clear();
                foreach (ICard card in list)
                    AddCard(card);
            }
        }

        public ICard DrawCard()
        {
            if (Cards.Count > 0)
            {
                ICard card = Cards[0];
                RemoveCard(card);
                return card;
            }
            
            return null;
        }

        public IEnumerable<ICard> DrawCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ICard card = DrawCard();
                yield return card;
            }
        }
    }
}