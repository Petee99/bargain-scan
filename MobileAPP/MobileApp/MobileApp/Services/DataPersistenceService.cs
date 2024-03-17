// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPersistenceService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Services
{
    #region Imports

    using MobileApp.Interfaces;

    #endregion

    internal class DataPersistenceService : IDataPersistenceService
    {
        #region Constants and Private Fields

        public const string ItemsFileName = "items.json";

        public const string UserProfileFileName = "userProfile.json";

        #endregion

        #region Public Methods and Operators

        public bool TrySaveLocalData(string fileName, string data)
        {
            try
            {
                File.WriteAllText(Path.Combine(LocalPath, fileName), data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> TryLoadLocalData(string fileName)
        {
            try
            {
                return await File.ReadAllTextAsync(Path.Combine(LocalPath, fileName));
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion

        private static string LocalPath => FileSystem.Current.AppDataDirectory;
    }
}