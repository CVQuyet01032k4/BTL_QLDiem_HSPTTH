namespace BTL_QLDiem_HSPTTH.Views.TeacherV;

public partial class Thongbao : ContentPage
{
	public Thongbao()
	{
		InitializeComponent();
	}

    private void Picker_LopChanged(object sender, EventArgs e)
    {
        if (BindingContext is TeacherVM.ThongbaoVM vm)
        {
            vm.LoadHocsinhCommand.Execute(null);
        }
    }
}
