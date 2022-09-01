using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ObedienceX.Data
{
    public class SaveAsData : INotifyPropertyChanged
	{
		public string Folder { get { return App.FolderPath; } }
		public string FileName { get; set; }

		private string _error;
		public string Error { 
			get { return _error; } 
			set { 
				_error = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));

			} 
		}
	
		public event PropertyChangedEventHandler PropertyChanged;

		public void DispatchChanged()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileName)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Folder)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
		}
	}
}
