using BTL_QLDiem_HSPTTH.Data.Models;

namespace BTL_QLDiem_HSPTTH.Services
{
    public class UserSessionService
    {
        private User _currentUser;

        public User CurrentUser => _currentUser;
        public bool IsLoggedIn => _currentUser != null;

        public void SetUser(User user)
        {
            _currentUser = user;
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}

