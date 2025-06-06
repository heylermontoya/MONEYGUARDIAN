using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Domain.Tests.DataBuilder
{
    public class UserBuilder
    {
        private int _id;
        private string _name;
        private string _username;
        private string? _password;
        private bool _isUserGoogle;

        public UserBuilder()
        {
            _id = 1;
            _name = "Default Name";
            _username = "defaultuser";
            _password = "defaultpassword";
            _isUserGoogle = false;
        }

        public User Build()
        {
            return new User
            {
                Id = _id,
                Name = _name,
                Username = _username,
                Password = _password,
                IsUserGoogle = _isUserGoogle
            };
        }

        public UserBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public UserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UserBuilder WithUsername(string username)
        {
            _username = username;
            return this;
        }

        public UserBuilder WithPassword(string? password)
        {
            _password = password;
            return this;
        }

        public UserBuilder WithIsUserGoogle(bool isUserGoogle)
        {
            _isUserGoogle = isUserGoogle;
            return this;
        }
    }
}
