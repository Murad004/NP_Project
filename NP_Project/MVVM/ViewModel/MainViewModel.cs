using NP_Project.Commands;
using NP_Project.Helpers;
using NP_Project.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NP_Project.MVVM.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Props
        public string PathFile { get; set; }
        public string Username { get; set; }
        #endregion
        #region PropFull
        private PDF selectedItemPdf;

        public PDF SelectedItemPdf
        {
            get { return selectedItemPdf; }
            set { selectedItemPdf = value; OnPropertyChanged(); }
        }

        private Image selectedItemImage;

        public Image SelectedItemImage
        {
            get { return selectedItemImage; }
            set { selectedItemImage = value; OnPropertyChanged(); }
        }


        #endregion

        #region ServerNetworkField
        public TcpListener TcpListener;
        IPEndPoint endPoint;
        BinaryReader BinaryReader;
        #endregion

        #region RelayCommands

        public RelayCommand RunServerCommand { get; set; }
        public RelayCommand OpenCommand { get; set; }
        #endregion

        public MainViewModel(MainWindow mainWindow)
        {
            RunServerCommand = new RelayCommand((sender) =>
              {

                  Task.Run(() =>
                  {

                      endPoint = new IPEndPoint(IPAddress.Parse(EndPointHelper.IP), EndPointHelper.PORT);
                      TcpListener = new TcpListener(endPoint);
                      TcpListener.Start();

                      while (true)
                      {
                          var client = TcpListener.AcceptTcpClient();
                          MessageBox.Show($"{client.Client.RemoteEndPoint} connected");
                          byte[] bytes = new byte[50000000];



                          var reader = Task.Run(() =>
                          {

                              var stream = client.GetStream();
                              BinaryReader = new BinaryReader(stream);

                              while (true)
                              {

                                  PathFile = BinaryReader.ReadString();
                                  Username = BinaryReader.ReadString();

                                  if (".pdf".Equals(Path.GetExtension(PathFile), StringComparison.OrdinalIgnoreCase))
                                  {

                                      App.Current.Dispatcher.Invoke(() =>
                                      {

                                          mainWindow.MyPdfListBox.Items.Add(new PDF { PdfPath = PathFile, Username = $"Name : {Username}" });

                                      });

                                  }
                                  else
                                  {
                                      App.Current.Dispatcher.Invoke(() =>
                                      {

                                          mainWindow.MyImageListBox.Items.Add(new Image { ImagePath = PathFile, Username = $"Sender Name : {Username}" });


                                      });
                                  }
                                  break;

                              }

                          });

                      };
                  });
              });

            OpenCommand = new RelayCommand((sender) =>
              {
                  try
                  {


                      if (mainWindow.MyPdfListBox.Items.Count > 0 && SelectedItemPdf != null)
                      {
                          Process.Start(SelectedItemPdf.PdfPath);
                          //mainWindow.MyPdfListBox.SelectedItem = null;
                      }
                      else if (mainWindow.MyImageListBox.Items.Count > 0 && SelectedItemImage != null)
                      {
                          Process.Start(SelectedItemImage.ImagePath);
                          //mainWindow.MyImageListBox.SelectedItem = null;
                      }




                  }
                  catch (Exception ex)
                  {
                  }
              });
        }



    }
}
