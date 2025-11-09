using Inmobiliaria_api_mobile.Data;
using Inmobiliaria_api_mobile.Models;
using Inmobiliaria_api_mobile.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria_api_mobile.Repositories.Implementations;

public class UserRepository(AppCntxt context) : IUserRepository
{
    private readonly AppCntxt _context = context;
    private readonly PasswordHasher<User> _hasher = new();

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> AddAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.Password = _hasher.HashPassword(user, user.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
