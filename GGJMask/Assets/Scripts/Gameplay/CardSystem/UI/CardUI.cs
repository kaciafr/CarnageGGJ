
using System;
using Gameplay.CardSystem.PointCards;
using Gameplay.CardSystem.SpecialCards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.CardSystem.UI
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text scoreText;

        public ICard CurrentCard { get; private set; }
        
        public void Connect(ICard card)
        {
            if(CurrentCard != null)
                Disconnect();
            
            CurrentCard = card;
            icon.sprite = card.Icon;
            switch (card)
            {
                case ScoreCard pointCard:
                    scoreText.gameObject.SetActive(true);
                    scoreText.text = pointCard.Score.ToString();
                    scoreText.color = pointCard.Color;
                    icon.color = pointCard.Color;
                    break;
                case SpecialCard specialCard:
                    scoreText.gameObject.SetActive(false);
                    icon.color = Color.black;
                    scoreText.color = Color.black;
                    break;
            }
            
        }

        public void Disconnect()
        {
            
        }

        private void OnDestroy()
        {
            if(CurrentCard != null)
                Disconnect();
        }
    }
}