// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    public interface IDataService<T>
    {
        #region Public Methods and Operators

        public Task Create(T model);

        public Task CreateMany(IEnumerable<T> models);
        
        public Task Delete(string id);

        public Task Update(T model);

        public Task<List<T>> GetAll();

        public Task<T?> GetById(string id);

        #endregion
    }
}