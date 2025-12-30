namespace BTL_QLDiem_HSPTTH.Helpers
{
    public static class DialogHelper
    {
        public static async Task<bool> Confirm(string message)
        {
            return await Application.Current.MainPage
                .DisplayAlert("Xác nhận", message, "Có", "Không");
        }
    }
}
