﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using KitchIn.BL.Helpers;
using KitchIn.Core.Entities;
using KitchIn.Core.Enums;
using KitchIn.Core.Interfaces;
using KitchIn.Core.Models;
using KitchIn.Core.Services.Jobs;
using KitchIn.Core.Services.Mailing;
using KitchIn.Core.Services.Yummly;
using KitchIn.Core.Services.Yummly.Response;
using KitchIn.WCF.Core.Models;
using KitchIn.WCF.Core.Models.MyAccount;
using KitchIn.WCF.Core.Models.MyFavorites;
using KitchIn.WCF.Core.Models.MyKitchen;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using Castle.Core.Internal;
using KitchIn.Core.Services.OCRRecognize;
using KitchIn.WCF.Core.Models.CommonDataContract;
using KitchIn.WCF.Core.Models.UserPreference;
using Newtonsoft.Json;

namespace KitchIn.WCF
{
    using System.Text.RegularExpressions;

    using KitchIn.BL.Entities;
    using KitchIn.BL.Extensions;
    using KitchIn.BL.Implementation;
    using KitchIn.Core.Services.OCRRecognize.Parsers;
    using KitchIn.WCF.Core.Models.CommonDataContract;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class KitchInAppService : IKitchInAppService
    {
        private readonly IManageUserProvider userProvider;

        private readonly IManageProductProvider productProvider;

        private readonly IManageKitchenProvider kitchenProvider;

        private readonly IManageFavoritesProvider favoritesProvider;

        private readonly IManageStoreProvider manageStoreProvider;

        private readonly IManageMatchingTexts manageMatchingTexts;

        private readonly IManageProductByUserProvider productByUserProvider;

        private readonly IManageUserPreferenceProvider userPreferenceProvider;
        private IYummly yummlyManager;

        private IRunable yummlyUpdater;

        public KitchInAppService(IManageUserProvider userProvider, IManageProductProvider productProvider, IManageKitchenProvider kitchenProvider,
            IManageFavoritesProvider favoritesProvider, IManageStoreProvider manageStoreProvider, IManageMatchingTexts manageMatchingTexts,
            IManageProductByUserProvider productByUserProvider, IManageUserPreferenceProvider userPreferenceProvider, IYummly yummlyManager, IRunable yummlyUpdater)
        {
            this.userProvider = userProvider;
            this.productProvider = productProvider;
            this.kitchenProvider = kitchenProvider;
            this.favoritesProvider = favoritesProvider;
            this.manageStoreProvider = manageStoreProvider;
            this.manageMatchingTexts = manageMatchingTexts;
            this.productByUserProvider = productByUserProvider;
            this.userPreferenceProvider = userPreferenceProvider;


            this.yummlyManager = yummlyManager;
            this.yummlyUpdater = yummlyUpdater;
        }

        public UserPreferenceResponse AddOrUpdateUserPreference(UserPreferenceRequest request)
        {
            string message = this.userPreferenceProvider.AddOrUpdateUserPreference(request.Guid, request.UserPreference);
            return new UserPreferenceResponse
            {
                Success = message == CommonConstants.MESSAGE_SUCCESS ? true : false,
                Message = message
            };
        }

        public UserPreferenceResponse RemoveUserPreference(UserPreferenceRequest request)
        {
            string message = this.userPreferenceProvider.RemoveUserPreference(request.Guid, request.UserPreference);
            return new UserPreferenceResponse
            {
                Success = message == CommonConstants.MESSAGE_SUCCESS ? true : false,
                Message = message
            };
        }
        public GetUserPreferenceResponse GetUserPreferences(GetUserPreferenceRequest request)
        {
            return new GetUserPreferenceResponse
                     {
                         UserPreference =
                               this.userPreferenceProvider
                               .GetUser(request.Guid)
                               .UserPreferences
                               .Select(p => new UserPreferenceiPhoneModel
                               {
                                   Allergies = p.Allergies.Select(t => t.SearchValue).ToList(),
                                   AllowedIngridients = p.AllowedIngredients == null ? null : p.AllowedIngredients.Select(t => t.Term).ToList(),
                                   ExcludedIngridients = p.ExcludedIngredients == null ? null : p.ExcludedIngredients.Select(t => t.Term).ToList(),
                                   Cuisines = p.Cuisines == null ? null : p.Cuisines.Select(t => t.Name).ToList(),
                                   Diets = p.Diets == null ? null : p.Diets.Select(t => t.ShortDescription).ToList(),
                                   DishType = p.DishType == null ? null : p.DishType.Name,
                                   Holidays = p.Holidays == null ? null : p.Holidays.Select(t => t.Name).ToList(),
                                   Meal = p.Meal,
                                   OwnerPreference = p.OwnerPreference,
                                   Time = new List<TimeType>() { (TimeType)Enum.Parse(typeof(TimeType), p.Time, true) }
                               }).ToList()
                     };
        }
        public LoginResponse LogIn(LoginRequest request)
        {
            if (request == null)
            {
                return new LoginResponse { Success = false, SessionId = null };
            }

            var user = this.userProvider.GetUser(request.Email, request.Password);
            this.userProvider.LogIn(user);
            return user == null
                       ? new LoginResponse
                             {
                                 SessionId = null,
                                 Success = false
                             }
                       : new LoginResponse
                             {
                                 SessionId = user.SessionId,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Success = true
                             };
        }

        public StatusResponse LogOut(Guid id)
        {
            this.userProvider.LogOut(id);
            var result = new StatusResponse() { IsSuccessfully = true, Message = "Ok" };
            return result;
        }

        public RegisterResponse Register(RegisterRequest request)
        {
            if (request == null)
            {
                return new RegisterResponse { IsUserRegistered = false, SessionId = null, Message = Errors.EmptyRequest.GetMessage() };
            }

            if (!this.IsEmail(request.Email))
            {
                return new RegisterResponse { IsUserRegistered = false, SessionId = null, Message = String.Format(Errors.EmailNotValid.GetMessage(), request.Email) };
            }

            if (String.IsNullOrEmpty(request.FirstName))
            {
                return new RegisterResponse { IsUserRegistered = false, SessionId = null, Message = Errors.EmptyFirstName.GetMessage() };
            }

            if (String.IsNullOrEmpty(request.LastName))
            {
                return new RegisterResponse { IsUserRegistered = false, SessionId = null, Message = Errors.EmptyLastName.GetMessage() };
            }

            if (String.IsNullOrEmpty(request.Password))
            {
                return new RegisterResponse { IsUserRegistered = false, SessionId = null, Message = Errors.EmptyPassword.GetMessage() };
            }

            var user = this.userProvider.CreateUser(request.Email, request.Password, request.FirstName, request.LastName);

            return user == null
                      ? new RegisterResponse
                      {
                          IsUserRegistered = false,
                          SessionId = null,
                          Message = Errors.ErrorCreatingUser.GetMessage()
                      }
                      : new RegisterResponse
                      {
                          IsUserRegistered = true,
                          SessionId = user.SessionId,
                          Message = "Ok"
                      };
        }

        public StatusResponse Forgot(string email)
        {
            var result = new StatusResponse() { IsSuccessfully = this.userProvider.ForgotPassword(email), Message = "Ok" };
            return result;
        }

        public PreviewResponse Preview(PreviewRequest request)
        {
            PreviewProductsHelper.GetPreviewProducts(new byte[2]); ////request.Img

            PreviewResponse model = new PreviewResponse();
            model.PreviewProducts.Add(new KeyValuePair<string, IList<ProductModel>>(
                                      "First",
                                      new List<ProductModel>
                                          {
                                              new ProductModel
                                                  {
                                                      Name = "First",
                                                      Description = "FirstDescription",
                                                      ProductId = 111,
                                                      UnitType = UnitType.FZ,
                                                      Quantity = 1
                                                  }
                                          }));
            model.PreviewProducts.Add(new KeyValuePair<string, IList<ProductModel>>(
                                      "Second",
                                      new List<ProductModel>
                                          {
                                              new ProductModel
                                                  {
                                                      Name = "Second1",
                                                      Description = "Second1Description",
                                                      ProductId = 222,
                                                      UnitType = UnitType.OZ,
                                                      Quantity = 1
                                                  },
                                              new ProductModel
                                                  {
                                                      Name = "Second2",
                                                      Description = "Second2Description",
                                                      ProductId = 223,
                                                      UnitType = UnitType.OZ,
                                                      Quantity = 1
                                                  }
                                          }));
            model.PreviewProducts.Add(new KeyValuePair<string, IList<ProductModel>>(
                                      "Third",
                                      new List<ProductModel>
                                          {
                                              new ProductModel
                                                  {
                                                      Name = string.Empty,
                                                      Description = string.Empty,
                                                      UnitType = UnitType.ML,
                                                      Quantity = 1
                                                  },
                                          }));

            return model;
        }

        public bool SavePreview(SavePreviewRequest request)
        {
            return this.kitchenProvider.AddProducts(request.Guid, request.Products);
        }

        #region MyKitchen

        public KitchenCategoriesResponse Categories(KitchenCategoriesRequest request)
        {
            return new KitchenCategoriesResponse { Categories = this.kitchenProvider.GetCategories(request.Guid) };
        }

        public KitchenProductsResponse Products(KitchenProductsRequest request)
        {
            return new KitchenProductsResponse { Products = this.kitchenProvider.GetProducts(request.Guid, request.CategoryId) };
        }

        public bool AddProduct(KitchenProductRequest request)
        {
            return this.kitchenProvider.AddProductOnKitchen(request.Guid, request.Product);
        }

        public ProductsResponse AllProducts(long storeId, long categoryId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var products = this.productProvider.GetAllProductsByStoreAndCategory(storeId, categoryId)
                .Select(x => new PropuctSimpleModel() { Id = x.Id, Name = textInfo.ToTitleCase(x.Name.ToLower()), ShortName = x.ShortName, IngredientName = textInfo.ToTitleCase(x.IngredientName.ToLower()) });
            return new ProductsResponse { Products = products };
        }

        public void RemoveProduct(string productId)
        {
            this.kitchenProvider.RemoveProductOnKitchen(productId);
        }

        public void EditProduct(string productId, string quantity)
        {
            this.kitchenProvider.EditProductOnKitchen(productId, quantity);
        }

        #endregion

        #region MyFavorites

        public bool SetFavorites(SetFavoritesRequest request)
        {
            return this.favoritesProvider.SetFavorites(request.Guid, request.RecipesBigOven, request.HasFavorites);
        }

        public FavoritesResponse GetAll(GetFavoritesRequest request)
        {
            return new FavoritesResponse
                       {
                           BigOvenRecipes =
                               this.favoritesProvider
                               .GetUser(request.Guid)
                               .FavoriteRecipes
                               .Select(x => x.RecipeBigOven)
                       };
        }

        #endregion

        #region MyAccount

        public OperationResult UpdatePassword(PasswordRequest request)
        {
            return new OperationResult()
            {
                Success = this.userProvider.ChangeUserPassword(request.Guid, request.OldPassword, request.NewPassword)
            };
        }

        public OperationResult UpdateUserData(UpdateUserRequest request)
        {
            return new OperationResult()
            {
                Success = this.userProvider.ChangeUserData(request.Guid, request.NewEmail, request.FirstName, request.LastName)
            };
        }

        #endregion

        public Stream GetVideo(string name)
        {
            HttpContext context = HttpContext.Current;

            var path = HostingEnvironment.MapPath("~/Content/temp/JoeNall10_3DH-_W640.wmv");
            if (File.Exists(path))
            {
                var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                WebOperationContext.Current.OutgoingResponse.ContentType = "video/x-ms-wmv";
                return stream;
            }

            return null;
        }

        public IList<ProductMediumModel> ListProducts(CheckOutOfTheStore checkOutOfTheStore)
        {
            var result = new List<ProductMediumModel>();
            string[] textRecognizersResult;
            bool isExistStore = this.manageStoreProvider.GetStore(checkOutOfTheStore.StoreId) != null;
            if (!String.IsNullOrEmpty(checkOutOfTheStore.ImageAsBase64String) && isExistStore)
            {
                var image = Base64ToImage(checkOutOfTheStore.ImageAsBase64String);
                var bitmap = new Bitmap(image, image.Width, image.Height);
                var memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, ImageFormat.Jpeg);
                var ocrRecognizeService = new OCRRecognizeService(memoryStream);
                textRecognizersResult = ocrRecognizeService.GetResult();

                var parser = new PotashParser(textRecognizersResult);
                var products = parser.GetProducts().Select(x => x.Title).ToArray();
                var resultsOfTheMatching = manageMatchingTexts.GetResultsOfTheMatching(products, checkOutOfTheStore.StoreId);
                resultsOfTheMatching.ForEach(
                    x =>
                    {
                        if (x.IsSuccessMatching && x.Id != null && productProvider.IsFoodGroupe(x.Id.Value))
                        {
                            result.Add(x);
                        }
                        else
                        {
                            if (!x.IsSuccessMatching)
                            {
                                result.Add(x);
                            }
                        }
                    });
            }
            return result;
        }

