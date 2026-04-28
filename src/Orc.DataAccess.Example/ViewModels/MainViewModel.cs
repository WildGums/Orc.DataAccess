namespace Orc.DataAccess.Example.ViewModels;

using System;
using Catel.MVVM;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }

    public override string Title => "Orc.DataAccess example";
}
