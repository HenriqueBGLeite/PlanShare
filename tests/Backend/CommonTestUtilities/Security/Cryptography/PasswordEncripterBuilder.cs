using Moq;
using PlanShare.Domain.Security.Cryptography;

namespace CommonTestUtilities.Security.Cryptography;

public class PasswordEncripterBuilder
{
    private readonly Mock<IPasswordEncripter> _mockPasswordEncripter;

    public PasswordEncripterBuilder()
    {
        _mockPasswordEncripter = new Mock<IPasswordEncripter>();

        _mockPasswordEncripter.Setup(encrypter => encrypter.Encrypt(It.IsAny<string>()))
            .Returns("passwordEncrypted");
    }

    public IPasswordEncripter Build() => _mockPasswordEncripter.Object;
}
