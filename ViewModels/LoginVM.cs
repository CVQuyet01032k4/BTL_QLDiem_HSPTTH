using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Views.AdminV;
using BTL_QLDiem_HSPTTH.Views.TeacherV;
using BTL_QLDiem_HSPTTH.Views.StudentV;
using BTL_QLDiem_HSPTTH.Views.ParentV;
using System.Windows.Input;

namespace BTL_QLDiem_HSPTTH.ViewModels
{
    public class LoginVM : BindableObject
    {
        private readonly AuthService _authService;
        private readonly UserSessionService _session;

        public LoginVM(AuthService authService, UserSessionService session)
        {
            _authService = authService;
            _session = session;
            LoginCommand = new Command(OnLogin);
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public ICommand LoginCommand { get; }

        private async void OnLogin()
        {
            var user = _authService.Login(Username, Password);

            if (user == null)
            {
                ErrorMessage = "Sai tài khoản hoặc mật khẩu";
                OnPropertyChanged(nameof(ErrorMessage));
                return;
            }

            _session.SetUser(user);

            Page dashboard = null;

            switch (user.Role)
            {
                case Role.Admin:
                    dashboard = new AdminDashboard();
                    break;
                case Role.Teacher:
                    dashboard = new TeacherDashboard();
                    break;
                case Role.Student:
                    dashboard = new StudentDashboard();
                    break;
                case Role.Parent:
                    dashboard = new ParentDashboard();
                    break;
            }

            Application.Current.MainPage = new NavigationPage(dashboard);
        }
    }
}
