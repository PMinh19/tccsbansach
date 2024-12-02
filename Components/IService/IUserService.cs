using BanSach.Components.Model;

namespace BanSach.Components.IService
{
    public interface IUserService
    {
        Task<List<User>> GetAllStaff();
        Task<User> CreateUser(User User);
        Task EditUser(User User);
        Task DeleteUser(User User);
        Task<User?> GetUserById(int userId);
        Task<List<Role>> GetAllRole();
        Task<Role> GetRoleById(int roleId);
        Task<List<User>> GetAllUser();
    }
}
