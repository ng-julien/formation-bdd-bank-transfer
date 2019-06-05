namespace Training.BDD.Tests
{
    using Shouldly;

    using TechTalk.SpecFlow;

    [Binding]
    public class GestionDesVirementsSteps
    {
        private CreditBankAccount creditBankAccount;

        private DebitBankAccount debitBankAccount;

        private StateTransfer stateTransfer;

        [Given(@"j'ai un compte cheque avec un solde de (.*)€​")]
        public void GivenJAiUnCompteChequeAvecUnSoldeDe(uint soldInitial)
        {
            this.debitBankAccount = new DebitBankAccount(soldInitial);
        }

        [Given(@"j'ai un compte épargne avec un solde de (.*)€​")]
        public void GivenJAiUnCompteEpargneAvecUnSoldeDe(uint soldInitial)
        {
            this.creditBankAccount = new CreditBankAccount(soldInitial);
        }

        [When(@"j'effectue un virement de (.*)€ du compte cheque vers le compte épargne")]
        public void WhenJEffectueUnVirementDeDuCompteChequeVersLeCompteEpargne(uint amount)
        {
            this.stateTransfer = this.debitBankAccount.Debit(amount, this.creditBankAccount.Credit);
        }

        [Then(@"le solde du compte cheque est (.*)€​")]
        public void ThenLeSoldeDuCompteChequeEst(uint soldFinal)
        {
            this.debitBankAccount.Sold.ShouldBe(soldFinal);
        }

        [Then(@"le solde du compte épargne est (.*)€​")]
        public void ThenLeSoldeDuCompteEpargneEst(uint soldFinal)
        {
            this.creditBankAccount.Sold.ShouldBe(soldFinal);
        }

        [Then(@"le virement est confirmé")]
        public void ThenLeVirementEstConfirme()
        {
            this.stateTransfer.ShouldBe(StateTransfer.Success);
        }

        [Then(@"le virement est refusé pour motif hors provision​")]
        public void ThenLeVirementEstRefusePourMotifHorsProvision()
        {
            this.stateTransfer.ShouldBe(StateTransfer.OutOfProvision);
        }

        [Given(@"la limite de virement est (.*)€​")]
        public void GivenLaLimiteDeVirementEst(uint limit)
        {
            this.debitBankAccount.DefineAuthorizedLimit(limit);
        }

        [Given(@"la limite de virement par défaut et de 400€​")]
        public void GivenLaLimiteDeVirementParDefautEtDe400()
        {
        }


        [Then(@"le virement est refusé pour motif plafond dépassé")]
        public void ThenLeVirementEstRefusePourMotifPlafondDepasse()
        {
            this.stateTransfer.ShouldBe(StateTransfer.LimitExceed);
        }
    }
}