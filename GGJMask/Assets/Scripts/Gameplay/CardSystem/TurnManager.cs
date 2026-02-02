using UnityEngine;

namespace Gameplay.CardSystem
{
    public enum Phase
    {
        Start,
        DrawPhase,
        SelectPhase, 
        CaculateScore, 
        ApplyDamage, 
        EndPhase, 
        
    }
    public class TurnManager : MonoBehaviour
    {
        private PlayerTurn playerTurn;
        private IATurn iaturn; 
        [SerializeField] private UtilitiesCard utilitiesCard;
        
    }
}