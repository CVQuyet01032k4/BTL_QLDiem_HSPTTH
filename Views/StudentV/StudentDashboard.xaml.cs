using BTL_QLDiem_HSPTTH.Helpers;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Views;
using BTL_QLDiem_HSPTTH.Views.StudentV;
using BTL_QLDiem_HSPTTH.Views.TeacherV;

namespace BTL_QLDiem_HSPTTH.Views.StudentV
{
    public partial class StudentDashboard : ContentPage
    {
        private readonly UserSessionService _session;

        public StudentDashboard()
        {
            InitializeComponent();

            _session = App.Current.Handler.MauiContext.Services.GetService<UserSessionService>();
            var user = _session.CurrentUser;

            Avatar.Text = user.Username.Substring(0, 1).ToUpper();
            TenUser.Text = user.Username;
        }

        private async Task Tap(View v)
        {
            await AnimationHelper.ScaleTap(v);
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            _session.Logout();
            Application.Current.MainPage = new NavigationPage(App.Current.Handler.MauiContext.Services.GetService<Login>());
        }

        private async void Xemdiem_Tapped(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new Xemdiem());
        }

        private async void Bieudo_Tapped(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new Bieudo());
        }

        private async void ThongBao_Tapped(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new TeacherV.Thongbao());
        }
    }
}
