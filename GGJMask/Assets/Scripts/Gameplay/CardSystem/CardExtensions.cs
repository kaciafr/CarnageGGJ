using System;
using System.Collections.Generic;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.PointCards;
using Gameplay.CardSystem.SpecialCards;
using Gameplay.CardSystem.UI;
using UnityEngine;

namespace Gameplay.CardSystem
{
    public static class CardExtensions
    {
        /*
        public CardLocalPlayer CardLocalPlayer { get; set; }
        public AICardPlayer AICardPlayer { get; set; }
        public River River { get; set; }

        [SerializeField] private ScoreCardLibrary scoreCardLibrary;
        [SerializeField] private SpecialCardData specialCardsData;
        [SerializeField] private int handSize = 5;
        [SerializeField] private int attackSize = 3;
        [SerializeField] private int defenseSize = 2;
        [SerializeField] private int riverSize = 5;
        [SerializeField] private int numberSpecial = 2;
        

        private Deck maindeck;
        [SerializeField] private Transform playerHandcontainer;
        [SerializeField] private Transform playerAttackcontainer;
        [SerializeField] private Transform playerDefensecontainer;
        [SerializeField] private Transform playerRiver;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private SelectionManager selectionManager;
        
     

        private List<CardUI> playerCardUI = new List<CardUI>();
        
        public static UtilitiesCards instance { get; private set; }


        private void Awake()
        {
            if (instance != null)
            {
                return; 
            }

            instance = this;
        }


        private void Start()
        {
            StartInitialize();
        }
        */


        public static int GetCollectionDamage(this CardCollection river, CardCollection collection, GameMetrics metrics)
        {
            int totalDamage = 0;
            totalDamage += GetDamageForSuit(CardSuit.Black, metrics, river, collection);
            totalDamage += GetDamageForSuit(CardSuit.Blue, metrics, river, collection);
            totalDamage += GetDamageForSuit(CardSuit.Green, metrics, river, collection);
            totalDamage += GetDamageForSuit(CardSuit.Red, metrics, river, collection);
            
            
            return totalDamage;
        }

        private static int GetDamageForSuit(CardSuit suit, GameMetrics metrics, params CardCollection[] collections)
        {
            int comboSize = 0;
            int damage = 0;

            foreach (CardCollection c in collections)
            {
                foreach (ICard card in c.Cards)
                {
                    if (card is ScoreCard scoreCard && scoreCard.Suit == suit)
                    {
                        damage += scoreCard.Score;
                        comboSize++;
                    }
                }
            }
            
            return damage * comboSize;
        }
        
        public static void FillCollectionWithAllCards(this CardCollection collection, GameMetrics gameMetrics)
        {
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    ScoreCard scoreCard = new ScoreCard(gameMetrics.ScoreCardLibrary, value, suit);
                    collection.AddCard(scoreCard);
                }
            }
            
            SpecialCardData[] specialCardsData = Resources.LoadAll<SpecialCardData>("SpecialCards");
            foreach (SpecialCardData specialCardData in specialCardsData)
            {
                for (int i = 0; i < gameMetrics.NumberSpecial; i++)
                {
                    SpecialCard specialCard = new SpecialCard(specialCardData);
                    collection.AddCard(specialCard);
                }
            }
            
