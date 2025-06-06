using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;

namespace MONEY_GUARDIAN.Domain.Services.user
{
    [DomainService]
    public class UserService(
        IGenericRepository<User> userRepository
    )
    {
        public async Task<User> CreateUserAsync(
            string name,
            string userName,
            string password
        )
        {

            User User = new()
            {
                Name = name,
                Username = userName,
                Password = password
            };

            User = await userRepository.AddAsync(User);

            return User;
        }

        public async Task<User> UpdateUserAsync(
            int id,
            string name,
            string userName,
            string? password
        )
        {
            User User = await GetUserById(id);

            User.Name = name;
            User.Username = userName;

            if (password != null)
            {
                User.Password = password;
            }

            User = await userRepository.UpdateAsync(User);

            return User;
        }

        public async Task<User> ValidLoginUserAsync(
            string userName,
            string password
        )
        {
            User? user = (await userRepository.GetAsync(user => user.Username == userName && user.Password == password)).FirstOrDefault();

            return user ?? throw new AppException(MessagesExceptions.InvalidCredential);
        }

        public async Task<User> RegisterUserGoogleAsync(
            string userName,
            bool IsUserGoogle
        )
        {
            var listUser = (await userRepository.GetAsync(x => x.Username == userName)).ToList();

            if (listUser.Count != 0)
            {
                return listUser.FirstOrDefault()!;
            }

            User user = new()
            {
                Username = userName,
                Name = userName,
                IsUserGoogle = IsUserGoogle
            };

            user = await userRepository.AddAsync(user);

            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            User? User = await userRepository.GetByIdAsync(id);

            return User ?? throw new AppException(MessagesExceptions.NotExistUser);
        }
    }
}
