using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace RunTime.TpTSystem
{
    public class CardCollectionUI : MonoBehaviour
    {
        public List<CardUI> Cards { get; private set; }
        
        [SerializeField]
        private TurnManager turnManager;
        [SerializeField]
        private CardUIFactory factory;
        [SerializeField]
        private Transform slotContainer;
        
        [SerializeField]
        private RectTransform slotPrefab;
        
        private Vector3 targetPosition;
        public CardCollection Collection { get; private set; }
        public bool CanInteract { get; set; }


        private void Awake()
        {
            Cards = new List<CardUI>();
            foreach (Transform t in slotContainer)
            {
                Destroy(t.gameObject);
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        public void Connect(CardCollection newCollection)
        {
            if(Collection != null)
                Disconnect();
            
            Collection = newCollection;
            Collection.OnCardAdded += OnCardAdded;
            Collection.OnCardRemoved += OnCardRemoved;
        }

        public void Disconnect()
        {
            if(Collection == null)
                return;
            
            Collection.OnCardAdded -= OnCardAdded;
            Collection.OnCardRemoved -= OnCardRemoved;
            Collection = null;
        }

        private void OnCardAdded(ICard card)
        {
            if (factory.TryGetCardUI(card, out CardUI cardUI))
            {
                RectTransform slot = Instantiate(slotPrefab, slotContainer);
                cardUI.SetSlot(this, slot);
                
                Cards.Add(cardUI);
            }

        }

        private void OnCardRemoved(ICard card)
        {
            if (factory.TryGetCardUI(card, out CardUI cardUI) && Cards.Remove(cardUI))
            {
                Destroy(cardUI.Slot.gameObject);
                cardUI.ClearSlot();
            }
        }

        
        public void ProcessCardDrag(CardUI draggedCardUI)
        {
            Transform draggedSlot = draggedCardUI.Slot;
            if(draggedSlot == null)
                return;
            
            if(!draggedSlot.IsChildOf(slotContainer))
                return;

            int draggedCardSlotIndex = draggedSlot.GetSiblingIndex();
            for (int i = 0; i < Cards.Count; i++)
            {
                CardUI cardUI = Cards[i];
                if(cardUI == draggedCardUI)
                    continue;
                
                if(cardUI.Slot == null)
                    continue;
                
                RectTransform cardUISlotTransform = (RectTransform)cardUI.Slot.transform;
                Vector3 selectedCardPosition = draggedCardUI.transform.position;

                int cardUISlotIndex = cardUISlotTransform.GetSiblingIndex();
                if (selectedCardPosition.x > cardUISlotTransform.position.x && draggedCardSlotIndex < cardUISlotIndex)
                    draggedSlot.SetSiblingIndex(cardUISlotIndex);

                if (selectedCardPosition.x < cardUISlotTransform.position.x  && draggedCardSlotIndex > cardUISlotIndex)
                    draggedSlot.SetSiblingIndex(cardUISlotIndex);
            }
        }
    }
}
