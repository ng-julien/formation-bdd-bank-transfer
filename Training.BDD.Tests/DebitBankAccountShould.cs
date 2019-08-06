namespace Training.BDD.Tests
{
    using Moq;

    using NUnit.Framework;

    using Shouldly;

    using Types;

    public class DebitBankAccountShould
    {
        [Test]
        public void ReturnCreditStateOfTransferWhenCallDebitAndDebitAccountHaveProvisionAndAmountNotExceededLimit()
        {
            var amountToBeDebited = new Amount(100);
            var creditMock = new Mock<Credit>();
            creditMock.Setup(credit => credit(amountToBeDebited))
                      .Returns(new CreditBankAccount(new Amount(0), TransferState.Fail)).Verifiable();
            var debitBankAccount = new DebitBankAccount(new Amount(300));

            var actual = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            creditMock.Verify();
            actual.TransferState.ShouldBe(TransferState.Fail);
        }

        [Test]
        public void
            ReturnCreditStateOfTransferWhenCallDebitAndDebitAccountHaveProvisionAndAmountNotExceededLimitAndCreditIsSuccess()
        {
            var amountToBeDebited = new Amount(100);
            var creditMock = new Mock<Credit>();
            creditMock.Setup(credit => credit(amountToBeDebited))
                      .Returns(new CreditBankAccount(new Amount(0), TransferState.Success)).Verifiable();
            var debitBankAccount = new DebitBankAccount(new Amount(300));

            var actual = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            creditMock.Verify();
            actual.TransferState.ShouldBe(TransferState.Success);
        }

        [Test]
        public void ReturnLimitTransferIsEqual400ByDefault()
        {
            var amountToBeDebited = new Amount(401);
            var creditMock = new Mock<Credit>();
            var debitBankAccount = new DebitBankAccount(new Amount(600));

            var actual = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            actual.TransferState.ShouldBe(TransferState.LimitExceed);
            creditMock.Verify(credit => credit(It.IsAny<Amount>()), Times.Never);
        }

        [Test]
        public void ReturnOutOfProvisionWhenFutureSoldIsLessThanZero()
        {
            var amountToBeDebited = new Amount(301);
            var creditMock = new Mock<Credit>();
            var debitBankAccount = new DebitBankAccount(new Amount(300));

            var actual = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            actual.TransferState.ShouldBe(TransferState.OutOfProvision);
            creditMock.Verify(credit => credit(It.IsAny<Amount>()), Times.Never);
        }

        [Test]
        public void SubtractSoldWhenCallDebitAndDebitAccountHaveProvisionAndAmountNotExceededLimitAndCreditIsSuccess()
        {
            var expectedSold = new Amount(200);
            var amountToBeDebited = new Amount(100);
            var creditMock = new Mock<Credit>();
            creditMock.Setup(credit => credit(amountToBeDebited))
                      .Returns(new CreditBankAccount(Amount.Zero, TransferState.Success));
            var debitBankAccount = new DebitBankAccount(new Amount(300));

            var actual = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            actual.Balance.Value.ShouldBe(expectedSold.Value);
        }

        [Test]
        public void UnChangeSoldWhenCreditHasFailed()
        {
            var expectedSold = new Amount(300);
            var amountToBeDebited = new Amount(100);
            var creditMock = new Mock<Credit>();
            creditMock.Setup(credit => credit(amountToBeDebited))
                      .Returns(new CreditBankAccount(Amount.Zero, TransferState.Fail));
            var debitBankAccount = new DebitBankAccount(new Amount(300));

            var actual = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            actual.Balance.Value.ShouldBe(expectedSold.Value);
        }
    }
}