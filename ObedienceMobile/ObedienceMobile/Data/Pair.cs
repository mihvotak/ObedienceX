using System;

[Serializable]
public class Pair
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
}