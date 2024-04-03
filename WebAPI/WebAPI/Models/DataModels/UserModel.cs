// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.DataModels

{
    #region Imports

    using BCrypt.Net;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using WebAPI.Interfaces;
    using WebAPI.Properties;

    #endregion

    public class UserModel : IDataModel
    {
        #region Public Properties

        [BsonElement(Constants.RefreshTokenExpiryTimeField)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public static string CollectionName => Constants.UserCollectionName;

        [BsonElement(Constants.EmailField)]
        public string? Email { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        [BsonElement(Constants.PasswordField)]
        public string? Password { get; set; }

        [BsonElement(Constants.RefreshTokenField)]
        public string? RefreshToken { get; set; }

        [BsonElement(Constants.RoleField)]
        public string? Role { get; set; }

        [BsonElement(Constants.UserNameField)]
        public string? UserName { get; set; }

        #endregion

        #region Public Methods and Operators

        public bool IsValidPassword(string password)
        {
            return BCrypt.EnhancedVerify(password, Password);
        }

        public void ClearSensitiveData()
        {
            ID = string.Empty;
            Password = string.Empty;
        }

        public void HashPassword()
        {
            Password = BCrypt.EnhancedHashPassword(Password);
        }

        #endregion
    }
}