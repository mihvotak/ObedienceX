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

	public Pair()
	{
		BirthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
	}

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
	[JsonIgnore]
	public int CurrentMarkIndex { get; set; }

	public bool PenaltyIsSet;
	public double PenaltyValue;
	public string PenaltyStr { 
		get { return PenaltyIsSet ? PenaltyValue.ToString() : ""; }
		set {
			double val = 0;
			value = value.Replace(',', '.');
			bool isSet = double.TryParse(value, out val) && val > 0;
			if (val != PenaltyValue || isSet != PenaltyIsSet)
			{
				PenaltyIsSet = isSet;
				PenaltyValue = val;
				Competition.Saved = false; 
				RecalculateSum();
			}
		} 
	}
	public double Sum { get; set; }
	[JsonIgnore]
	public string Result
	{ 
		get 
		{
			if (!string.IsNullOrEmpty(SpecialStatus))
				return SpecialStatus;
			else if (Sum >= 256)
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
	public bool Unique { get; set; }
	public string PlaceStr { get { return !string.IsNullOrEmpty(SpecialStatus) || Sum == 0 ? "-" : 
				Place.ToString() + (Unique ? "" : "*"); } }

	static public List<string> AllStatusesList
	{
		get
		{
			return new List<string> { "", "ВнеЗачёта", "Неявка", "Снятие", "СнятСИзв", "Дисквал" };
		}
	}

	[JsonIgnore]
	public List<string> StatusesList { get { return AllStatusesList; } }

	private string _specialStatus = "";
	[JsonIgnore]
	public string SpecialStatus {
		get { return _specialStatus; }
		set { 
			if (_specialStatus != value)
			{
				_specialStatus = value;
				Competition.Saved = false;
				RecalculateSum();
			}
		}
	}

	static public List<string> AllGendersList
	{
		get
		{
			return new List<string> { "кобель", "сука" };
		}
	}

	[JsonIgnore]
	public List<string> GendersList { get { return AllGendersList; } }

	[JsonIgnore]
	public string SelectedGender
	{
		get
		{
			return Gender == Pair.DogGender.Male ? GendersList[0] : GendersList[1];
		}
		set
		{
			DogGender newGender = value == GendersList[0] ? DogGender.Male : DogGender.Female;
			if (newGender != Gender)
			{
				Gender = newGender;
				Competition.Saved = false;
			}
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

	public int StartNumber { 
		get; 
		set; 
	}
	public string StartNumStr { 
		get 
		{
			return StartNumber.ToString();
		}
		set
		{
			if (int.TryParse(value, out int newValue) && newValue > 0)
			{
				StartNumber = newValue;
				Competition.Saved = false;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartNumber)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartNumStr)));
			}
		}
	}

	public void DispatchNamesChanged()
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Handler)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DogName)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DogKind)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DogKindAndName)));
	}

	public void DispatchPlaceChanged()
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlaceStr)));
	}

	public void RecalculateSum()
	{
		double sum = 0;
		for (int i = 0; i < Marks.Count; i++)
			sum += Marks[i].MultipliedValue;
		sum -= PenaltyIsSet ? PenaltyValue : 0;
		if (!string.IsNullOrEmpty(SpecialStatus))
			sum = 0;
		Sum = sum;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sum)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Place)));
		//Competition.RecalculateResults();
	}

}