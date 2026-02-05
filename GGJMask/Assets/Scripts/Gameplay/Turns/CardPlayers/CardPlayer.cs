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
        public event Action<int , int > OnChangeHealth;
        public event Action OnChangedMoney; 
        public event Action OnChangedDamage;
        
        public Hand MainHand { get; private set;}
        public Hand HandDefense { get; private set;}
        public Hand HandAttack { get; private set;}

        [field: SerializeField]
        public int MaxHealth { get; private set; } = 100; 
        [field: SerializeField]
        public int CurrentHealth { get; private set; } = 100;
        [field: SerializeField]
        public int CurrentMoney { get; private set; } = 100;
        public int MoneyToAdd { get; private set; } = 1000;

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
            
            OnChangedDamage?.Invoke(); // Reset damage display
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

            HandAttack.OnCardAdded += OnAttackHandChanged;
            HandAttack.OnCardRemoved += OnAttackHandChanged;

            CurrentHealth = MaxHealth;
            OnChangeHealth?.Invoke(CurrentHealth, 0);
            OnChangedDamage?.Invoke();
        }

        private void OnAttackHandChanged(ICard card)
        {
            OnChangedDamage?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0) 
                return;

            int oldHealth = CurrentHealth;
            int defense = 0;
            foreach (var card in HandDefense.cards)
            {
                if(card is ScoreCard scoreCard)
                    defense += scoreCard.Score;
            }

            defense *= 10;
            damage -= defense;
            if(damage <= 0)
                damage = 0;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                CurrentHealth = 0;

            int delta = CurrentHealth - oldHealth;
            OnChangeHealth?.Invoke(CurrentHealth, delta);
        }

        public void TakeDamageSimple(int damageSimple)
        {
            CurrentHealth -= damageSimple;
            if (CurrentHealth <= 0)
                CurrentHealth = 0;

            
    
         
    
           
        }

        public void AddMoney(int amount = -1)
        {
            int moneyToAdd = amount > 0 ? amount : MoneyToAdd;
            CurrentMoney += moneyToAdd;
            OnChangedMoney?.Invoke();
            Debug.Log($"{gameObject.name}: +{moneyToAdd}$ (Total: {CurrentMoney}$)");
        }

        private void OnDestroy()
        {
            if (HandAttack != null)
            {
                HandAttack.OnCardAdded -= OnAttackHandChanged;
                HandAttack.OnCardRemoved -= OnAttackHandChanged;
            }
        }

        public void ResetForNewGame()
        {
            throw new NotImplementedException();
        }
    }
}
