using System;
using System.ComponentModel;
using Xamarin.Forms;

[Serializable]
public class Examination : INotifyPropertyChanged
{
	public string Name { get; set; }
	public int Multiplier { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;
	public bool Selected { get; private set; }
	public void SetSelected(bool value)
	{
		Selected = value;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
	}
}