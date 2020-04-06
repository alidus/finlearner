using System.Collections.Generic;

namespace FinOps.Deposit
{
    // Abstract deposit class.
    public abstract class AbstractDeposit
    {
        private float depositValue;
        
        protected float DepositValue
        {
            get => depositValue;
            set => depositValue = value;
        }
        
        private List<StatusEffect> statusEffects = new List<StatusEffect>();

        public List<StatusEffect> StatusEffects
        {
            get => statusEffects;
            set => statusEffects = value;
        }
    }
}