using BTL_QLDiem_HSPTTH.ViewModels;

namespace BTL_QLDiem_HSPTTH.Views;

public partial class Login : ContentPage
{
    public Login(LoginVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
