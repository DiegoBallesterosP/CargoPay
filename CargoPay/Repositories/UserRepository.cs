using CargoPay.Entities;
using CargoPay.Infrastructure;
using CargoPay.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
    }
}