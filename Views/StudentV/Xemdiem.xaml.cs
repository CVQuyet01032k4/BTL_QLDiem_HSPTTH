using System;
using BTL_QLDiem_HSPTTH.StudentVM;

namespace BTL_QLDiem_HSPTTH.Views.StudentV;

public partial class Xemdiem : ContentPage
{
	public Xemdiem()
	{
		InitializeComponent();
	}

    private void Picker_HockyChanged(object sender, EventArgs e)
    {
        if (BindingContext is XemdiemVM vm)
        {
            vm.LoadDiemCommand.Execute(null);
        }
    }
}
