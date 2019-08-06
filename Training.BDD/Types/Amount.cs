namespace Training.BDD.Types
{
    using LanguageExt;
    using LanguageExt.ClassInstances;

    public class Amount : FloatType<Amount, TDecimal, decimal>
    {
        static Amount()
        {
            Zero = new Amount(0m);
        }

        public Amount(decimal value)
            : base(value)
        {
        }

        public static Amount Zero { get; }
    }
}