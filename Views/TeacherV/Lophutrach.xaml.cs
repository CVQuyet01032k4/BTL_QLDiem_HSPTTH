using BTL_QLDiem_HSPTTH.TeacherVM;

namespace BTL_QLDiem_HSPTTH.Views.TeacherV;

public partial class Lophutrach : ContentPage
{
	public Lophutrach()
	{
		InitializeComponent();
	}

    private void Picker_Changed(object sender, EventArgs e)
    {
        if (BindingContext is LophutrachVM vm && vm.PhancongSelected != null)
        {
            vm.LoadHocsinhCommand.Execute(null);
        }
    }
}
