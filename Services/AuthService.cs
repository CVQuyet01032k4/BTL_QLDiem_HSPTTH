using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;

namespace BTL_QLDiem_HSPTTH.Services
{
    public class AuthService
    {
        private readonly DatabaseService _db;

        public AuthService(DatabaseService db)
        {
            _db = db;
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            return _db.GetUser(username, password);
        }
    }
}

