// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Properties
{
    public static class Constants
    {
        #region Constants and Private Fields

        public const int PreFlightMaxTimeInSeconds = 3600;

        public const string AccessToken = "X-Access-Token";

        public const string AddAdminRoute = "add_admin";

        public const string Admin = "Admin";

        public const string AdminCollectionName = "admins";

        public const string AuthenticateRoute = "authenticate";

        public const string ShopItemsRoute = "api/shopitems";

        public const string ClientDomain = "http://localhost:4200";

        public const string CollectionPropertyString = "CollectionName";

        public const string ConnectionString =
            "mongodb://mako99:FingerBoard101499@cluster0-shard-00-00.vf2zk.mongodb.net:27017,cluster0-shard-00-01.vf2zk.mongodb.net:27017,cluster0-shard-00-02.vf2zk.mongodb.net:27017/?ssl=true&replicaSet=atlas-lkoynm-shard-0&authSource=admin&retryWrites=true&w=majority";

        public const string CorsPolicyKey = "ApiCorsPolicy";

        public const string DatabaseName = "wt2-database";

        public const string DeleteMethod = "DELETE";

        public const string DeleteUserRoute = "deleteuser";

        public const string EmailField = "email";

        public const string EveryContentType = "*";

        public const string GetMethod = "GET";

        public const string ID = "{id}";

        public const string IdPropertyString = "_id";

        public const string IsAuthenticatedRoute = "isauthenticated";

        public const string Key = "8Jo1zQHUrtFf4hSsLDoasVDJmwqAr2qcZH8lcc2ErLnH8AKNekpcnsiPTs1";

        public const string LogOutRoute = "logout";

        public const string MongoDBKey = "MongoDB";

        public const string OptionsMethod = "OPTIONS";

        public const string PasswordField = "password";

        public const string PostMethod = "POST";

        public const string PutMethod = "PUT";

        public const string RefreshRoute = "refresh";

        public const string RefreshToken = "X-Refresh-Token";

        public const string RefreshTokenExpiryTimeField = "refreshtoken_expirytime";

        public const string RefreshTokenField = "refreshtoken";

        public const string RoleField = "role";
        
        public const string SetScraperServiceRoute = "api/scraper-service";

        public const string UploadDataFromWeb = "itemlist-upload";

        public const string User = "User";

        public const string UserCollectionName = "users";

        public const string ShopItemCollectionName = "shop-items";

        public const string ShopItemCategoryCollectionName = "sub-categories";

        public const string UserName = "UserName";

        public const string UserNameField = "username";

        public const string UserRoute = "api/user";

        #endregion
    }
}