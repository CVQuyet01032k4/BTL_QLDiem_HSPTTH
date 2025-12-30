using System;
using BTL_QLDiem_HSPTTH.StudentVM;

namespace BTL_QLDiem_HSPTTH.Views.StudentV;

public partial class Bieudo : ContentPage
{
	public Bieudo()
	{
		InitializeComponent();
	}

    private void Picker_Changed(object sender, EventArgs e)
    {
        if (BindingContext is BieudoVM vm)
        {
            vm.LoadDataCommand.Execute(null);
        }
    }
}
