namespace Training.BDD.Tests
{
    using System;

    using NUnit.Framework;

    using Shouldly;

    public class CreditBankAccountShould
    {
        [Test]
        public void AddAmountToSoldWhenCallCredit()
        {
            uint expectedSold = 400;
            uint amountToBeDebited = 100;
            var creditBankAccount = new CreditBankAccount(300);

            creditBankAccount.Credit(amountToBeDebited);

            creditBankAccount.Sold.ShouldBe(expectedSold);
        }

        [Test]
        public void ReturnSuccessStateTransferWhenCallCredit()
        {
            uint amountToBeDebited = 100;
            var creditBankAccount = new CreditBankAccount(300);

            var actualState = creditBankAccount.Credit(amountToBeDebited);

            actualState.ShouldBe(StateTransfer.Success);
        }
    }
}