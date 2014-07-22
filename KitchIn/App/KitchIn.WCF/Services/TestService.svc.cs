using KitchIn.WCF.Contracts;

namespace KitchIn.WCF.Services
{
    /// <summary>
    /// Implementation the ITestService interface
    /// </summary>
    public class TestService : ITestService
    {
        /// <summary>
        /// The test method. Performance testing service
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The received message
        /// </returns>
        public string Test(string message)
        {
            return message;
        }
    }
}
