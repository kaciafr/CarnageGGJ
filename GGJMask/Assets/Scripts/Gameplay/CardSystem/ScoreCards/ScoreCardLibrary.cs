using System;
using UnityEngine;

namespace Gameplay.CardSystem.PointCards
{
    [CreateAssetMenu( menuName = "Card/PointCardLibrary", order = 0)]
    public class ScoreCardLibrary : ScriptableObject
    {
        [Serializable]
        public struct ColorForSuit
        {
            [field: SerializeField]
            public CardSuit Suit { get; private set; }
            [field: SerializeField]
            public Color Color { get; private set; }
        }
        
        [Serializable]
        public struct ValueInfos
        {
            [field: SerializeField]
            public CardValue Value { get; private set; }
            [field: SerializeField, Range(0, 5)]
            public int Score { get; private set; }
            [field: SerializeField]
            public Sprite Icon { get; private set; }
        }

        [field: SerializeField]
        public GameObject UIPrefab { get; private set; }
        
        [field: SerializeField]
        public ColorForSuit[] Colors { get; private set; }
        
        [field: SerializeField]
        public ValueInfos[] CardValueInfos { get; private set; }

        public bool TryGetColorForSuit(CardSuit suit, out Color color)
        {
            for (int i = 0; i < Colors.Length; i++)
            {
                if (Colors[i].Suit == suit)
                {
                    color = Colors[i].Color;
                    return true;
                }
            }
            color = Color.white;
            return false;
        }

        public bool TryGetValueInfos(CardValue cardValue, out ValueInfos valueInfos)
        {
            for (int i = 0; i < CardValueInfos.Length; i++)
            {
                ValueInfos infos = CardValueInfos[i];
                if (infos.Value == cardValue)
                {
                    valueInfos = infos;
                    return true;
                }
            }
            valueInfos = default;
            return false;
        }
    }
}