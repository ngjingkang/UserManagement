using Domain.Entities;
using Domain.Requests;
using Service.Services.Users.Commands;
using static Domain.Entities.User;

namespace Service.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>?> GetUsersAsync();
        Task<bool> AddUserAsync(AddUserRequest user);
        Task<bool> UpdateUserAsync(UpdateUserRequest user);
        Task<bool> DeleteUserAsync(long id);
    }
}
