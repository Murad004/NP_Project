using NP_Project.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP_Project.MVVM.ViewModel
{
    public class MainViewModel
    {
        public RelayCommand RunServerCommand { get; set; }

        public MainViewModel(MainWindow mainWindow)
        {

        }


        
    }
}
