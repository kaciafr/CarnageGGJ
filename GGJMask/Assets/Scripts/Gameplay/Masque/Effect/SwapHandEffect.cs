using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.UI;
using UnityEngine;
using System.Collections.Generic;

namespace Masque.Effect
{
    [CreateAssetMenu(menuName = "Effect/SwapHandsEffect")]
    public class SwapHandsEffect : MaskEffect
    {
        public override void ApplyEffect(TurnManager manager)
        {
            var player1 = manager.Players[0];
            var player2 = manager.Players[1];

            List<ICard> tempHand = new List<ICard>(player1.MainHand.Cards);

            player1.MainHand.Clear();
            foreach (var card in player2.MainHand.Cards)
            {
                player1.MainHand.AddCard(card);
            }

            player2.MainHand.Clear();
            foreach (var card in tempHand)
            {
                player2.MainHand.AddCard(card);
            }

            Debug.Log(" Mains échangées !");
        }

        public override void RemoveEffect()
        {
        }
    }
}