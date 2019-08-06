namespace Training.BDD
{
    using Types;

    public delegate CreditBankAccount Credit(Amount amount);

    public class DebitBankAccount : BankAccount
    {
        private readonly Amount authorizedLimit;

        public DebitBankAccount(Amount balance, TransferState transferState = TransferState.None)
            : this(balance, transferState, new Amount(400))
        {
        }

        private DebitBankAccount(Amount balance, TransferState transferState, Amount authorizedLimit)
            : base(balance)
        {
            this.TransferState = transferState;
            this.authorizedLimit = authorizedLimit;
        }

        public TransferState TransferState { get; }

        public DebitBankAccount Debit(Amount debit, Credit credit)
        {
            if (ExceedTheAllowedLimit(debit, this.authorizedLimit))
            {
                return new DebitBankAccount(this.Balance, TransferState.LimitExceed);
            }

            var balance = this.Balance.Subtract(debit);
            if (IsOutOfProvision(balance))
            {
                return new DebitBankAccount(this.Balance, TransferState.OutOfProvision);
            }

            var creditBankAccount = credit(debit);
            if (creditBankAccount.TransferState == TransferState.Success)
            {
                return new DebitBankAccount(balance, TransferState.Success);
            }

            return new DebitBankAccount(this.Balance, creditBankAccount.TransferState);
        }

        public DebitBankAccount DefineAuthorizedLimit(Amount newLimit) =>
            new DebitBankAccount(this.Balance, this.TransferState, newLimit);

        private static bool ExceedTheAllowedLimit(Amount debit, Amount limit)
        {
            return debit > limit;
        }

        private static bool IsOutOfProvision(Amount balance)
        {
            return balance < Amount.Zero;
        }
    }
}