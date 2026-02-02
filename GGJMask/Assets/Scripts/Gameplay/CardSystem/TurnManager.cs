using System.Collections;
using Gameplay.CardSystem;
using RunTime.TpTSystem;
using RunTime.TpTSystem.Data;
using UnityEngine;
public enum TurnPhase
{
    None, 
    Start,
    DrawPhase , 
    MainPhase, 
    ResolvePhase,
    End
}

public class TurnManager : MonoBehaviour
{
  [SerializeField] private PlayerTurnBase playerTurnBase;
  [SerializeField] private IATurnBase iaturnBase;
  private PlayerTurn playerTurn;
  private IATurnBase iaturn;
  

  [SerializeField] private UtilitiesCards utilitiesCards;
  private TurnPhase currentPhase = TurnPhase.None;
  private bool isPlayerTurn = true;


  
  
 


  private void Start()
  {
      StartTurn(isPlayerTurn);
  }

  private void StartTurn(bool playerStarts)
  {
      isPlayerTurn = playerStarts;
      currentPhase = TurnPhase.Start;
      StartCoroutine(RunTurn());
  }

  private IEnumerator RunTurn()
  {
      yield return StartCoroutine(PhaseDrawPhase());
      yield return StartCoroutine(PhaseMainPhase());
      yield return StartCoroutine(PhaseResolve());
      yield return StartCoroutine(PhaseEnd());

  }
  
  private IEnumerator PhaseDrawPhase()
  {
      throw new System.NotImplementedException();
  }
  
  private IEnumerator PhaseMainPhase()
  {
      throw new System.NotImplementedException();
  }
  
  private IEnumerator PhaseResolve()
  {
      throw new System.NotImplementedException();
  }
  
  private IEnumerator PhaseEnd()
  {
      throw new System.NotImplementedException();
  }


  

 


 
}
