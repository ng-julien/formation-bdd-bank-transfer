namespace Training.BDD
{
    public delegate StateTransfer Credit(uint amount);

    public class DebitBankAccount : BankAccount
    {
        private uint authorizedLimit;

        public DebitBankAccount(uint initialSold)
            : this(initialSold, 400)
        {
        }

        public DebitBankAccount(uint initialSold, uint authorizedLimit)
            : base(initialSold)
        {
            this.authorizedLimit = authorizedLimit;
        }

        public StateTransfer Debit(uint amountToBeDebited, Credit credit)
        {
            if (ExceedTheAllowedLimit(amountToBeDebited, this.authorizedLimit))
            {
                return StateTransfer.LimitExceed;
            }

            if (IsOutOfProvision(this.Sold, amountToBeDebited, out var soldAfterDebit))
            {
                return StateTransfer.OutOfProvision;
            }

            var stateTransfer = credit(amountToBeDebited);
            if (stateTransfer == StateTransfer.Success)
            {
                this.Sold = soldAfterDebit.GetValueOrDefault();
            }

            return stateTransfer;
        }

        public void DefineAuthorizedLimit(uint newAuthorizedLimit)
        {
            this.authorizedLimit = newAuthorizedLimit;
        }

        private static bool ExceedTheAllowedLimit(uint amountToBeDebited, uint authorizedLimit)
        {
            return amountToBeDebited > authorizedLimit;
        }

        private static bool IsOutOfProvision(uint sold, uint amountToBeDebited, out uint? soldAfterDebit)
        {
            soldAfterDebit = null;
            int result = (int)sold - (int)amountToBeDebited;
            if (result < 0)
            {
                return true;
            }

            soldAfterDebit = (uint)result;
            return false;
        }
    }
}