using BTL_QLDiem_HSPTTH.TeacherVM;
using BTL_QLDiem_HSPTTH.Data.Models;
using System;
using LoaiDiem = BTL_QLDiem_HSPTTH.Data.Models.LoaiDiem;

namespace BTL_QLDiem_HSPTTH.Views.TeacherV;

public partial class Nhapdiem : ContentPage
{
	public Nhapdiem()
	{
		InitializeComponent();
	}

    private void Picker_PhancongChanged(object sender, EventArgs e)
    {
        if (BindingContext is NhapdiemVM vm)
        {
            vm.LoadHocsinhCommand.Execute(null);
        }
    }

    private void Picker_HocsinhChanged(object sender, EventArgs e)
    {
        if (BindingContext is NhapdiemVM vm)
        {
            vm.LoadDiemCommand.Execute(null);
        }
    }

    private void Picker_LoaiDiemChanged(object sender, EventArgs e)
    {
        if (BindingContext is NhapdiemVM vm && LoaiDiemPicker.SelectedIndex >= 0)
        {
            vm.LoaiDiemSelected = (LoaiDiem)(LoaiDiemPicker.SelectedIndex + 1);
            // Set hệ số mặc định
            var heSo = vm.LoaiDiemSelected switch
            {
                LoaiDiem.Miemg => 1,
                LoaiDiem.MuoiLamPhut => 1,
                LoaiDiem.MotTiet => 2,
                LoaiDiem.Thi => 3,
                _ => 1
            };
            vm.SetHeSo(heSo);
        }
    }
}
