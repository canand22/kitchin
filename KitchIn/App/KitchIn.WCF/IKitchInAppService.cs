using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using KitchIn.Core.Services.Yummly.Response;
using KitchIn.WCF.Core.Models;
using KitchIn.WCF.Core.Models.MyAccount;
using KitchIn.WCF.Core.Models.MyFavorites;
using KitchIn.WCF.Core.Models.MyKitchen;
using KitchIn.WCF.Core.Models.CommonDataContract;
using System.Collections.Generic;
using System.ComponentModel;
using KitchIn.WCF.Core.Models.UserPreference;

namespace KitchIn.WCF
{
    using KitchIn.Core.Entities;

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
        StatusResponse LogOut(Guid id);
        
        [OperationContract]
        [WebInvoke(Method = "POST",
         ResponseFormat = WebMessageFormat.Json,
         RequestFormat = WebMessageFormat.Json)]
        RegisterResponse Register(RegisterRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        StatusResponse Forgot(string email);

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
        [WebGet(UriTemplate = "MyKitchen/AllProducts?storeId={storeid}&categoryId={categoryId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Gat all products in category into the store")]
        ProductsResponse AllProducts(long storeId, long categoryId);

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
        [WebInvoke(Method = "POST", UriTemplate = "ListProducts", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Returns a list of products for storeId by photo receipt")]
        IList<ProductMediumModel> ListProducts(CheckOutOfTheStore checkOutOfTheStore);

        [OperationContract]
        [WebGet(UriTemplate = "SearchProduct?product={product}&categoryId={categoryId}&storeId={storeId}",
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Product search by first letters on the short name")]
        IList<ProductMediumModel> SearchProduct(string product, string categoryId, string storeId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Product", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Adding of the product to the database by the user")]
        StatusResponse Product(ProductByUserModel productByUserModel);


        #region UserPreference

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserPreference/AddOrUpdate", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Add new user or update preference for current user")]
        UserPreferenceResponse AddOrUpdateUserPreference(UserPreferenceRequest request);
       
         [WebInvoke(Method = "POST", UriTemplate = "UserPreference/Remove", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Remove preference for current user")]
        UserPreferenceResponse RemoveUserPreference(UserPreferenceRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserPreference/Get", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Get all preferences for current user")]
        GetUserPreferenceResponse GetUserPreferences(GetUserPreferenceRequest request);
#endregion

        [OperationContract]
        [WebGet(UriTemplate = "Recipies?cookwith={cookWith}&cookwithout={cookWithout}&allergies={allergies}&diets={diets}&cuisine={cuisine}&dishtype={dishType}&holiday={holiday}&meal={meal}&time={time}", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<RecipeSearchRes> SearchRecipies(string cookWith, string cookWithout, string allergies, string diets, string cuisine, string dishType, string holiday, string meal, string time);

        [OperationContract]
        [WebGet(UriTemplate = "Recipe/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RecipeRes GetRecipe(string id);

        [OperationContract]
        [WebGet(UriTemplate = "Metadata/{key}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IDictionary<string, string> GetMetadata(string key);
        
        
    }
}
