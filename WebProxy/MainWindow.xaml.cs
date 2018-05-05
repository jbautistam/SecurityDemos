using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WebProxy
{
	/// <summary>
	///		Ventana principal del proxy
	/// </summary>
	public partial class MainWindow : Window
	{
        public MainWindow()
        {
			// Inicializa los componentes
            InitializeComponent();
			// Inicializa el ViewModel y arranca el proxy
			grdMain.DataContext = ViewModel = new ViewModels.MainViewModel(Dispatcher);
			ViewModel.Start();
        }

		/// <summary>
		///		Sale de la aplicación
		/// </summary>
		private void ExitApp()
		{
			ViewModel.Stop();
		}

		/// <summary>
		///		ViewModel de la ventana principal
		/// </summary>
		public ViewModels.MainViewModel ViewModel { get; set; }

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ExitApp();
		}
	}
}
