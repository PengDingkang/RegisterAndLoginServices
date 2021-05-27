using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegisterAndLoginServices.Services;
using RegisterAndLoginServices.Models;

namespace UnitTest
{
    [TestClass]
    class RegisterTest
    {
        [TestMethod]
        public void Register_With_Right_Contact_And_Password()
        {
            // Arrange
            RegisterModel reg = new()
            {
                contact = "13944556677",
                password = "password"
            };
            bool succeed = true;

            // Act
            try
            {
                Register.AccepteRegister(reg);
            }
            catch
            {
                succeed = false;
            }

            // Assert
            Assert.IsTrue(succeed);
        }
    }
}
