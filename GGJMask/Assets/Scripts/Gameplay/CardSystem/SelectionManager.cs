using System.Collections.Generic;
using UnityEngine;
using Gameplay.CardSystem.UI;

namespace Gameplay.CardSystem
{
    public class SelectionManager : MonoBehaviour
    {
        private List<CardUI> selectedCards = new List<CardUI>();
        
        [SerializeField] private int maxSelectionAttack = 3;
        [SerializeField] private int maxSelectionDefense = 2;
        
        private SelectionType currentSelectionMode = SelectionType.None;

        public void SetSelectionMode(SelectionType mode)
        {
            currentSelectionMode = mode;
            ClearSelection();
            Debug.Log($"Mode de sélection activé: {mode}");
        }

        public void ToggleCardSelection(CardUI cardUI)
        {
            if (currentSelectionMode == SelectionType.None)
            {
                Debug.Log("Aucun mode de sélection actif!");
                return;
            }

            if (selectedCards.Contains(cardUI))
            {
                selectedCards.Remove(cardUI);
                cardUI.Deselect();
                Debug.Log($"Carte désélectionnée. Total: {selectedCards.Count}");
            }
            else
            {
                int maxCards = currentSelectionMode == SelectionType.Attack ? maxSelectionAttack : maxSelectionDefense;
                
                if (selectedCards.Count >= maxCards)
                {
                    Debug.Log($"Limite atteinte! Maximum: {maxCards} cartes");
                    return;
                }

                selectedCards.Add(cardUI);
                cardUI.Select(currentSelectionMode);
                Debug.Log($"Carte sélectionnée. Total: {selectedCards.Count}/{maxCards}");
            }
        }

        public List<ICard> GetSelectedCards()
        {
            List<ICard> cards = new List<ICard>();
            foreach (CardUI cardUI in selectedCards)
            {
                if (cardUI.CurrentCard != null)
                {
                    cards.Add(cardUI.CurrentCard);
                }
            }
            return cards;
        }

        public void ClearSelection()
        {
            foreach (CardUI cardUI in selectedCards)
            {
                cardUI.Deselect();
            }
            selectedCards.Clear();
            Debug.Log("Sélection effacée");
        }

        public int GetSelectedCount()
        {
            return selectedCards.Count;
        }

        public SelectionType GetCurrentMode()
        {
            return currentSelectionMode;
        }
    }
}