            Debug.Log($"{collection.Count} cards generated");
        }
        
        
         public static void TransferToAttack(CardPlayer cardPlayer , GameMetrics gameMetrics)
        {
            for (int i = 0; i < gameMetrics.AttackSize; i++)
            {
                if (cardPlayer.MainHand.Count == 0)
                {
                    Debug.LogWarning("Plus de cartes dans la main pour transférer en défense");
                    break;
                }

                ICard card = cardPlayer.MainHand.RemoveCardAtIndex(0); 
                cardPlayer.HandAttack.AddCard(card);
            }
        }

        public static void TransferToDefense(CardPlayer cardPlayer, GameMetrics gameMetrics)
        {
            for (int i = 0; i < gameMetrics.DefenseSize; i++)
            {
                if (cardPlayer.MainHand.Count == 0)
                {
                    Debug.LogWarning("Plus de cartes dans la main pour transférer en défense");
                    break;
                }

                ICard card = cardPlayer.MainHand.RemoveCardAtIndex(0);
                cardPlayer.HandDefence.AddCard(card);
            }
        }
        
        
        
        
        
        
        /*
        private void DisplayPlayerHand()
        {
            if (playerHandcontainer != null)
            {
                foreach (Transform child in playerHandcontainer)
                {
                    Destroy(child.gameObject);
                }
            }

            playerCardUI.Clear();

            var cards = CardLocalPlayer.HandPlayer.GetCards();
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                if (card == null) continue;

                GameObject cardObj = Instantiate(cardPrefab, playerHandcontainer);
                CardUI cardUI = cardObj.GetComponent<CardUI>();

                if (cardUI != null)
                {
                    cardUI.Initialize(selectionManager);
                    cardUI.SetIndex(i); 
                    cardUI.Connect(card);
                    playerCardUI.Add(cardUI);
                }
            }
        }

        private void DisplayRiverHand()
        {
            if (playerRiver != null)
            {
                foreach (Transform child in playerRiver)
                {
                    Destroy(child.gameObject);
                }
            }

            var riverCards = River.GetCards();
            for (int i = 0; i < riverCards.Count; i++)
            {
                var card = riverCards[i];
                if (card == null) continue;
                GameObject cardObj = Instantiate(cardPrefab, playerRiver);
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Initialize(selectionManager);
                    cardUI.SetIndex(i);
                    cardUI.Connect(card);
                    playerCardUI.Add(cardUI);
                }
            }
        }

        private void DisplayAttack()
        {
            if (playerAttackcontainer != null)
            {
                foreach (Transform child in playerAttackcontainer)
                {
                    Destroy(child.gameObject);
                }
            }

            var attackCards = CardLocalPlayer.HandAttack.GetCards();
            for (int i = 0; i < attackCards.Count; i++)
            {
                var card = attackCards[i];
                if (card == null) continue;
                GameObject cardObj = Instantiate(cardPrefab, playerAttackcontainer);
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Initialize(selectionManager);
                    cardUI.SetIndex(i);
                    cardUI.Connect(card);
                    playerCardUI.Add(cardUI);
                }
            }
        }

        private void DisplayDefense()
        {
            if (playerDefensecontainer != null)
            {
                foreach (Transform child in playerDefensecontainer)
                {
                    Destroy(child.gameObject);
                }
            }

            var defCards = CardLocalPlayer.HandDefence.GetCards();
            for (int i = 0; i < defCards.Count; i++)
            {
                var card = defCards[i];
                if (card == null) continue;
                GameObject cardObj = Instantiate(cardPrefab, playerDefensecontainer);
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Initialize(selectionManager);
                    cardUI.SetIndex(i);
                    cardUI.Connect(card);
                    playerCardUI.Add(cardUI);
                }
            }
        }


        private void DistributeCards()
        {
            foreach (var card in maindeck.DrawCards(handSize))
            {
                if (card != null)
                    CardLocalPlayer.HandPlayer.AddCard(card);
            }

            foreach (var card in maindeck.DrawCards(handSize))
            {
                if (card != null)
                    AICardPlayer.HandPlayer.AddCard(card);
            }
            foreach (var card in maindeck.DrawCards(riverSize))
            {
                if (card != null)
                    River.AddCard(card);
            }

            Debug.Log($"{maindeck.Count} cards distributed");
            Debug.Log($"{CardLocalPlayer.HandPlayer.Count} nb hand player");
            Debug.Log($"{AICardPlayer.HandPlayer.Count} nb hand player");
            Debug.Log($"{River.Count} nb riverCard");
        }
        */

       
        

        
      
 
    }
    
}