        public IList<ProductMediumModel> SearchProduct(string product, string categoryId, string storeId)
        {
            var result = new List<ProductMediumModel>();
            if (!String.IsNullOrEmpty(product))
            {
                try
                {
                    var stId = Convert.ToInt64(storeId);
                    var catid = Convert.ToInt64(categoryId);
                    var tmp = productProvider.SearchProductsByFirstLetters(product, catid, stId);
                    
                    result.AddRange(tmp.Select(item => (ProductMediumModel)item));
                }
                catch (Exception ex)
                {
                    throw new Exception("Can not convert string to long: {0}", ex);
                }
            }
            return result;
        }

        public StatusResponse Product(ProductByUserModel product)
        {
            var result = new StatusResponse();
            if (String.IsNullOrEmpty(product.Name) && String.IsNullOrEmpty(product.ShortName) && String.IsNullOrEmpty(product.UpcCode))
            {
                result.IsSuccessfully = false;
                result.Message = Errors.NotFilledRequiredFields.GetMessage();
                return result;
            }
            if (!userProvider.IsExistUser(product.SessionId))
            {
                result.IsSuccessfully = false;
                result.Message = Errors.UserNotExist.GetMessage();
                return result;
            }
            var provider = (this.userProvider as BaseProvider);
            var user = provider.GetUser(product.SessionId);

            this.productByUserProvider.Save(product.UpcCode, product.ShortName, product.Name, product.IngredientName, product.CategoryId, product.StoreId,
                user, product.ExpirationDate);
            result.IsSuccessfully = true;
            result.Message = "Ok";
            return result;
        }

