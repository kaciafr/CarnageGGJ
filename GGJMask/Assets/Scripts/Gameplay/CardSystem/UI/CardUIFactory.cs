using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.CardSystem.UI
{
    public class CardUIFactory : MonoBehaviour
    {
        [SerializeField] 
        private CardUI cardUIPrefab;
        
        
        private Dictionary<ICard, CardUI> cardUIDictionary = new Dictionary<ICard, CardUI>();


        public bool TryGetCardUI(ICard card, out CardUI cardUI)
        {
            if(cardUIDictionary.TryGetValue(card, out cardUI))
                return true;
            
            cardUI = Instantiate(cardUIPrefab, transform);
            cardUI.transform.localPosition = Vector3.zero;
            cardUIDictionary.Add(card, cardUI);
            
            cardUI.Connect(card);
            return true;
        }

        private void Update()
        {
            List<ICard> cards = cardUIDictionary.Keys.ToList();
            foreach (var card in cards)
            {
                CardUI ui = cardUIDictionary[card];
                if (ui.Slot == null)
                {
                    cardUIDictionary.Remove(card);
                    ui.Disconnect();

                    ui.transform.DOScale(0, .2f)
                        .OnComplete(() => Destroy(ui.gameObject));
                }
            }
        }
    }
}