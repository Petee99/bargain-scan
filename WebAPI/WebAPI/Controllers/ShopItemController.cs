// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SensorDataController.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Controllers

{
    #region Imports

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Web.Http.Results;
    using WebAPI.Enums;
    using WebAPI.Models.DataModels;
    using WebAPI.Models.WebScraping;
    using WebAPI.Properties;
    using WebAPI.Services;

    #endregion

    [Route(Constants.ShopItemsRoute)]
    public class ShopItemController : ApiControllerBase<ShopItem>
    {
        #region Constructors and Destructors

        public ShopItemController(DataBaseService<ShopItem> dataBaseService)
            : base(dataBaseService)
        {
        }

        #endregion

        #region Public Methods and Operators

        [HttpPost]
        [AllowAnonymous]
        public override async Task<IActionResult> Post([FromBody] ShopItem model)
        {
            await dataBaseService.Create(model);

            return Ok();
        }

        [HttpPost]
        [Route(Constants.UploadDataFromWeb)]
        public async Task<IActionResult> UploadDataFromWebsite([FromBody] IEnumerable<ShopItem> models)
        {
            if(models is null || models.Count() is 0)
            {
                return StatusCode(500);
            }

            foreach(var model in models)
            {
                if(model.Name is null || model.CategoryName is null || 
                    model.SubCategoryName is null || model.Price is null)
                {
                    return StatusCode(500);
                }
            }

            await dataBaseService.CreateMany(models);

            return Ok();
        }


        [HttpGet]
        [AllowAnonymous]
        public override async Task<List<ShopItem>> Get()
        {
            return await dataBaseService.GetAll();
        }

        #endregion
    }
}