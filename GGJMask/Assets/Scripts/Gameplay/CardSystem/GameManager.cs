using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.PointCards;
using Gameplay.CardSystem.SpecialCards;
using UnityEngine;

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
        private Deck maindeck;


        private void Start()
        {
            StartInitialize();
        }
        private void StartInitialize()
        {
            maindeck = new Deck();
            GenerateAllCards();
            maindeck.Shuffle();
            
            River = new River();
            
            PlayerTurn = new PlayerTurn();
            PlayerTurn.HandPlayer = new Hand(handSize);
            PlayerTurn.HandAttack = new Hand(attackSize);
            PlayerTurn.HandDefence = new Hand(defenseSize);

            IaTurn = new IATurn(); 
            IaTurn.HandPlayer = new Hand(handSize);
            IaTurn.HandAttack = new Hand(attackSize); 
            IaTurn.HandDefence = new Hand(defenseSize);

            DistributeCards(); 
            
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