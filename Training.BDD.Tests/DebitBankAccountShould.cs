namespace Training.BDD.Tests
{
    using Moq;

    using NUnit.Framework;

    using Shouldly;

    public class DebitBankAccountShould
    {
        [Test]
        public void ReturnCreditStateOfTransferWhenCallDebitAndDebitAccountHaveProvisionAndAmountNotExceededLimit()
        {
            const uint amountToBeDebited = 100;
            var creditMock = new Mock<Credit>();
            creditMock.Setup(credit => credit(amountToBeDebited))
                      .Returns(StateTransfer.Fail).Verifiable();
            var debitBankAccount = new DebitBankAccount(300);

            var actualState = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            creditMock.Verify();
            actualState.ShouldBe(StateTransfer.Fail);
        }

        [Test]
        public void
            ReturnCreditStateOfTransferWhenCallDebitAndDebitAccountHaveProvisionAndAmountNotExceededLimitAndCreditIsSuccess()
        {
            const uint amountToBeDebited = 100;
            var creditMock = new Mock<Credit>();
            creditMock.Setup(credit => credit(amountToBeDebited))
                      .Returns(StateTransfer.Success).Verifiable();
            var debitBankAccount = new DebitBankAccount(300);

            var actualState = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            creditMock.Verify();
            actualState.ShouldBe(StateTransfer.Success);
        }

        [Test]
        public void ReturnLimitTransferIsEqual400ByDefault()
        {
            const uint amountToBeDebited = 401;
            var creditMock = new Mock<Credit>();
            var debitBankAccount = new DebitBankAccount(600);

            var actualState = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            actualState.ShouldBe(StateTransfer.LimitExceed);
            creditMock.Verify(credit => credit(It.IsAny<uint>()), Times.Never);
        }

        [Test]
        public void ReturnOutOfProvisionWhenFutureSoldIsLessThanZero()
        {
            const uint amountToBeDebited = 301;
            var creditMock = new Mock<Credit>();
            var debitBankAccount = new DebitBankAccount(300);

            var actualState = debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            actualState.ShouldBe(StateTransfer.OutOfProvision);
            creditMock.Verify(credit => credit(It.IsAny<uint>()), Times.Never);
        }

        [Test]
        public void SubtractSoldWhenCallDebitAndDebitAccountHaveProvisionAndAmountNotExceededLimitAndCreditIsSuccess()
        {
            const uint expectedSold = 200;
            const uint amountToBeDebited = 100;
            var creditMock = new Mock<Credit>();
            creditMock.Setup(credit => credit(amountToBeDebited))
                      .Returns(StateTransfer.Success);
            var debitBankAccount = new DebitBankAccount(300);

            debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            debitBankAccount.Sold.ShouldBe(expectedSold);
        }

        [Test]
        public void UnChangeSoldWhenCreditHasFailed()
        {
            const uint expectedSold = 300;
            const uint amountToBeDebited = 100;
            var creditMock = new Mock<Credit>();
            creditMock.Setup(credit => credit(amountToBeDebited))
                      .Returns(StateTransfer.Fail);
            var debitBankAccount = new DebitBankAccount(300);

            debitBankAccount.Debit(amountToBeDebited, creditMock.Object);

            debitBankAccount.Sold.ShouldBe(expectedSold);
        }
    }
}