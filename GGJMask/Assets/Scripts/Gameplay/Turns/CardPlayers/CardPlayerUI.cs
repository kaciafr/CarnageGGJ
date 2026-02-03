using Gameplay.CardSystem.Turns;
using Gameplay.CardSystem.UI;
using RunTime.TpTSystem;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.CardSystem
{
    public class CardPlayerUI : MonoBehaviour
    {
        [SerializeField]
        private CardCollectionUI handUI;
        [SerializeField]
        private CardCollectionUI defenseUI;
        [SerializeField]
        private CardCollectionUI attackUI;
        [SerializeField]
        private CanvasGroup canvasGroup;
        
        private CardPlayer currentPlayer;
        private TurnManager currentTurnManager;
        
        public void Connect(TurnManager turnManager, CardPlayer cardPlayer)
        {
            if(currentPlayer != null)
                Disconnect();
            
            currentTurnManager = turnManager;
            currentPlayer = cardPlayer;
            
            bool isRealPlayer = cardPlayer is CardLocalPlayer;

            currentPlayer.OnBeginTurn += OnNewTurnBegins;
            currentPlayer.OnEndTurn += OnNewTurnEnds;
            
            canvasGroup.blocksRaycasts = isRealPlayer;

            if (handUI != null)
            {
                handUI.CanInteract = isRealPlayer;
                handUI.Connect(cardPlayer.MainHand);
            }

            if (defenseUI != null)
            {
                defenseUI.CanInteract = isRealPlayer;   
                defenseUI.Connect(cardPlayer.HandDefense);
            }

            if (attackUI != null)
            {
                attackUI.CanInteract = isRealPlayer;   
                attackUI.Connect(cardPlayer.HandAttack);
            }
        }

        public void Disconnect()
        {
            currentPlayer.OnBeginTurn -= OnNewTurnBegins;
            currentPlayer.OnEndTurn -= OnNewTurnEnds;

            if(handUI != null)
                handUI.Disconnect();
            
            if(defenseUI != null)
                defenseUI.Disconnect();
            
            if(attackUI != null)
                attackUI.Disconnect();
            
            currentTurnManager = null;
            currentPlayer = null;
        }

        public bool CanEndTurn()
        {
            int selectedCards = 0;
            foreach (var cardUI in handUI.Cards)
            {
                if(cardUI.IsSelected)
                    selectedCards++;
            }
            
            return selectedCards == currentTurnManager.Metrics.AttackSize;
        }
        
        public void EndTurn()
        {
            using (ListPool<CardUI>.Get(out var cardUIs))
            {
                cardUIs.AddRange(handUI.Cards);
                foreach (var cardUI in cardUIs)
                {
                    if (cardUI.IsSelected)
                        cardUI.CurrentCard.Transfer(handUI.Collection, attackUI.Collection);
                    else
                        cardUI.CurrentCard.Transfer(handUI.Collection, defenseUI.Collection);
                }
            }
            
            currentPlayer.SetIsDone();
        }

        private void OnNewTurnBegins()
        {
        }

        private void OnNewTurnEnds()
        {
        }

    }
}