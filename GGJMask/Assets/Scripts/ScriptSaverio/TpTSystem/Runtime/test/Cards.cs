using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RunTime.TpTSystem
{
    public class Cards : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [Header("Rotation followSpeed")] [SerializeField]
        private float rotateSpeedLimit = 1f;
        [SerializeField] private float rotatebreakLimit = 2f;
        [SerializeField] private float moveSpeedLimit = 600;
        
        public bool isDragging = false;
        public bool selected = false;
        public float selectionOffset = 50;
        
        
        [HideInInspector] public UnityEvent<Cards> BeginDragEvent;
        [HideInInspector] public UnityEvent<Cards> EndDragEvent;
        
        private float angleY;
        private Canvas canvas;
        private Image imageComponent;
        private Vector3 offset;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            imageComponent = GetComponent<Image>();
            
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
            Debug.Log("begin drag");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            EndDragEvent.Invoke(this);

            Image canvasGroup = GetComponent<Image>();
            canvasGroup.raycastTarget = true;
            imageComponent.raycastTarget = true;

            transform.eulerAngles = Vector3.zero;
            Debug.Log("yo");

        }
        public void OnDrag(PointerEventData eventData)
        {
        }
        
        public int ParentIndex()
        {
            return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
        }
    }
}