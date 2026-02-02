using System;
using System.Collections;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.PointCards;
using UnityEngine;

namespace Gameplay.CardSystem
{
    public abstract class CardPlayer : MonoBehaviour
    {
        public Hand MainHand { get; private set;}
        public Hand HandDefence { get; private set;}
        public Hand HandAttack { get; private set;}
        
        [field: SerializeField]
        public int MaxHealth { get; private set; } = 100; 
        [field: SerializeField]
        public int CurrentHealth { get; private set; } = 100;
        
        public abstract IEnumerator DrawPlayerCards(TurnManager manager);

        public abstract IEnumerator PlayCards(TurnManager manager);

        public abstract IEnumerator EndTurn(TurnManager manager);

        public void PrepareForGame(TurnManager turnManager)
        {
            MainHand = new Hand();
            HandDefence = new Hand();
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