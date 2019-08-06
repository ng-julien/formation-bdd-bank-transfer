namespace Training.BDD
{
    using LanguageExt;

    using Types;

    public class BankAccount : Record<BankAccount>
    {
        protected BankAccount(Amount balance)
        {
            this.Balance = balance;
        }

        public Amount Balance { get; protected set; }
    }
}