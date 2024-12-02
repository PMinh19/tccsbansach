using BanSach.Components.Data;
using BanSach.Components.IService;
using BanSach.Components.Model;
using Microsoft.EntityFrameworkCore;

namespace BanSach.Components.Services
{
    public class UserService: IUserService
    {
        private readonly AppDbContext db;
        public UserService(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<List<User>> GetAllStaff()
        {
            return await db.Users.Where(u => u.RoleId == 3).ToListAsync();
        }
        public async Task<List<User>> GetAllUser()
        {
            return await db.Users.Where(u => u.RoleId == 2).ToListAsync();
        }

        public async Task<User> CreateUser(User User)
        {
            db.Users.Add(User);
            await db.SaveChangesAsync();
            return User; 
        }

        public async Task DeleteUser(User User)
        {
            db.Users.Remove(User);
            await db.SaveChangesAsync();
        }

        public async Task EditUser(User User)
        {
            db.Users.Update(User);
            await db.SaveChangesAsync();
        }
        public async Task<User?> GetUserById(int userId)
        {
            var User = await db.Users.FirstOrDefaultAsync(p => p.UserId == userId);
            if (User == null)
            {
                return null;
            }

            return User;
        }
// role
        public async Task<List<Role>> GetAllRole()
        {
            return await db.Roles.ToListAsync();
        }
        public async Task<Role> GetRoleById(int roleId)
        {
            var role = await db.Roles.FirstOrDefaultAsync(p => p.RoleId == roleId);
            if (role == null)
            {
                return null;
            }

            return role;
        }
    }
}
