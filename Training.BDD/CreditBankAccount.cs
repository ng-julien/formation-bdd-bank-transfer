namespace Training.BDD
{
    public class CreditBankAccount : BankAccount
    {
        public CreditBankAccount(uint initialSold)
            : base(initialSold)
        {
        }

        public StateTransfer Credit(uint amount)
        {
            this.Sold += amount;
            return StateTransfer.Success;
        }
    }
}