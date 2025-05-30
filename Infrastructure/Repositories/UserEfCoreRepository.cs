using Domain.Entities;
using Domain.Requests;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserEfCoreRepository(
         DbConn dbConn, LocalDbContext dbContext) : IUserRepository
    {
        public async Task<bool> AddUserAsync(AddUserRequest user)
        {
            int rowAffected = 0;

            var newUser = new User
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy
            };
            await dbContext.Users.AddAsync(newUser);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>?> GetUsersAsync()
         => await dbContext.Users
            .AsNoTracking()
            .ToListAsync();

        public async Task<bool> UpdateUserAsync(UpdateUserRequest user)
        {
            var data = await dbContext.Users.FindAsync(user.Id);

            data.Name = user.Name;
            data.Age = user.Age;
            data.Email = user.Email;
            data.IsActive = user.IsActive;
            data.ModifiedOn = user.ModifiedOn;
            data.ModifiedBy = user.ModifiedBy;

            var rowAffected = await dbContext.SaveChangesAsync();

            return rowAffected > 0;
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
                return false;

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
