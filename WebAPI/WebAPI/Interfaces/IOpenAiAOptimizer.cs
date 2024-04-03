// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOpenAiAOptimizer.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    public interface IOpenAiOptimizer
    {
        public Task OptimizeCategoriesToMatchSchema(IList<IShopItemCategory> categories, IList<IShopItemCategory> schemaCategories);
    }
}