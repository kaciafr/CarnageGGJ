using System;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.PointCards;
using Gameplay.CardSystem.SpecialCards;
using UnityEngine;
using UnityEngine.Pool;

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


        public static void TransferAll(this CardCollection from, CardCollection to)
        {
            using (ListPool<ICard>.Get(out var list))
            {
                list.AddRange(from.Cards);
                foreach (var card in list)
                    card.Transfer(from, to);
            }
        }

        public static void Transfer(this ICard card, CardCollection from, CardCollection to)
        {
            from.RemoveCard(card);
            to.AddCard(card);
        }
    }

}
