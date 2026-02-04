using DG.Tweening;
using Gameplay.CardSystem.PointCards;
using Gameplay.CardSystem.SpecialCards;
using RunTime.TpTSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay.CardSystem.UI
{
    public class CardUI : MonoBehaviour, 
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IPointerEnterHandler,IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField] 
        private Image icon;
        [SerializeField]
        private TMP_Text scoreText;

        [SerializeField]
        private float rotateSpeedLimit = 1f;
        [SerializeField] 
        private float rotateBreakLimit = 2f;
        
        [SerializeField, Range(0, 250)]
        private float cardMovementSpeed;
        
        [SerializeField]
        private GameObject shadow;
        
        [Header("Parametre animation")] 
        [SerializeField]
        private float shake = 10;
        [SerializeField]
        private float scale = 1.2f;
        public ICard CurrentCard { get; private set; }
        public Transform Slot { get; private set; }
        
        public bool IsDragged { get; private set; }
        
        public bool IsSelected { get; private set; }
        
        public Vector3 DraggedPosition { get; private set; }
        
        public CardCollectionUI CollectionUI { get; private set; }

        private float angleY;
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
                    scoreText.text= pointCard.Score.ToString();
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
            CurrentCard = null;
        }

        private void Update()
        {
            
            Vector3 targetPosition;
            if (IsDragged)
                targetPosition = DraggedPosition;
            else if (Slot != null)
                targetPosition = Slot.position;
            else
                targetPosition = transform.position;
            
            Vector3 lerpPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cardMovementSpeed);
            
            Vector3 direction = lerpPosition - transform.position;
            /*
            float mouseX = direction.x / rotateBreakLimit;
            angleY += mouseX * rotateSpeedLimit;
            angleY = Mathf.Clamp(angleY, -20, 20);
				
            transform.rotation = Quaternion.Euler(0, 0, angleY);
            */
            transform.position = lerpPosition;
        }

        public void ClearSlot()
        {
            CollectionUI = null;
            Slot = null;
        }
        
        public void SetSlot(CardCollectionUI collectionUI, Transform slot)
        {
            CollectionUI = collectionUI;
            Slot = slot;
        }

        private void OnDestroy()
        {
            if(CurrentCard != null)
                Disconnect();
        }


        public void OnDrag(PointerEventData eventData)
        {
            if(CollectionUI == null || !CollectionUI.CanInteract)
                return;
            DraggedPosition = eventData.position;
            if(CollectionUI)
                CollectionUI.ProcessCardDrag(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(CollectionUI == null || !CollectionUI.CanInteract)
                return;
            IsDragged = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(CollectionUI == null || !CollectionUI.CanInteract)
                return;
            IsDragged = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
            if(CollectionUI == null || !CollectionUI.CanInteract)
                return;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(CollectionUI == null || !CollectionUI.CanInteract)
                return;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(CollectionUI == null || !CollectionUI.CanInteract)
                return;
            
            IsSelected = !IsSelected;
            
            if (IsSelected)
            {
                transform.DOKill();
                transform.DOScale(Vector3.one * 1.15f, .25f).SetEase(Ease.OutQuad);
            }
            else
            {
                transform.DOKill();
                transform.DOScale(Vector3.one, .25f).SetEase(Ease.OutQuad);
            }
        }
    }
}