using System.Collections.Generic;
using KitchIn.Core.Services.OCRRecognize;
using KitchIn.Core.Services.OCRRecognize.Parsers;
using KitchIn.WCF.Core.Models;

namespace KitchIn.BL.Helpers
{
    public class PreviewProductsHelper
    {
        public static PreviewResponse GetPreviewProducts(byte[] img)
        {
            var previewProducts = new PreviewResponse();
            //var service = new OCRRecognizeService(img);
            //var filepath = service.GetResult();

            var parser = new JewelOscoParser();

            var products = parser.GetProducts();

            //var parserWhole = new WholeFoodsParser();

            //var productsWhole = parserWhole.GetProducts();

            //var parserDominicks = new DominicksParser();

            //var productsDominicks = parserDominicks.GetProducts();
            
            ////
            ////Здесь будет логика поиска и сравнения продуктов полученных с чека и продуктов в базе 
            ////

            //foreach (var product in productsDominicks)
            //{
            //    //previewProducts.PreviewProducts.Add(product.Title, new List<ProductModel>());
            //}

            return previewProducts;
        }
    }
}