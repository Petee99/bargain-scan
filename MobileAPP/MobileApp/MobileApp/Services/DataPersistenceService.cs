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

        private static string LocalPath => FileSystem.Current.AppDataDirectory;

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
                string filePath = Path.Combine(LocalPath, fileName);
                return File.Exists(filePath) ? await File.ReadAllTextAsync(filePath) : string.Empty;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion
    }
}