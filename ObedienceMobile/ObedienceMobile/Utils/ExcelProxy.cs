using ObedienceX.Data;
using OfficeOpenXml;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace ObedienceX.Utils
{
	public class ExcelProxy
	{
		public Competition ReadExcel(string fileName)
		{
			Competition competition = new Competition();

			competition.Name = Path.GetFileNameWithoutExtension(fileName);
			competition.ExcelName = fileName;

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage(fileName))
			{
				var sheet = package.Workbook.Worksheets[0];
				int index = 0;
				int startColumn = 12;
				try
				{
					while (true)
					{
						var cell1 = sheet.Cells[1, startColumn + index];
						var cell2 = sheet.Cells[2, startColumn + index];
						var cell3 = sheet.Cells[3, startColumn + index];
						if (cell1 == null || cell1.Value == null
							|| cell2 == null || cell2.Value == null
							|| cell3 == null || cell3.Value == null)
							break;
						int num = Convert.ToInt32((double)(cell1.Value));
						if (num <= 0)
							break;
						string name = (string)cell2.Value;
						if (string.IsNullOrEmpty(name) || name.Length < 2)
							break;
						int multiplier = Convert.ToInt32((double)cell3.Value);
						if (multiplier < 2 || multiplier > 4)
							break;
						competition.Examinations.Add(new Examination() { Name = name, Multiplier = multiplier });
						index++;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

				int startRow = 4;
				index = 0;
				try
				{
					string[] values = new string[12];
					while (true)
					{
						var cell1 = sheet.Cells[startRow + index, 1];
						if (cell1 == null || cell1.Value == null)
							break;
						int num = Convert.ToInt32((double)(cell1.Value));
						if (num <= 0)
							break;
						Pair pair = new Pair();
						competition.Pairs.Add(pair);

						DateTime date = DateTime.Now;
						for (int i = 2; i <= 11; i++)
						{
							var cell = sheet.Cells[startRow + index, i];
							if (i == 6)
							{
								if (cell != null && cell.Value != null)
									date = (DateTime)cell.Value;
							}
							else
							{
								string value = cell == null || cell.Value == null ? "" : (cell.Value is string ? (string)cell.Value : cell.Value.ToString());
								values[i] = value;
							}
						}
						pair.Handler = values[2] ?? "";
						pair.DogKind = values[3] ?? "";
						pair.DogName = values[4] ?? "";
						pair.Gender = values[5] == pair.GendersList[0] ? Pair.DogGender.Male : Pair.DogGender.Female;
						pair.BirthDate = date;
						pair.Pedigree = values[7] ?? "";
						pair.QualiBook = values[8] ?? "";
						pair.ChipNumber = values[9] ?? "";
						pair.Owner = values[10] ?? "";
						pair.OwnedBy = values[11] ?? "";
						pair.Marks = new ObservableCollection<Mark>();

						for (int i = 12; i < 12 + competition.Examinations.Count; i++)
						{
							var cell = sheet.Cells[startRow + index, i];
							Mark mark = new Mark();
							mark.Examination = competition.Examinations[pair.Marks.Count];
							pair.Marks.Add(mark);
							if (cell != null && cell.Value != null && cell.Value is double)
							{
								mark.Value = (double)(cell.Value);
								mark.IsSet = mark.IsValid;
							}
						}
						pair.RecalculateSum();

						index += 2;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

				try
				{
					var sheet1 = package.Workbook.Worksheets[1];
					competition.Club = sheet1.GetValue(14, 1).ToString();
					competition.Place = sheet1.GetValue(15, 3).ToString();
					competition.Level = sheet1.GetValue(24, 4).ToString();
					competition.Judge = sheet1.GetValue(27, 2).ToString();
					competition.Secretary = sheet1.GetValue(32, 2).ToString();
					competition.Director = sheet1.GetValue(35, 1).ToString();
					var dateObj = sheet1.GetValue(37, 9);
					competition.Date = dateObj is DateTime ? (DateTime)dateObj : DateTime.Now;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
			competition.RecalculateResults();
			return competition;
		}

		public void WriteExcel(string fileName)
		{
			var competition = Model.Competition;
			//competition.Name = Path.GetFileNameWithoutExtension(fileName);

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage(fileName))
			{
				var sheet = package.Workbook.Worksheets[0];
				int startColumn = 12;
				try
				{
					for (int i = 0; i < competition.Examinations.Count; i++)
					{
						sheet.Cells[1, startColumn + i].Value = i + 1;
						sheet.Cells[2, startColumn + i].Value = competition.Examinations[i].Name;
						sheet.Cells[3, startColumn + i].Value = competition.Examinations[i].Multiplier;
					}
					for (int i = 0; i < competition.Pairs.Count; i++)
					{
						Pair pair = competition.Pairs[i];
						sheet.Cells[4 + i * 2, 1].Value = i + 1;
						sheet.Cells[4 + i * 2, 2].Value = pair.Handler;
						sheet.Cells[4 + i * 2, 3].Value = pair.DogKind;
						sheet.Cells[4 + i * 2, 4].Value = pair.DogName;
						sheet.Cells[4 + i * 2, 5].Value = pair.SelectedGender;
						sheet.Cells[4 + i * 2, 6].Value = pair.BirthDate;
						sheet.Cells[4 + i * 2, 7].Value = pair.Pedigree;
						sheet.Cells[4 + i * 2, 8].Value = pair.QualiBook;
						sheet.Cells[4 + i * 2, 9].Value = pair.ChipNumber;
						sheet.Cells[4 + i * 2, 10].Value = pair.Owner;
						sheet.Cells[4 + i * 2, 11].Value = pair.OwnedBy;
						for (int j = 0; j < pair.Marks.Count; j++)
						{
							Mark mark = pair.Marks[j];
							if (mark.IsSet)
								sheet.Cells[4 + i * 2, 12 + j].Value = mark.Value;
							else
								sheet.Cells[4 + i * 2, 12 + j].Value = null;
						}
					}
					package.Save();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				competition.ExcelName = fileName;
			}
		}
	}
}
