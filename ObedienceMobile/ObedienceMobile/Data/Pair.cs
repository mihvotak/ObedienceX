using System;
using System.Collections.Generic;
using System.ComponentModel;

[Serializable]
public class Pair: INotifyPropertyChanged
{
	public enum DogGender { Male, Female }
	public enum ResultMark { None, Good, VeryGood, Excelent }

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

	public float[] Marks { get; set; }
	public float Sum { get; set; }
	public ResultMark Result { get; set; }
	public int Place { get; set; }

	public List<string> GendersList
	{
		get
		{
			return new List<string> { "Кобель", "Сука" };
		}
	}

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
	public int Number { get; private set; }
	public void SetNumber(int value)
	{
		Number = value;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
	}

	public void DispatchChanges()
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Handler)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DogName)));
	}
}