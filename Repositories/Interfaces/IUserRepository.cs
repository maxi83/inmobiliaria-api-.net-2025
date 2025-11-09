using Inmobiliaria_api_mobile.Models;

namespace Inmobiliaria_api_mobile.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User> AddAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}
