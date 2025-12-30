using BTL_QLDiem_HSPTTH.Helpers;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Views;
using BTL_QLDiem_HSPTTH.Views.AdminV;

namespace BTL_QLDiem_HSPTTH.Views.AdminV
{
    public partial class AdminDashboard : ContentPage
    {
        private readonly UserSessionService _session;

        public AdminDashboard()
        {
            InitializeComponent();

            _session = App.Current.Handler.MauiContext.Services.GetService<UserSessionService>();
            var user = _session.CurrentUser;

            Avatar.Text = user.Username.Substring(0, 1).ToUpper();
            TenUser.Text = user.Username;
            VaiTro.Text = "Quản trị viên";
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            _session.Logout();
            Application.Current.MainPage = new NavigationPage(App.Current.Handler.MauiContext.Services.GetService<Login>());
        }

        private async Task Tap(View view)
        {
            await AnimationHelper.ScaleTap(view);
        }

        private async void Go_QuanlyLop(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new QuanlyLop());
        }

        private async void Go_QuanlyMon(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new QuanlyMon());
        }

        private async void Go_QuanlyGiaovien(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new QuanlyGiaovien());
        }

        private async void Go_QuanlyHocsinh(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new QuanlyHocsinh());
        }

        private async void Go_Phancong(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new Phancong());
        }

        private async void Go_Baocao(object sender, TappedEventArgs e)
        {
            await Tap((View)sender);
            await Navigation.PushAsync(new Baocao());
        }
    }
}
