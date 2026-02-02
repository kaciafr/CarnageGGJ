using System;
using Gameplay.CardSystem.PointCards;
using Gameplay.CardSystem.SpecialCards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Gameplay.CardSystem.UI
{
    public class CardUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image selectionOverlay;

        private Gameplay.CardSystem.SelectionManager selectionManager;

        public ICard CurrentCard { get; private set; }
        public bool IsSelected { get; private set; }
        public Gameplay.CardSystem.SelectionType SelectionType { get; private set; }

        public int CardIndex { get; private set; }

        public void Initialize(Gameplay.CardSystem.SelectionManager manager)
        {
            selectionManager = manager;
        }

        public void SetIndex(int index)
        {
            CardIndex = index;
        }

        public void Connect(ICard card)
        {
            CurrentCard = card;

            if (card is ScoreCard scoreCard)
            {
                if (icon != null && scoreCard.Icon != null)
                {
                    icon.sprite = scoreCard.Icon;
                    icon.enabled = true;
                }
                if (scoreText != null)
                {
                    scoreText.text = scoreCard.Score.ToString();
                    scoreText.enabled = true;
                }

                if (backgroundImage != null)
                {
                    backgroundImage.color = GetSuitColor(scoreCard.Suit);
                }
            }
            else if (card is SpecialCard specialCard)
            {
                if (icon != null && specialCard.Icon != null)
                {
                    icon.sprite = specialCard.Icon;
                    icon.enabled = true;
                }

                if (scoreText != null)
                {
                    scoreText.enabled = false;
                }

                if (backgroundImage != null)
                {
                    backgroundImage.color = Color.yellow;
                }
            }

            if (selectionOverlay != null)
            {
                selectionOverlay.enabled = false;
            }
        }

        private Color GetSuitColor(CardSuit suit)
        {
            switch (suit)
            {
                case CardSuit.Red: return Color.red;
                case CardSuit.Blue: return Color.blue;
                case CardSuit.Green: return Color.green;
                case CardSuit.Black: return Color.black;
                default: return Color.white;
            }
        }

        public void Disconnect()
        {
            CurrentCard = null;
            Deselect();
        }

        public void Select(Gameplay.CardSystem.SelectionType type)
        {
            IsSelected = true;
            SelectionType = type;

            if (selectionOverlay != null)
            {
                selectionOverlay.enabled = true;
                selectionOverlay.color = type == Gameplay.CardSystem.SelectionType.Attack
                    ? new Color(1f, 0f, 0f, 0.5f)  
                    : new Color(0f, 0f, 1f, 0.5f); 
            }
        }

        public void Deselect()
        {
            IsSelected = false;
            if (selectionOverlay != null)
            {
                selectionOverlay.enabled = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (selectionManager != null)
            {
                selectionManager.ToggleCardSelection(this);
            }
            else
            {
                Debug.LogWarning("SelectionManager non assigné sur CardUI!");
            }
        }
    }
}
