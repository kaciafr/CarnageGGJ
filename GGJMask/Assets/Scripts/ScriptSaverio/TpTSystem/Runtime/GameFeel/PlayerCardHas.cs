using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace RunTime.TpTSystem
{
    public class PlayerCardHas : MonoBehaviour
    {
        [SerializeField] private Cards selectedCard;
        [SerializeField] private GameObject handCard;
        [SerializeField] private GameObject bankCard;
        [SerializeField] private GameObject slots;
        
        [SerializeField] public List<Cards> cards;
        [SerializeField] private List<Transform> handCardsSlots;
        [SerializeField] private List<Transform> bankCardsSlots;
        [SerializeField] private List<Transform> allSlots;
        private Vector3[] corners = new Vector3[4];
        
        
        [SerializeField] private int maxSlotHand = 5;
        [SerializeField] private int maxSlotBank = 2;
        
        
        private Vector3 targetPosition;
        private bool isCrossing;
        private Transform originalParent;
        private Vector3 originalLocalPosition;

        private Slots originalSlot;

        private void Start()
        {

            for (int i = 0; i < maxSlotHand; i++)
            {
                GameObject slot = Instantiate(slots, handCard.transform);
                handCardsSlots.Add(slot.transform);
                
            }

            for (int i = 0; i < maxSlotBank; i++)
            {
                GameObject slot = Instantiate(slots, bankCard.transform);
                bankCardsSlots.Add(slot.transform);
            }
            
            for (int i = 0; i < maxSlotHand; i++)
            {
                Cards card = Instantiate(selectedCard, handCardsSlots[i]);
                card.name = "Card_" + i;
                card.BeginDragEvent.AddListener(BeginDrag);
                card.EndDragEvent.AddListener(EndDrag);
                card.transform.localPosition = Vector3.zero;
                cards.Add(card);
            }
            
            allSlots.AddRange(handCardsSlots);
            allSlots.AddRange(bankCardsSlots);
            
        }
        
        private Slots GetHoveredSlot()
        {
            foreach (Transform t in allSlots)
            {
                Slots slot = t.GetComponent<Slots>();
                if (slot != null && slot.hovering)
                    return slot;
            }
            return null;
        }

        private void Update()
        {
            if (isCrossing)
                return;
            
            if(selectedCard == null)
                return;
            
            for (int i = 0; i < cards.Count; i++)
            {
                var cardTransform = cards[i].transform;
                
                RectTransform rectTransform = (RectTransform)cardTransform;
                rectTransform.GetWorldCorners(corners);
                
                float yMin = corners[0].y;
                float yMax = corners[1].y;

                Vector3 selectedCardPosition = selectedCard.transform.position;
               // Debug.Log($"{selectedCardPosition}, {yMin}, {yMax}");
                if (selectedCardPosition.y < yMin || selectedCardPosition.y > yMax)
                    continue;


                if (selectedCardPosition.x > cardTransform.position.x)
                {
                    if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                    {
                        originalSlot.ClearCard();
                        Swap(i);
                        break;
                    }
                }

                if (selectedCardPosition.x < cardTransform.position.x)
                {
                    if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                    {
                        originalSlot.ClearCard();
                        Swap(i);
                        break;
                    }
                }
            }
        }

        private void BeginDrag(Cards cards)
        {
            selectedCard = cards;
            originalSlot = cards.transform.parent.GetComponent<Slots>();
            
            originalSlot.ClearCard();

            originalParent = cards.transform.parent;
            originalLocalPosition = cards.transform.localPosition;
        }

        private void EndDrag(Cards cards)
        {
            if(selectedCard == null) return;
            Slots hoveredSlot = GetHoveredSlot();

            if (hoveredSlot != null && hoveredSlot.IsFree)
            {
                selectedCard.transform.SetParent(hoveredSlot.transform);
                selectedCard.transform.DOLocalMove(Vector3.zero, 0.12f).SetEase(Ease.OutBack);
                
                hoveredSlot.SetCard(selectedCard);
            }
            else
            {
                selectedCard.transform.SetParent(originalParent);
                selectedCard.transform.DOLocalMove(originalLocalPosition, 0.12f)
                    .SetEase(Ease.OutBack);

                if (originalSlot != null)
                    originalSlot.SetCard(selectedCard);
            }
            selectedCard = null;
        }
        
        void Swap(int index)
        {
            isCrossing = true;

            Transform focusedParent = selectedCard.transform.parent;
            Transform crossedParent = cards[index].transform.parent;
            
            Slots slotA = focusedParent.GetComponent<Slots>();
            Slots slotB = crossedParent.GetComponent<Slots>();
            
            slotA.ClearCard();
            slotB.ClearCard();
            
            cards[index].transform.SetParent(focusedParent);
            cards[index].transform.DOLocalMove(cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero, 0.12f).SetEase(Ease.OutBack);

            selectedCard.transform.SetParent(crossedParent);
            /*
            selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0, selectedCard.selectionOffset, 0) : Vector3.zero, 0.12f).SetEase(Ease.OutBack);
            */

            isCrossing = false;
        }
    }
}
