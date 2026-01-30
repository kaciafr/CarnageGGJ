using System;
using System.Collections.Generic;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.PointCards;
using Gameplay.CardSystem.SpecialCards;
using Gameplay.CardSystem.UI;
using RunTime.TpTSystem;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Gameplay.CardSystem
{
    public class GameManager : MonoBehaviour
    {
        public PlayerTurn PlayerTurn {get; set; } 
        public IATurn IaTurn {get; set; } 
        
        public River River {get; set; }

        [SerializeField] private ScoreCardLibrary scoreCardLibrary; 
        [SerializeField] private SpecialCardData specialCardsData;

        [SerializeField] private int handSize = 5;
        [SerializeField] private int attackSize = 3;
        [SerializeField] private int defenseSize = 2;
        [SerializeField] private int riverSize = 5;
        [SerializeField] private int numberSpecial = 2;

        private Card card; 
        private Deck maindeck;
        [SerializeField]private Transform playerHandcontainer;
        [SerializeField]private Transform playerAttackcontainer; 
        [SerializeField] private Transform playerDefensecontainer;
        [SerializeField] private Transform playerRiver;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private SelectionManager selectionManager;
        

        
        
        private List<CardUI> playerCardUI = new List<CardUI>();



        private void Start()
        {
            StartInitialize();
        }

        private void Update()
        {
            
        }
        private void StartInitialize()
        {
            
            PlayerTurn = new PlayerTurn();
            PlayerTurn.HandPlayer = new Hand(handSize);
            PlayerTurn.HandAttack = new Hand(attackSize);
            PlayerTurn.HandDefence = new Hand(defenseSize);
            
            
            River = new River();

            IaTurn = new IATurn(); 
            IaTurn.HandPlayer = new Hand(handSize);
            IaTurn.HandAttack = new Hand(attackSize); 
            IaTurn.HandDefence = new Hand(defenseSize);
            
            maindeck = new Deck();
            GenerateAllCards();
            maindeck.Shuffle();
            DistributeCards();

            DisplayPlayerHand();
            DisplayRiverHand();
            
        }


        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerTurn.HandPlayer.TransferTo(PlayerTurn.HandAttack, 3);
                Debug.Log($"vous venez de transferer {PlayerTurn.HandAttack}");
                DisplayPlayerHand();
                DisplayAttack();

            }

        }


        private void DisplayPlayerHand()
        {
            foreach (Transform child in playerHandcontainer)
            {
                Destroy(child.gameObject);
            }
            playerCardUI.Clear();

            foreach (var card in PlayerTurn.HandPlayer.GetCards())
            {
                if (card == null) continue;

                GameObject cardObj = Instantiate(cardPrefab, playerHandcontainer);
                CardUI cardUI = cardObj.GetComponent<CardUI>();
        
                if (cardUI != null)
                {
                    cardUI.Connect(card);
                    playerCardUI.Add(cardUI);
                }
            }
        }

        private void DisplayRiverHand()
        {
            foreach (Transform child  in playerRiver )
            {
                Destroy(child.gameObject);
            }

            foreach (var card in River.GetCards())
            {
                if (card == null) continue;
                GameObject cardObj = Instantiate(cardPrefab, playerRiver);
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Connect(card);
                    playerCardUI.Add(cardUI);
                }
            }
            
            
        }

        private void DisplayAttack()
        {
            foreach (Transform child  in playerAttackcontainer )
            {
                Destroy(child.gameObject);
            }

            foreach (var card in PlayerTurn.HandAttack.GetCards())
            {
                if (card == null) continue;
                GameObject cardObj = Instantiate(cardPrefab, playerAttackcontainer);
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Connect(card);
                    playerCardUI.Add(cardUI);
                }
            }
        }
        
        private void DisplayDefense()
        {
            foreach (Transform child  in playerDefensecontainer )
            {
                Destroy(child.gameObject);
            }

            foreach (var card in PlayerTurn.HandDefence.GetCards())
            {
                if (card == null) continue;
                GameObject cardObj = Instantiate(cardPrefab, playerDefensecontainer);
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Connect(card);
                    playerCardUI.Add(cardUI);
                }
            }
        }

        private void GenerateAllCards()
        {
            foreach (CardValue value in System.Enum.GetValues(typeof(CardValue)))
            {
                foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
                {
                    ScoreCard scoreCard = new ScoreCard(scoreCardLibrary,value, suit ); 
                    maindeck.AddCard(scoreCard);
                } 
            }
            if (specialCardsData != null)
            {
                for (int i = 0; i < numberSpecial; i++)
                {
                    SpecialCard specialCard = new SpecialCard(specialCardsData);
                    maindeck.AddCard(specialCard);
                }
            }

            Debug.Log($"{maindeck.Count} cards generated");
            
        }

        private void DistributeCards()
        {
            foreach (var card in maindeck.DrawCards(handSize))
            {
                if (card != null)
                    PlayerTurn.HandPlayer.AddCard(card);
            }

            foreach (var card in maindeck.DrawCards(handSize))
            {
                if (card != null)
                    IaTurn.HandPlayer.AddCard(card);
            }

            foreach (var card in maindeck.DrawCards(riverSize))
            {
                if (card != null)
                    River.AddCard(card);
            }
            
            Debug.Log($"{maindeck.Count} cards distributed");
            Debug.Log($"{PlayerTurn.HandPlayer.Count} nb hand player");
            Debug.Log($"{IaTurn.HandPlayer.Count} nb hand player");
            Debug.Log($"{River.Count} nb riverCard"); 
            
        }
      
    }
}