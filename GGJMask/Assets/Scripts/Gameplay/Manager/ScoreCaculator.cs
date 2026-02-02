using System;
using System.Collections.Generic;
using Gameplay.CardSystem.PointCards; 

namespace Gameplay.CardSystem
{
    public static class ScoreCalculator
    {
        public static int CalculateAttackScore(IReadOnlyList<ICard> playedCards, IReadOnlyList<ICard> riverCards)
        {
            if (playedCards == null || playedCards.Count == 0) return 0;

            int baseSum = 0;
            foreach (var card in playedCards)
            {
                if (card is ScoreCard sc) baseSum += sc.Score;
            }

            var riverCount = new Dictionary<CardSuit, int>();
            foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
                riverCount[s] = 0;

            if (riverCards != null)
            {
                foreach (var rc in riverCards)
                    if (rc is ScoreCard rsc)
                        riverCount[rsc.Suit]++;
            }

            int matching = 0;
            foreach (var p in playedCards)
                if (p is ScoreCard scoreCard)
                    matching += riverCount[scoreCard.Suit];

            int multiplier = 1 + matching;
            return baseSum * multiplier;
        }

        public static int CalculateDefenseScore(IReadOnlyList<ICard> playedDefense)
        {
            if (playedDefense == null || playedDefense.Count == 0) return 0;
            int baseSum = 0;
            foreach (var c in playedDefense)
                if (c is ScoreCard sc) baseSum += sc.Score;
            return baseSum;
        }
    }
}