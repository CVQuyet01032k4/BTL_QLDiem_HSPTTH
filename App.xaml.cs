using BTL_QLDiem_HSPTTH.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BTL_QLDiem_HSPTTH
{
    public partial class App : Application
    {
        public App(Login login)
        {
            InitializeComponent();
            MainPage = new NavigationPage(login);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(MainPage);
        }
    }
}