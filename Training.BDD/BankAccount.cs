namespace Training.BDD
{
    public class BankAccount
    {
        protected BankAccount(uint initialSold)
        {
            this.Sold = initialSold;
        }

        public uint Sold { get; protected set; }
    }
}