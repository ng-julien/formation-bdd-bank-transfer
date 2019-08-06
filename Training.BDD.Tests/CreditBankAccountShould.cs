namespace Training.BDD.Tests
{
    using System;

    using NUnit.Framework;

    using Shouldly;

    using Types;

    public class CreditBankAccountShould
    {
        [Test]
        public void AddAmountToSoldWhenCallCredit()
        {
            uint expectedSold = 400;
            Amount amountToBeDebited = new Amount(100);
            var creditBankAccount = new CreditBankAccount(new Amount(300));

            var actual = creditBankAccount.Credit(amountToBeDebited);

            actual.Balance.Value.ShouldBe(expectedSold);
        }

        [Test]
        public void ReturnSuccessStateTransferWhenCallCredit()
        {
            var amountToBeDebited = new Amount(100);
            var creditBankAccount = new CreditBankAccount(new Amount(300));

            var actual = creditBankAccount.Credit(amountToBeDebited);

            actual.TransferState.ShouldBe(TransferState.Success);
        }
    }
}