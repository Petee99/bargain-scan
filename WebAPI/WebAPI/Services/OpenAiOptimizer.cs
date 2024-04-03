// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenAiOptimizer.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Services
{
    #region Imports

    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Text.Unicode;

    using Microsoft.Identity.Client.Internal;

    using Newtonsoft.Json;

    using OpenAI_API;

    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;

    using Constants = WebAPI.Properties.Constants;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    #endregion

    public class OpenAiOptimizer : IOpenAiOptimizer
    {
        #region Constants and Private Fields
        
        private const string Pattern = @"\[(.|\s)*?\]";

        private static readonly Regex Regex = new(Pattern, RegexOptions.Compiled);

        private readonly IOpenAIAPI _api;

        #endregion

        #region Constructors and Destructors

        public OpenAiOptimizer(IOpenAIAPI openAiApi)
        {
            _api = openAiApi;
        }
        
        #endregion

        #region Public Methods and Operators

        public async Task OptimizeCategoriesToMatchSchema(IList<IShopItemCategory> categories, IList<IShopItemCategory> schemaCategories)
        {
            var tasks = new List<Task>();

            try
            {
                tasks.AddRange(CreateCategoryListFragments(categories)
                    .Select(categoryListFragment => RenameCategoriesAsync(_api, categoryListFragment, schemaCategories)));

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                var exs = ex;
            }
        }

        #endregion

        #region Private Methods

        private static IEnumerable<IShopItemCategory> ConvertResponseStringToCategories(string response)
        {
            var matchedResponse = Regex.Match(response).Value;

            return JsonConvert.DeserializeObject<List<ShopItemCategory>>(matchedResponse)!.Cast<IShopItemCategory>().ToList();
        }

        private static IList<IList<IShopItemCategory>> CreateCategoryListFragments(IList<IShopItemCategory> categories)
        {
            var subLists = new List<IList<IShopItemCategory>>();

            for (int i = 0; i < categories.Count; i += 10)
            {
                List<IShopItemCategory> sublist = categories.Skip(i).Take(10).ToList();
                subLists.Add(sublist);
            }

            return subLists;
        }

        private static string ConstructPrompt(IList<IShopItemCategory> categories, IList<IShopItemCategory> schemaCategories)
        {
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            return
                $"""
                     I have two sets of category data in JSON format. The first set, 'Actual Subcategories', contains {categories.Count} entries, each with a subcategory and its parent category.
                     The second set, 'Standard Subcategories', contains a list of standard subcategories and their parent categories.
                     I need to match each 'Actual Subcategory' to the closest 'Standard Subcategory' based on similarity in names and context, then rename the 'Actual Subcategories' to align with the 'Standard Subcategories'.
                 
                     The output should be a JSON document with the updated 'Actual Subcategories', with subcategory names and parent matched and renamed to the corresponding 'Standard Subcategories'.
                     The document should be in Hungarian.
                 
                     Here are the JSON data for both sets:
                 
                     Actual Subcategories:
                     {JsonSerializer.Serialize(categories, options)}
                 
                     Standard Subcategories:
                     {JsonSerializer.Serialize(schemaCategories, options)}
                 
                     Please provide the updated list of 'Actual Subcategories' in the requested format.
                 """
                ;
        }

        private static async Task RenameCategoriesAsync(IOpenAIAPI api, IList<IShopItemCategory> categories,
            IList<IShopItemCategory> schemaCategories)
        {
            int maxAttempts = 3;
            int attempts = 0;

            while (maxAttempts > attempts)
            {
                try
                {
                    var response = "";
                    var chat = api.Chat.CreateConversation();
                    chat.AppendUserInput(ConstructPrompt(categories, schemaCategories));
                    await foreach (var res in chat.StreamResponseEnumerableFromChatbotAsync())
                    {
                        response += res;
                    }

                    OverWriteCategoriesWithNewCategoryNames(categories, ConvertResponseStringToCategories(response).ToList());
                    break;
                }
                catch (Exception)
                {
                    attempts++;

                    if (attempts > maxAttempts)
                    {
                        throw;
                    }
                }
            }
        }

        private static void OverWriteCategoriesWithNewCategoryNames(IList<IShopItemCategory> categories, IList<IShopItemCategory> newCategories)
        {
            if (categories.Count != newCategories.Count)
            {
                throw new InvalidOperationException();
            }

            for (int index = 0; index < categories.Count; index++)
            {
                categories[index].Name = newCategories[index].Name;
                categories[index].Parent = newCategories[index].Parent;
            }
        }

        #endregion
    }
}