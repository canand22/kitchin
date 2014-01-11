using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using KitchIn.WCF.Core.Models;
using KitchIn.WCF.Core.Models.MyAccount;
using KitchIn.WCF.Core.Models.MyFavorites;
using KitchIn.WCF.Core.Models.MyKitchen;
using System.Collections.Generic;
using System.ComponentModel;

namespace KitchIn.WCF
{
    using KitchIn.WCF.DataContract;

    [ServiceContract]
    public interface IKitchInAppService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        LoginResponse LogIn(LoginRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        void LogOut(Guid id);
        
        [OperationContract]
        [WebInvoke(Method = "POST",
         ResponseFormat = WebMessageFormat.Json,
         RequestFormat = WebMessageFormat.Json)]
        RegisterResponse Register(LoginRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        bool Forgot(string email);

        [OperationContract]
        [WebInvoke(Method = "POST",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json)]
        PreviewResponse Preview(PreviewRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
         ResponseFormat = WebMessageFormat.Json,
         RequestFormat = WebMessageFormat.Json)]
        bool SavePreview(SavePreviewRequest request);

        #region MyKitchen

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "MyKitchen/Categories",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        KitchenCategoriesResponse Categories(KitchenCategoriesRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "MyKitchen/Products",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        KitchenProductsResponse Products(KitchenProductsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "MyKitchen/Add",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        bool AddProduct(KitchenProductRequest request);

        [OperationContract]
        [WebGet(
            UriTemplate = "MyKitchen/AllProducts/{categoryId}",
            ResponseFormat = WebMessageFormat.Json)]
        ProductsResponse AllProducts(string categoryId);

        [OperationContract]
        [WebGet(
            UriTemplate = "MyKitchen/Remove/{productId}",
            ResponseFormat = WebMessageFormat.Json)]
        void RemoveProduct(string productId);

        [OperationContract]
        [WebGet(
            UriTemplate = "MyKitchen/Edit/{productId}/{quantity}",
            ResponseFormat = WebMessageFormat.Json)]
        void EditProduct(string productId, string quantity);

        #endregion

        #region MyFavorites

        [OperationContract]
        [WebInvoke(UriTemplate = "Favorites",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        bool SetFavorites(SetFavoritesRequest request);

        [OperationContract]
        [WebInvoke(UriTemplate = "Favorites/All",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        FavoritesResponse GetAll(GetFavoritesRequest request);

        #endregion

        #region MyAccount

        [OperationContract]
        [WebInvoke(UriTemplate = "MyAccount/Update",
            ResponseFormat = WebMessageFormat.Json)]
        bool UpdatePassword(PasswordRequest request);

        #endregion

        [OperationContract]
        [WebGet(UriTemplate = "Media/{videoId}")]
        Stream GetVideo(string videoId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListProducts?storeId={storeId}")]
        [Description("Returns a list of products for storeId by photo receipt")]
        IList<ListProducts> ListProducts(Stream fileContents, long storeId);
    }
}
