namespace Training.BDD
{
    using Types;

    public class CreditBankAccount : BankAccount
    {
        public TransferState TransferState { get; }

        public CreditBankAccount(Amount balance, TransferState transferState = TransferState.None)
            : base(balance)
        {
            this.TransferState = transferState;
        }

        public CreditBankAccount Credit(Amount amount) => new CreditBankAccount( this.Balance + amount, TransferState.Success);
    }
}