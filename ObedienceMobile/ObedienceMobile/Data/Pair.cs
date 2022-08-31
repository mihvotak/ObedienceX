using Newtonsoft.Json;
using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

[Serializable]
public class Pair: INotifyPropertyChanged
{
	public enum DogGender { Male, Female }

	[JsonIgnore]
	public Competition Competition { get; set; }

	public string Handler { get; set; }
	public string Owner { get; set; }
	public string OwnedBy { get; set; }
	public string DogKind { get; set; }
	public string DogName { get; set; }
	public DogGender Gender { get; set; }
	public DateTime BirthDate { get; set; }
	public string Pedigree { get; set; }
	public string QualiBook { get; set; }
	public string ChipNumber { get; set; }

	[JsonIgnore]
	public string DogKindAndName { get { return DogKind + "\n" + DogName; } }

	public ObservableCollection<Mark> Marks { get; set; }
	public float Sum { get; set; }
	[JsonIgnore]
	public string Result
	{ 
		get 
		{
			if (Sum >= 256)
				return "Отлично";
			else if (Sum >= 224)
				return "Очень хорошо";
			else if (Sum >= 192)
				return "Хорошо";
			else
				return "---";
		} 
	}
	public int Place { get; set; }

	static public List<string> AllGendersList
	{
		get
		{
			return new List<string> { "Кобель", "Сука" };
		}
	}

	[JsonIgnore]
	public List<string> GendersList { get { return AllGendersList; } }

	[JsonIgnore]
	public string SelectedBreed
	{
		get
		{
			return Gender == Pair.DogGender.Male ? GendersList[0] : GendersList[1];
		}
		set
		{
			if (value == GendersList[0])
				Gender = Pair.DogGender.Male;
			else
				Gender = Pair.DogGender.Female;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	[JsonIgnore]
	public int Number { get; private set; }
	public void SetNumber(int value)
	{
		Number = value;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
	}

	public void DispatchNamesChanged()
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Handler)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DogName)));
	}

	public void DispatchPlaceChanged()
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Place)));
	}

	public void RecalculateSum()
	{
		float sum = 0;
		for (int i = 0; i < Marks.Count; i++)
			sum += Marks[i].MultipliedValue;
		Sum = sum;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sum)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Place)));
		//Competition.RecalculateResults();
	}
}