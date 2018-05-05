using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Titanium.Web.Proxy.Http;
using WebProxy.Controllers.EventArguments;
using WebProxy.MVVM;

namespace WebProxy.ViewModels
{
	/// <summary>
	///		ViewModel para el ListView de sesiones
	/// </summary>
	public class SessionListViewModel : BaseObservableObject
	{
		// Variables privadas
        private readonly Dictionary<Guid, SessionListItemViewModel> _sessions = new Dictionary<Guid, SessionListItemViewModel>();
        private SessionListItemViewModel _selectedSession;
		private ObservableCollection<SessionListItemViewModel> _items =  new ObservableCollection<SessionListItemViewModel>();
		private int _sessionsNumber;

		public SessionListViewModel()
		{
		}

		/// <summary>
		///		Añade los datos de una sesión
		/// </summary>
		public SessionListItemViewModel Add(NewSessionEventArgs e)
		{
			var item = new SessionListItemViewModel();

				// Añade la sesión al diccionario
				_sessions.Add(e.Session.Id, item);
				// Asigna los valores al elemento
				item.InitViewModel(++_sessionsNumber, e);
				// Añade la sesión
				Items.Add(item);
				// Y lo devuelve
				return item;
		}

		/// <summary>
		///		Modifica los datos de una sesión
		/// </summary>
		internal void UpdateSession(UpdateSessionEventArgs args)
		{
			if (_sessions.TryGetValue(args.Session.Id, out SessionListItemViewModel item))
			{
				// Modifica el elemento
				item.Update(args.Session);
				// y lo quita del diccionario
				_sessions.Remove(args.Session.Id);
			}
		}

		/// <summary>
		///		Lista de sesiones
		/// </summary>
        public ObservableCollection<SessionListItemViewModel> Items 
		{ 
			get { return _items; }
			set { CheckObject(ref _items, value); }
		}

		/// <summary>
		///		Sesión seleccionada
		/// </summary>
        public SessionListItemViewModel SelectedSession
        {
            get { return _selectedSession; }
            set { CheckObject(ref _selectedSession, value); }
		}
	}
}
