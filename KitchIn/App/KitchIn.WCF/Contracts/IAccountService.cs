using System.ServiceModel;
using KitchIn.WCF.Core.Models;

namespace KitchIn.WCF.Contracts
{
    /// <summary>
    /// The account service interface
    /// </summary>
    [ServiceContract]
    public interface IAccountService
    {
        /// <summary>
        /// Registration user in system
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns>
        /// The RegisterUserResponse: 
        ///		IsUserRegistered: true - user is registered; false - user doesn't registered in system; 
        ///		ValidationErrors - the list of the form : name exception / exception description 
        /// </returns>
        [OperationContract]
        RegisterUserResponse Register(RegisterUserRequest model);
    }
}
