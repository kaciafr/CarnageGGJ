using Gameplay.CardSystem;

using UnityEngine;


namespace Masque.Effect
{
    [CreateAssetMenu(menuName = "Effect/SwapHandWithRiverEffect")]
    public class SwapHandWithRiverEffect : MaskEffect
    {
        public override void ApplyEffect(TurnManager manager)
        {
            var currentPlayer = manager.CurrentTurnPlayer;
            
            if (currentPlayer.MainHand.Count < 2)
            {
                Debug.LogWarning("Pas assez de cartes dans la main !");
                return;
            }
            
            if (manager.River.Count < 2)
            {
                Debug.LogWarning(" Pas assez de cartes dans la River !");
                return;
            }

            ICard handCard1 = currentPlayer.MainHand.Cards[0];
            ICard handCard2 = currentPlayer.MainHand.Cards[1];
            ICard riverCard1 = manager.River.Cards[0];
            ICard riverCard2 = manager.River.Cards[1];

            currentPlayer.MainHand.RemoveCard(handCard1);
            currentPlayer.MainHand.RemoveCard(handCard2);
            
            manager.River.RemoveCard(riverCard1);
            manager.River.RemoveCard(riverCard2);

            currentPlayer.MainHand.AddCard(riverCard1);
            currentPlayer.MainHand.AddCard(riverCard2);
            
            manager.River.AddCard(handCard1);
            manager.River.AddCard(handCard2);

            Debug.Log("🔄 2 cartes de la main échangées avec 2 de la River !");
        }

        public override void RemoveEffect()
        {
        }
    }
}

