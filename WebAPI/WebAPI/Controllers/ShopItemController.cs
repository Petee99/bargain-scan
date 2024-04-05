// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopItemController.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Controllers
{
    #region Imports

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;
    using WebAPI.Properties;

    #endregion

    [Route(Constants.ShopItemsRoute)]
    public class ShopItemController : ApiControllerBase<ShopItem>
    {
        #region Constructors and Destructors

        public ShopItemController(IDataBaseService<ShopItem> dataBaseService)
            : base(dataBaseService)
        {
        }

        #endregion

        #region Public Methods and Operators

        [HttpPost]
        [AllowAnonymous]
        public override async Task<IActionResult> Post([FromBody] ShopItem model)
        {
            await DataBaseService.Create(model);

            return Ok();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public override async Task<List<ShopItem>> Get()
        {
            return await DataBaseService.GetAll();
        }

        [HttpPost]
        [Route(Constants.UploadDataFromWeb)]
        public async Task<IActionResult> UploadDataFromWebsite([FromBody] IEnumerable<ShopItem>? models)
        {
            if (models?.ToList() is not { Count: > 0 } itemList)
            {
                return StatusCode(500);
            }

            if (itemList.Any(item => item is not { Name: not null, CategoryName: not null, SubCategoryName: not null, Price: not null }))
            {
                return StatusCode(500);
            }

            await DataBaseService.CreateMany(itemList);

            return Ok();
        }

        #endregion
    }
}