using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RunTime.TpTSystem 
{
    public class BankCard : MonoBehaviour,IPointerEnterHandler

    {
    [Header("Cartes and slots")] [SerializeField]
    private Card selectCard;

    [SerializeField] private GameObject bankCard;
    [SerializeField] private GameObject slotPrefab;

    public List<Card> bankCards;
    public List<Transform> bankSlots;

    private List<Transform> slots;
    private int maxSlots = 0;
    private HandCardHolder handCardHolder;
    private bool isCrossing = false;

    void Update()
    {
        if (selectCard == null) return;

        if (isCrossing) return;

        for (int i = 0; i < bankCards.Count; i++)
        {

            if (selectCard.transform.position.x > bankCards[i].transform.position.x)
            {
                if (selectCard.ParentIndex() < bankCards[i].ParentIndex())
                {
                    Swap(i);
                    Card card = Instantiate(selectCard, bankCards[i].transform);
                    bankCards.Add(card);
                    break;
                }
            }

            if (selectCard.transform.position.x < bankCards[i].transform.position.x)
            {
                if (selectCard.ParentIndex() > bankCards[i].ParentIndex())
                {
                    Swap(i);
                    bankCards.RemoveAt(i);
                    break;
                }
            }
        }

    }
    

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectCard.transform.parent;
        Transform crossedParent = bankCards[index].transform.parent;

        bankCards[index].transform.SetParent(focusedParent);
        bankCards[index].transform.localPosition = bankCards[index].selected
            ? new Vector3(0, bankCards[index].selectionOffset, 0)
            : Vector3.zero;
        selectCard.transform.SetParent(crossedParent);

        isCrossing = false;

    }



    private void BeginDrag(Card cards)
    {
        selectCard = cards;

    }

    private void EndDrag(Card cards)
    {
        Vector3 targetPosition = selectCard.transform.localPosition;
        targetPosition = Vector3.zero;
        bool tweenCardReturn = true;

        selectCard.transform.DOLocalMove(targetPosition, 0.12f).SetEase(Ease.OutBack);


        selectCard = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        Debug.Log("OnPointerEnter");
        
        if (maxSlots <= 1)
        {
            maxSlots++;
            for (int i = 0; i < maxSlots; i++)
            {
                GameObject slot = Instantiate(slotPrefab, bankCard.transform);
                slot.name = "BankSlot_" + i;

                Card card = Instantiate(selectCard, slot.transform);
                card.name = "Card_" + (i + 1);
                card.transform.localPosition = Vector3.zero;

                card.BeginDragEvent.AddListener(BeginDrag);
                card.EndDragEvent.AddListener(EndDrag);
                return;
            }
        }
        else
        {
            Debug.Log("No More Slots");
        }
    }

    public bool isAcceptable(Card card)
    {
        return transform.childCount<maxSlots;
    }

    public void AcceptDrop(Card card)
    {
        Debug.Log("AcceptDrop");
        card.transform.SetParent(transform);
        card.transform.localPosition = Vector3.zero;
    }
    }
}

