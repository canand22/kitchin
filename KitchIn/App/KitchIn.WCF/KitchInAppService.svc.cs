using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Hosting;
using KitchIn.BL.Helpers;
using KitchIn.Core.Enums;
using KitchIn.Core.Interfaces;
using KitchIn.Core.Models;
using KitchIn.WCF.Core.Models;
using KitchIn.WCF.Core.Models.MyAccount;
using KitchIn.WCF.Core.Models.MyFavorites;
using KitchIn.WCF.Core.Models.MyKitchen;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using Castle.Core.Internal;
using KitchIn.Core.Services.OCRRecognize;
using KitchIn.WCF.DataContract;


namespace KitchIn.WCF
{
    using KitchIn.Core.Services.OCRRecognize.Parsers;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class KitchInAppService : IKitchInAppService
    {
        private readonly IManageUserProvider userProvider;

        private readonly IManageProductProvider productProvider;

        private readonly IManageKitchenProvider kitchenProvider;

        private readonly IManageFavoritesProvider favoritesProvider;

        private readonly IManageStoreProvider manageStoreProvider;

        private readonly IManageMatchingTexts manageMatchingTexts;

        public KitchInAppService(IManageUserProvider userProvider, IManageProductProvider productProvider, IManageKitchenProvider kitchenProvider, IManageFavoritesProvider favoritesProvider, IManageStoreProvider manageStoreProvider, IManageMatchingTexts manageMatchingTexts)
        {
            this.userProvider = userProvider;
            this.productProvider = productProvider;
            this.kitchenProvider = kitchenProvider;
            this.favoritesProvider = favoritesProvider;
            this.manageStoreProvider = manageStoreProvider;
            this.manageMatchingTexts = manageMatchingTexts;
        }

        public LoginResponse LogIn(LoginRequest request)
        {

            ////var uriRequest = new Uri("http://api.yummly.com/v1/api/metadata/ingredient?_app_id=9c98447e&_app_key=f5d8155ae0acc445e0fe2f504dc46bce");
            ////var req = WebRequest.Create(uriRequest);

            ////var response = (HttpWebResponse)req.GetResponse();

            ////var dataStream = response.GetResponseStream();
            ////if (response.ContentEncoding.ToLower().Contains("gzip"))
            ////{
            ////    dataStream = new GZipStream(dataStream, CompressionMode.Decompress);
            ////}

            ////var reader = new StreamReader(dataStream);

            ////var responseFromServer = reader.ReadToEnd();

            ////var startIndex = responseFromServer.IndexOf("[");
           
            ////responseFromServer = responseFromServer.Substring(startIndex);
            ////var ar = responseFromServer.ToCharArray();
            ////Array.Resize(ref ar, ar.Length - 2);
            ////responseFromServer = new string(ar);
            ////var r = JsonConvert.DeserializeObject<List<IngredientModel>>(responseFromServer);

            ////reader.Close();

            ////dataStream.Close();

            ////response.Close();

            if (request == null)
            {
                return new LoginResponse { Success = false, SessionId = null };
            }

            var user = this.userProvider.GetUser(request.Email, request.Password);

            return user == null
                       ? new LoginResponse
                             {
                                 SessionId = null,
                                 Success = false
                             }
                       : new LoginResponse
                             {
                                 SessionId = user.SessionId,
                                 Success = true
                             };
        }

        public void LogOut(Guid id)
        {
            this.userProvider.LogOut(id);
        }

        public RegisterResponse Register(LoginRequest request)
        {
            if (request == null)
            {
                return new RegisterResponse { IsUserRegistered = false, SessionId = null };
            }

            var user = this.userProvider.CreateUser(request.Email, request.Password);

            return user == null
                      ? new RegisterResponse
                      {
                          IsUserRegistered = false, SessionId = null
                      }
                      : new RegisterResponse
                      {
                          IsUserRegistered = true, SessionId = user.SessionId
                      };
        }

        public bool Forgot(string email)
        {
            return this.userProvider.ChangeUserPassword(email);
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

        public ProductsResponse AllProducts(string categoryId)
        {
            return new ProductsResponse { Products = this.productProvider.GetAllProducts(categoryId) };
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

        public bool UpdatePassword(PasswordRequest request)
        {
            return this.userProvider.ChangeUserPassword(request.Guid, request.OldPassword, request.NewPassword);
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

        public IList<ListProducts> ListProducts(Stream fileContents, long storeId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = new List<ListProducts>();
            string[] textRecognizersResult;
            bool isExistStore = this.manageStoreProvider.GetStore(storeId) != null;
            if (fileContents != null && isExistStore)
            {
                var image = Image.FromStream(fileContents);
                var bitmap = new Bitmap(image, image.Width, image.Height);
                var memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, ImageFormat.Jpeg);
                var ocrRecognizeService = new OCRRecognizeService(memoryStream);
                textRecognizersResult = ocrRecognizeService.GetResult();
                
                var parser = new PotashParser(textRecognizersResult);
                var products = parser.GetProducts().Select(x => x.Title).ToArray();
                var resultsOfTheMatching = manageMatchingTexts.GetResultsOfTheMatching(products, storeId);
                resultsOfTheMatching.ForEach(
                    x => {
                             if (x.IsSuccessMatching && x.Id!=null && productProvider.IsFoodGroupe(x.Id.Value))
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
            stopWatch.Stop();
            var time = stopWatch.ElapsedMilliseconds;
            return result;
        }

        //private static Stream ConverImgToBytes(Stream fileContents)
        //{
        //    //using (var memoryStream = new MemoryStream())
        //    //{
        //    //    fileContents.CopyTo(memoryStream);
        //    //    System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
        //    //    Image img = (Image)converter.ConvertFrom(memoryStream.ToArray());
        //    //}

        //    Stream serviceStream = new MemoryStream();
        //    byte[] buffer = new byte[10000];
        //    int bytesRead = 0;
        //    do
        //    {
        //        bytesRead = fileContents.Read(buffer, 0, buffer.Length);
        //        serviceStream.Write(buffer, 0, bytesRead);
        //    } while (bytesRead > 0);

        //    return serviceStream;
        //}
    }
}