        private Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        private bool IsEmail(String strEmail)
        {
            var rgxEmail = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                       @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                       @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return rgxEmail.IsMatch(strEmail);
        }

        //public IEnumerable<RecipeSearchRes> SearchRecipies(string cookWith = "", string cookWithout = "", string allergies = "", string diets = "", string cuisine = "", string dishType = "", string holiday = "", string meal = "", string time = "")
        public SearchResult SearchRecipies(string cookWith = "", string cookWithout = "", string allergies = "", string diets = "", string cuisine = "", string dishType = "", string holiday = "", string meal = "", string time = "", int page = 1, int perpage = 10)
        {
            try
            {
                var entity = new YummlyReqEntity()
                {
                    CookWith = !String.IsNullOrWhiteSpace(cookWith) ? cookWith.ToLower().Split(',') : null,
                    CookWithout = !String.IsNullOrWhiteSpace(cookWithout) ? cookWithout.ToLower().Split(',') : null,
                    Allergies = !String.IsNullOrWhiteSpace(allergies) ? allergies.ToLower().Split(',') : null,
                    Cuisine = !String.IsNullOrWhiteSpace(cuisine) ? cuisine.ToLower().Split(',') : null,
                    Diets = !String.IsNullOrWhiteSpace(diets) ? diets.ToLower().Split(',') : null,
                    DishType = !String.IsNullOrWhiteSpace(dishType) ? dishType.ToLower().Split(',') : null,
                    Holiday = !String.IsNullOrWhiteSpace(holiday) ? holiday.ToLower().Split(',') : null,
                    Meal = !String.IsNullOrWhiteSpace(meal) ? meal.ToLower().Split(',') : null,
                    Time = time,
                    Page = page,
                    PerPage = perpage
                };

                return yummlyManager.Search(entity);
            }
            catch
            {
                return new SearchResult();
            }
        }

        public RecipeRes GetRecipe(string id)
        {
            var res = yummlyManager.GetRecipe(id);

            return res;
        }

        public IDictionary<string, string> GetMetadata(string key)
        {
            return YummlyManager.GetMetadata(key);
        }

        public IList<ReferenceData> IngredientsRelations(string key = "All")
        {
            return yummlyManager.GetIngredientsRelations(key);
        }

        public IList<ReferenceData> YummlyIngredientsRelations(string key = "All")
        {
            return yummlyManager.GetIngredientsRelationsByYammlyName(key);
        }
    }
}