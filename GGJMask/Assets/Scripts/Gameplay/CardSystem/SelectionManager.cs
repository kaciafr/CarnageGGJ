using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.CardSystem
{


    public class SelectionManager : MonoBehaviour
    {
        [FormerlySerializedAs("GameManagerRef")] public UtilitiesCard utilitiesCardRef;

        private SelectionType currentSelectionMode = SelectionType.None;

        private readonly List<UI.CardUI> selectedCards = new List<UI.CardUI>();

        [SerializeField] private int maxSelectionAttack = 3;
        [SerializeField] private int maxSelectionDefense = 2;

        public SelectionType CurrentSelectionMode => currentSelectionMode;

        private void Start()
        {
            if (utilitiesCardRef == null)
            {
                utilitiesCardRef = FindObjectOfType<UtilitiesCard>();
            }
        }

        public void SetSelectionMode(SelectionType mode)
        {
            currentSelectionMode = mode;
            ClearSelection();
            Debug.Log($"Selection mode set to: {currentSelectionMode}");
        }

        public void ToggleCardSelection(UI.CardUI cardUI)
        {
            if (cardUI == null) return;

            SelectionType effectiveMode = currentSelectionMode == SelectionType.None
                ? SelectionType.Attack
                : currentSelectionMode;

            if (selectedCards.Contains(cardUI))
            {
                selectedCards.Remove(cardUI);
                cardUI.Deselect();
                Debug.Log($"Carte désélectionnée. Total sélectionnées: {selectedCards.Count}");
                return;
            }

            int maxAllowed = effectiveMode == SelectionType.Attack ? maxSelectionAttack : maxSelectionDefense;
            if (selectedCards.Count >= maxAllowed)
            {
                Debug.Log($"Limite de sélection atteinte pour {effectiveMode} (max {maxAllowed})");
                return;
            }

            selectedCards.Add(cardUI);
            cardUI.Select(effectiveMode);
            Debug.Log($"Carte sélectionnée en mode {effectiveMode}. Total: {selectedCards.Count}/{maxAllowed}");
        }

        public List<UI.CardUI> GetSelectedCardUIs()
        {
            return new List<UI.CardUI>(selectedCards);
        }

        public void ConfirmTransferToAttack()
        {
            if (utilitiesCardRef == null)
            {
                Debug.LogWarning("GameManagerRef non assigné dans SelectionManager.");
                return;
            }

            var sel = GetSelectedCardUIs();
            if (sel.Count == 0) return;

            utilitiesCardRef.TransferSelectedToAttack(sel);
            ClearSelection();
        }

        public void ConfirmTransferToDefense()
        {
            if (utilitiesCardRef == null)
            {
                Debug.LogWarning("GameManagerRef non assigné dans SelectionManager.");
                return;
            }

            var sel = GetSelectedCardUIs();
            if (sel.Count == 0) return;

            utilitiesCardRef.TransferSelectedToDefense(sel);
            ClearSelection();
        }

        public void ClearSelection()
        {
            for (int i = selectedCards.Count - 1; i >= 0; i--)
            {
                var ui = selectedCards[i];
                if (ui != null) ui.Deselect();
            }
            selectedCards.Clear();
            Debug.Log("Sélection nettoyée.");
        }

        public void SetMaxSelection(int attackMax, int defenseMax)
        {
            maxSelectionAttack = Mathf.Max(0, attackMax);
            maxSelectionDefense = Mathf.Max(0, defenseMax);
        }
    }
}
