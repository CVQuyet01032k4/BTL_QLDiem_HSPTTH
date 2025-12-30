namespace BTL_QLDiem_HSPTTH.Helpers;

public static class AnimationHelper
{
    public static async Task ScaleTap(View view)
    {
        await view.ScaleTo(0.95, 80);
        await view.ScaleTo(1, 80);
    }
}
