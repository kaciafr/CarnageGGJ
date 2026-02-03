using System;
using System.Collections;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.PointCards;
using UnityEngine;

namespace Gameplay.CardSystem
{
    public abstract class CardPlayer : MonoBehaviour
    {
        public event Action OnBeginTurn;
        public event Action OnEndTurn;
        public Hand MainHand { get; private set;}
        public Hand HandDefense { get; private set;}
        public Hand HandAttack { get; private set;}
        
        [field: SerializeField]
        public int MaxHealth { get; private set; } = 100; 
        [field: SerializeField]
        public int CurrentHealth { get; private set; } = 100;
        
        public bool IsDone { get; private set; }

        public virtual void PrepareTurn(TurnManager manager)
        {
            int cardToDraw = manager.CurrentTurn == 1 ? manager.Metrics.HandSize : manager.Metrics.AttackSize;
            
            HandDefense.TransferAll(MainHand);
            HandAttack.TransferAll(MainHand);
            for (int i = 0; i < cardToDraw; i++)
            {
                ICard card = manager.Deck.DrawCard();
                MainHand.AddCard(card);
            }
        }
        public virtual void BeginTurn(TurnManager manager)
        {
            IsDone = false;
            
            OnBeginTurn?.Invoke();
        }


        public virtual void EndTurn(TurnManager manager)
        {
            OnEndTurn?.Invoke();
        }
        
        public void SetIsDone() => IsDone = true;

        public void PrepareForGame(TurnManager turnManager)
        {
            MainHand = new Hand();
            HandDefense = new Hand();
            HandAttack = new Hand();
            
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0) 
                return;
            
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                CurrentHealth = 0;
        }
    }
}