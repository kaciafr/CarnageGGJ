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
        [SerializeField] private CardCollectionUI handUI;
        [SerializeField] private CardCollectionUI defenseUI;
        [SerializeField] private CardCollectionUI attackUI;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Canvas canvas;

        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text comboText;
        [SerializeField] private TMP_Text moneText;
        [SerializeField] private TMP_Text healthText;

        [Header("Containers à animer")]
        [SerializeField] private RectTransform damageContainer;
        [SerializeField] private RectTransform comboContainer;

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

            // 💥 ANIMATION DAMAGE - SCALE UNIQUEMENT (pas de mouvement)
            if (damageContainer != null)
            {
                damageContainer.DOKill();

                Sequence damageSeq = DOTween.Sequence();

                // Pop explosif
                damageSeq.Append(damageContainer.DOScale(1.8f, 0.15f).SetEase(Ease.OutBack));
                
                // Retour élastique
                damageSeq.Append(damageContainer.DOScale(1f, 0.4f).SetEase(Ease.OutElastic));
            }
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
                damageText.text = $"{totalDamage}";

            int combo = 0; 
            if (comboText != null)
            {
                comboText.text = $"{combo}";

                if (combo > 0 && comboContainer != null)
                {
                    comboContainer.DOKill();

                    float intensity = Mathf.Min(combo / 5f, 3f);

                    Sequence comboSeq = DOTween.Sequence();

                    // Pop explosif
                    comboSeq.Append(comboContainer.DOScale(2.5f * intensity, 0.12f).SetEase(Ease.OutQuad));

                    // Punch scale (reste en place)
                    comboSeq.Append(comboContainer.DOPunchScale(Vector3.one * 0.8f * intensity, 0.5f, 15, 1f));

                    // Retour élastique
                    comboSeq.Append(comboContainer.DOScale(1f, 0.4f).SetEase(Ease.OutElastic));
                }
            }

            Debug.Log($"[{currentPlayer.gameObject.name}] Dégâts: {totalDamage}, Combo: {combo}");
        }

        private void UpdateMoneyDisplay()
        {
            if (currentPlayer == null) return;

            if (moneText != null)
                moneText.text = $"Argent: {currentPlayer.CurrentMoney}$";
        }

        public void Disconnect()
        {
            if (currentPlayer != null)
            {
                currentPlayer.OnBeginTurn -= OnNewTurnBegins;
                currentPlayer.OnEndTurn -= OnNewTurnEnds;
                currentPlayer.OnChangeHealth -= OnHealthChanged;
                currentPlayer.OnChangedDamage -= OnDamageChanged;
                currentPlayer.OnChangedMoney -= OnMoneyChanged;
            }

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
                healthText.text = $"{hp}";
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
                Debug.Log($"[{currentPlayer?.gameObject.name}] Affiche zone dégâts");
            }

            if (comboText != null)
                comboText.gameObject.SetActive(true);
        }

        public void HideDamageText()
        {
            if (damageText != null)
            {
                damageText.gameObject.SetActive(false);
                Debug.Log($"[{currentPlayer?.gameObject.name}] Cache dégâts");
            }

            if (comboText != null)
                comboText.gameObject.SetActive(false);
        }
    }
}
