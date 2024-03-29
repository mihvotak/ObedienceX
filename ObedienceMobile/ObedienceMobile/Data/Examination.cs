﻿using Newtonsoft.Json;
using System;
using System.ComponentModel;

[Serializable]
public class Examination : INotifyPropertyChanged
{
	public string Name { get; set; }
	public int Multiplier { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	[JsonIgnore]
	public int Number { get; private set; }
	public void SetNumber(int value)
	{
		Number = value;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
	}
}