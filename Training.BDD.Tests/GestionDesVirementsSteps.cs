namespace Training.BDD.Tests
{
    using System.Linq;

    using LanguageExt;

    using Shouldly;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using TechTalk.SpecFlow.Assist.ValueRetrievers;

    using Types;

    [Binding]
    public class GestionDesVirementsSteps
    {
        private readonly ScenarioContext scenarioContext;

        public GestionDesVirementsSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        private CreditBankAccount creditBankAccount;

        private DebitBankAccount debitBankAccount;

        private DebitBankAccount actualDebitAccount;

        private CreditBankAccount actualCreditAccount;

        [Given(@"j'ai un compte cheque avec un solde de (.*)€​")]
        public void GivenJAiUnCompteChequeAvecUnSoldeDe(decimal soldInitial)
        {
            this.debitBankAccount = new DebitBankAccount(new Amount(soldInitial));
        }

        [Given(@"j'ai un compte épargne avec un solde de (.*)€​")]
        public void GivenJAiUnCompteEpargneAvecUnSoldeDe(uint soldInitial)
        {
            this.creditBankAccount = new CreditBankAccount(new Amount(soldInitial));
        }

        [When(@"j'effectue un virement de (.*)€ du compte cheque vers le compte épargne")]
        public void WhenJEffectueUnVirementDeDuCompteChequeVersLeCompteEpargne(uint amount)
        {
            this.actualDebitAccount = this.debitBankAccount.Debit(new Amount(amount), credit =>  this.actualCreditAccount = this.creditBankAccount.Credit(credit));
        }

        [Then(@"le solde du compte cheque est (.*)€​")]
        public void ThenLeSoldeDuCompteChequeEst(uint soldFinal)
        {
            this.actualDebitAccount.Balance.Value.ShouldBe(soldFinal);
        }

        [Then(@"le solde du compte épargne est (.*)€​")]
        public void ThenLeSoldeDuCompteEpargneEst(uint soldFinal)
        {
            if (!this.actualCreditAccount.IsNull())
            {
                this.actualCreditAccount.Balance.Value.ShouldBe(soldFinal);
            }
        }

        [Then(@"le virement est confirmé")]
        public void ThenLeVirementEstConfirme()
        {
            this.actualDebitAccount.TransferState.ShouldBe(TransferState.Success);
        }

        [Then(@"le virement est refusé pour motif hors provision​")]
        public void ThenLeVirementEstRefusePourMotifHorsProvision()
        {
            this.actualDebitAccount.TransferState.ShouldBe(TransferState.OutOfProvision);
        }

        [Given(@"la limite de virement est (.*)€​")]
        public void GivenLaLimiteDeVirementEst(decimal limit)
        {
            this.debitBankAccount.DefineAuthorizedLimit(new Amount(limit));
        }

        [Given(@"la limite de virement par défaut et de 400€​")]
        public void GivenLaLimiteDeVirementParDefautEtDe400()
        {
        }


        [Then(@"le virement est refusé pour motif plafond dépassé")]
        public void ThenLeVirementEstRefusePourMotifPlafondDepasse()
        {
            this.actualDebitAccount.TransferState.ShouldBe(TransferState.LimitExceed);
        }
    }
}