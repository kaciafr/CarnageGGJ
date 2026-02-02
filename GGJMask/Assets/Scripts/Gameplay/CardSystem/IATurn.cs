using Gameplay.CardSystem.Collections;

namespace Gameplay.CardSystem
{
    public class IATurn
    {
        public Hand HandPlayer { get; set;}
        public Hand HandDefence { get; set;}
        public Hand HandAttack { get; set;}

        public int currentHealth = 100; 
        
        public int MaxHealth = 100; 
        
        
    }
}