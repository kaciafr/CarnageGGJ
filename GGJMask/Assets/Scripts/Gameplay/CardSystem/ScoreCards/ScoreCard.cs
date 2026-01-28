using UnityEngine;

namespace Gameplay.CardSystem.PointCards
{
    public class ScoreCard : ICard
    {
        public readonly ScoreCardLibrary Library;
        public readonly CardValue Value;
        public readonly CardSuit Suit;
        
        public int Score => Library.TryGetValueInfos(Value, out var infos ) ? 
            infos.Score : 
            0;
        
        public Sprite Icon => Library.TryGetValueInfos(Value, out var infos ) ?
            infos.Icon :
            null;
        
        public Color Color => Library.TryGetColorForSuit(Suit, out var color ) ? 
            color :
            default;

        public GameObject Prefab => Library.UIPrefab;
        
        public ScoreCard(ScoreCardLibrary library, CardValue value, CardSuit suit)
        {
            this.Library = library;
            Value = value;
            Suit = suit;
        }
    }
}