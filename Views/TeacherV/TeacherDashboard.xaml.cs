using BTL_QLDiem_HSPTTH.Helpers;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Views;
using BTL_QLDiem_HSPTTH.Views.TeacherV;

namespace BTL_QLDiem_HSPTTH.Views.TeacherV
{
    public partial class TeacherDashboard : ContentPage
    {
        private readonly UserSessionService _session;

        public TeacherDashboard()
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

        private async void Nhapdiem_Tapped(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new Nhapdiem());
        }

        private async void Lophutrach_Tapped(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new Lophutrach());
        }

        private async void ThongBao_Tapped(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new Thongbao());
        }
    }
}
