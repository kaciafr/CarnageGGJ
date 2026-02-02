using System;
using System.Collections.Generic;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.PointCards;
using Gameplay.CardSystem.SpecialCards;
using Gameplay.CardSystem.UI;
using UnityEngine;

namespace Gameplay.CardSystem
{
    public class UtilitiesCards : MonoBehaviour
    {
        public PlayerTurn PlayerTurn { get; set; }
        public IATurn IaTurn { get; set; }

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

            if (selectionManager != null)
            {
                selectionManager.utilitiesCardsRef = this;
            }

            DisplayPlayerHand();
            DisplayRiverHand();
            DisplayAttack();
            DisplayDefense();
        }
        

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

            var cards = PlayerTurn.HandPlayer.GetCards();
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

            var attackCards = PlayerTurn.HandAttack.GetCards();
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

            var defCards = PlayerTurn.HandDefence.GetCards();
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

        private void GenerateAllCards()
        {
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    ScoreCard scoreCard = new ScoreCard(scoreCardLibrary, value, suit);
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

        public void TransferSelectedToAttack(List<CardUI> selectedUIs)
        {
            if (selectedUIs == null || selectedUIs.Count == 0) return;

            var handCards = PlayerTurn.HandPlayer.GetCards();

            List<int> indices = new List<int>();
            foreach (var ui in selectedUIs)
            {
                if (ui == null || ui.CurrentCard == null)
                {
                    Debug.LogWarning("Selected CardUI invalide ");
                    continue;
                }

                int idx = handCards.IndexOf(ui.CurrentCard);
                if (idx >= 0)
                    indices.Add(idx);
                else
                    Debug.LogWarning("La carte sélectionnée n'est pas trouvée dans la main ");
            }

            indices.Sort((a, b) => b.CompareTo(a));

            foreach (int idx in indices)
            {
                ICard card = PlayerTurn.HandPlayer.RemoveCardAtIndex(idx);
                if (card != null)
                {
                    bool success = PlayerTurn.HandAttack.AddCard(card);
                    if (!success)
                    {
                        Debug.LogWarning("Echec ajout en attaque, remise dans la main");
                        PlayerTurn.HandPlayer.AddCard(card);
                    }
                }
                else
                {
                    Debug.LogWarning($"Aucune carte à l'index {idx} lors du transfert en attaque");
                }
            }

            DisplayPlayerHand();
            DisplayAttack();
            if (selectionManager != null) selectionManager.ClearSelection();

        }
        public void TransferSelectedToDefense(List<CardUI> selectedUIs)
        {
            if (selectedUIs == null || selectedUIs.Count == 0) return;

            var handCards = PlayerTurn.HandPlayer.GetCards();

            List<int> indices = new List<int>();
            foreach (var ui in selectedUIs)
            {
                if (ui == null || ui.CurrentCard == null)
                {
                    Debug.LogWarning("Selected CardUI invalide (null) — ignorée.");
                    continue;
                }

                int idx = handCards.IndexOf(ui.CurrentCard);
                if (idx >= 0)
                    indices.Add(idx);
                else
                    Debug.LogWarning("La carte sélectionnée n'est pas trouvée dans la main (ignorée).");
            }

            indices.Sort((a, b) => b.CompareTo(a));

            foreach (int idx in indices)
            {
                ICard card = PlayerTurn.HandPlayer.RemoveCardAtIndex(idx);
                if (card != null)
                {
                    bool success = PlayerTurn.HandDefence.AddCard(card);
                    if (!success)
                    {
                        Debug.LogWarning("Echec ajout en défense, remise dans la main");
                        PlayerTurn.HandPlayer.AddCard(card);
                    }
                }
                else
                {
                    Debug.LogWarning($"Aucune carte à l'index {idx} lors du transfert en défense");
                }
            }

            DisplayPlayerHand();
            DisplayDefense();
            if (selectionManager != null) selectionManager.ClearSelection();
        }
        
        



      
 
    }
    
}
