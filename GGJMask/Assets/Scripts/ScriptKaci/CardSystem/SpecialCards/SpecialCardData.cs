using UnityEngine;

namespace ScriptKaci
{
    [CreateAssetMenu( menuName = "Card/cardData", order = 0)]
    public class SpecialCardData : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }
     
       
        
        
    }
}