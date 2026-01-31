using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RunTime.TpTSystem
{
    public class Cards : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
    {
        [Header("Rotation followSpeed")] [SerializeField]
        private float rotateSpeedLimit = 1f;
        [SerializeField] private float rotatebreakLimit = 2f;
        [SerializeField] private float moveSpeedLimit = 600;
        
        public bool isDragging = false;
        public bool selected = false;
        public float selectionOffset = 50;
        
        [Header("Visual")]
        public CardVisual cardVisual;
        
        
        [HideInInspector] public UnityEvent<Cards> BeginDragEvent;
        [HideInInspector] public UnityEvent<Cards> EndDragEvent;
        [HideInInspector] public UnityEvent<Cards> EnterEvent;
        [HideInInspector] public UnityEvent<Cards> ExitEvent;
        
        private float angleY;
        private Canvas canvas;
        private Image imageComponent;
        public bool wasDragged = false;
        private Vector3 offset;
        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            imageComponent = GetComponent<Image>();
            cardVisual =  GetComponent<CardVisual>();
            
        }
        private void Update()
        {
            if (isDragging)
            {
                Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
                mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
                transform.position = Vector2.MoveTowards(transform.position, mouseScreenPos, moveSpeedLimit * Time.deltaTime);

                float mouseX = Mouse.current.delta.value.x / rotatebreakLimit;
                angleY += mouseX * rotateSpeedLimit;
                angleY = Mathf.Clamp(angleY, -20, 20);
				
                transform.rotation = Quaternion.Euler(0, 0, angleY);
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            BeginDragEvent.Invoke(this);
            
            Image canvasGroup = GetComponent<Image>();
            canvasGroup.raycastTarget = false;
            imageComponent.raycastTarget = false;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
            offset = mousePosition - (Vector2)transform.position;
            wasDragged = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            EndDragEvent.Invoke(this);

            Image canvasGroup = GetComponent<Image>();
            canvasGroup.raycastTarget = true;
            imageComponent.raycastTarget = true;

            transform.eulerAngles = Vector3.zero;
            wasDragged = false;

        }
        public void OnDrag(PointerEventData eventData)
        {
        }
        
        public int ParentIndex()
        {
            return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EnterEvent.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ExitEvent.Invoke(this);
        }
    }
}