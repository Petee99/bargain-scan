// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPersistenceService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Services
{
    #region Imports

    using System.Reflection;

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
                string filePath = Path.Combine(LocalPath, fileName);

                if (File.Exists(filePath))
                {
                    return await File.ReadAllTextAsync(filePath);
                }

                return await TryLoadEmbeddedResourceAsStringAsync(fileName);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region Private Methods

        // Only for debug purposes
        private static async Task<string> TryLoadEmbeddedResourceAsStringAsync(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullyQualifiedResourceName = $"{assembly.GetName().Name}.Resources.{resourceName}";

            await using Stream stream = assembly.GetManifestResourceStream(fullyQualifiedResourceName);
            if (stream == null) throw new FileNotFoundException($"Resource {fullyQualifiedResourceName} not found.");

            using StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        #endregion

        private static string LocalPath => FileSystem.Current.AppDataDirectory;
    }
}