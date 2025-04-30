
using BankWebApi.Domain.AccountModel;
using FluentAssertions;
using NUnit.Framework;
 

namespace BankWebApi.Tests.UnitTests.Aggregates
{
    [Category("unit")]
    public class AccountIdTests
    {

        [Fact]
        public void ManuallyCreatedIsOk()
        {
            // Arrange
            const string value = "account-7f08993b-d654-488f-aeb9-76b32bdab9fe";

            // Act
            var testId = AccountId.With(value);

            // Test
            testId.Value.Should().Be(value);
        }

        [Fact]
        public void CreatedIsDifferent()
        {
            // Act
            var id1 = AccountId.New;
            var id2 = AccountId.New;

            // Assert
            id1.Value.Should().NotBe(id2.Value);
        }
        [Fact]
        public void SameIdsAreEqual()
        {
            // Arrange
            const string value = "account-7f08993b-d654-488f-aeb9-76b32bdab9fe";
            
            var id1 = AccountId.With(value);
            var id2 = AccountId.With(value);

            // Assert
            id1.Equals(id2).Should().BeTrue();
            (id1 == id2).Should().BeTrue();
        }

        [Fact]
        public void DifferentAreNotEqual()
        {
            // Arrange
            var id1 = AccountId.With("account-7f08993b-d654-488f-aeb9-76b32bdab9fe");
            var id2 = AccountId.With("account-43dcce20-908e-438b-b672-d039186d991e");

            // Assert
            id1.Equals(id2).Should().BeFalse();
            (id1 == id2).Should().BeFalse();
        }
    }
}
