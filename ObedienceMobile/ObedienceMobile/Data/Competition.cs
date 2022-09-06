using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

[Serializable]
public class Competition : INotifyPropertyChanged
{
	public const int MaxExamsCount = 11;

	public Competition()
	{
		Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
	}

	//[JsonIgnore]
	//public string FileName { get; set; }
	[JsonIgnore]
	public string ExcelName { get; set; }

	[JsonIgnore]
	public string Name { get { return string.IsNullOrEmpty(ExcelName) ? "" : Path.GetFileNameWithoutExtension(ExcelName); } }
	public DateTime Date { get; set; }
	public string Level { get; set; }
	public string Club { get; set; }
	public string Place { get; set; }
	public string Director { get; set; }
	public string Judge { get; set; }
	public string Secretary { get; set; }
	public ObservableCollection<Examination> Examinations = new ObservableCollection<Examination>();
	public ObservableCollection<Pair> Pairs = new ObservableCollection<Pair>();
	public int LastPairIndex { get; set; }

	[JsonIgnore]
	public List<string> Levels { get { return AllLevels; } }
	public static List<string> AllLevels = new List<string> { "Обидиенс-0", "Обидиенс-1", "Обидиенс-2", "Обидиенс-3" };

	public event PropertyChangedEventHandler PropertyChanged;

	public void DispatchNameChanged()
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
	}

	public void RecalculateResults()
	{
		List<Pair> list = new List<Pair>();
		foreach (var pair in Pairs)
			list.Add(pair);
		list.Sort((p1, p2) => p2.Sum.CompareTo(p1.Sum));
		int place = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (i == 0 || list[i].Sum < list[i - 1].Sum)
				place = i + 1;
			if (list[i].Place != place)
			{
				list[i].Place = place;
				list[i].DispatchPlaceChanged();
			}
		}
	}

}