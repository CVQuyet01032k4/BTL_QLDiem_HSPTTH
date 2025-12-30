using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace BTL_QLDiem_HSPTTH.Services
{
    public class ToastService
    {
        public async Task ShowSuccess(string message)
        {
            var toast = Toast.Make(message, ToastDuration.Short, 14);
            await toast.Show();
        }

        public async Task ShowError(string message)
        {
            var toast = Toast.Make(message, ToastDuration.Short, 14);
            await toast.Show();
        }

        public async Task ShowInfo(string message)
        {
            var toast = Toast.Make(message, ToastDuration.Short, 14);
            await toast.Show();
        }
    }
}

