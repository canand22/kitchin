using System.ServiceModel;

namespace KitchIn.WCF.Contracts
{
    /// <summary>
    /// The test service interface
    /// </summary>
    [ServiceContract]
    public interface ITestService
    {
        /// <summary>
        /// The test method. Performance testing service
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <returns> The received message </returns>
        [OperationContract]
        string Test(string message);
    }
}
