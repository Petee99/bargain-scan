// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMongoDbService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    #region Imports

    using MongoDB.Driver;

    #endregion

    public interface IMongoDbService
    {
        #region Public Methods and Operators

        IMongoDatabase GetDatabase();

        #endregion
    }
}