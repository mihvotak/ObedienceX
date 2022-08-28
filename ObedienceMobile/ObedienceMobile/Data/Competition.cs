using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Serializable]
public class Competition
{
	public string FileName { get; set; }
	public string Name { get; set; }
	public DateTime Date { get; set; }
	public int Level { get; set; }
	public string Club { get; set; }
	public string Place { get; set; }
	public string Director { get; set; }
	public string Judge { get; set; }
	public string Secretary { get; set; }
	public ObservableCollection<Examination> Examinations = new ObservableCollection<Examination>();
	public ObservableCollection<Pair> Pairs = new ObservableCollection<Pair>();
	public int LastPair { get; set; }
}