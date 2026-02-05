using DG.Tweening;
using Gameplay.CardSystem.Turns;
using Gameplay.CardSystem.UI;
using RunTime.TpTSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

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
        [SerializeField]
        private Canvas canvas;

        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text comboText;
        [SerializeField] private TMP_Text moneText;
        [SerializeField] private TMP_Text healthText;

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
            currentPlayer.OnChangeHealth += OnHealthChanged;
            currentPlayer.OnChangedDamage += OnDamageChanged;
            currentPlayer.OnChangedMoney += OnMoneyChanged;

            if (currentTurnManager != null)
            {
                currentTurnManager.OnDesactivateCanvas += DeactivateCanvas;
            }

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

            UpdateHealthDisplay(currentPlayer.CurrentHealth);
            UpdateMoneyDisplay();
            
            ShowDamageText();
        }

        private void OnDamageChanged()
        {
      
            UpdateDamageDisplay();
        }

        private void OnMoneyChanged()
        {
            UpdateMoneyDisplay();
        }

       
     
        private void UpdateDamageDisplay()
        {
            if (currentPlayer == null || currentTurnManager == null)
                return;

            int totalDamage = currentTurnManager.River.GetCollectionDamage(
                currentPlayer.HandAttack,  
                currentTurnManager.Metrics
            );

            if (damageText != null)
                damageText.text = $"Dégâts: {totalDamage}";

            if (comboText != null)
                comboText.text = $"Combo: 0";

            Debug.Log($"[{currentPlayer.gameObject.name}] 💥 Dégâts préparés: {totalDamage}");
        }

        private void UpdateMoneyDisplay()
        {
            if (currentPlayer == null) return;

            if (moneText != null)
                moneText.text = $"Argent: {currentPlayer.CurrentMoney}$";
        }

        public void Disconnect()
        {
            currentPlayer.OnBeginTurn -= OnNewTurnBegins;
            currentPlayer.OnEndTurn -= OnNewTurnEnds;
            currentPlayer.OnChangeHealth -= OnHealthChanged;
            currentPlayer.OnChangedDamage -= OnDamageChanged;
            currentPlayer.OnChangedMoney -= OnMoneyChanged;

            if (currentTurnManager != null)
            {
                currentTurnManager.OnDesactivateCanvas -= DeactivateCanvas;
            }

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
            int selectedCount = handUI.GetSelectedCardsCount();

            if (selectedCount != 3)
            {
                Debug.Log($"❌ Vous devez sélectionner exactement 3 cartes ! ({selectedCount}/3)");
                return; 
            }

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
            
            HideDamageText();

            currentPlayer.SetIsDone();
        }

        private void OnNewTurnBegins()
        {
          
            ShowDamageText();
            UpdateDamageDisplay();
        }

        private void OnNewTurnEnds()
        {
           
            HideDamageText();
        }

        private void UpdateHealthDisplay(int hp)
        {
            if (healthText != null)
                healthText.text = $"Pv: {hp}";
        }

        private void OnHealthChanged(int newHp, int delta)
        {
            int maxHp = (currentPlayer != null) ? currentPlayer.MaxHealth : 0;
            Debug.Log($"[CardPlayerUI] OnHealthChanged newHp={newHp}, delta={delta}, maxHp={maxHp}");

            if (healthText != null)
                healthText.text = $"Pv: {newHp}";
        }

        private void DeactivateCanvas()
        {
            if (canvas != null)
            {
                canvas.enabled = false;
            }
        }

        private void ActivateCanvas()
        {
            if (canvas != null)
            {
                canvas.enabled = true;
            }
        }
        
        private void ShowDamageText()
        {
            if (damageText != null)
            {
                damageText.gameObject.SetActive(true);
                Debug.Log($"[{currentPlayer?.gameObject.name}]  Affiche zone dégâts (préparation)");
            }

            if (comboText != null)
                comboText.gameObject.SetActive(true);
        }
        
        public void HideDamageText()
        {
            if (damageText != null)
            {
                damageText.gameObject.SetActive(false);
                Debug.Log($"[{currentPlayer?.gameObject.name}]  Cache dégâts (attaque en cours)");
            }

            if (comboText != null)
                comboText.gameObject.SetActive(false);
        }
    }
}
