using ObedienceX.Data;
using OfficeOpenXml;
using OfficeOpenXml.Core;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ObedienceX.Utils
{
	public class ExcelProxy
	{
		public Competition ReadExcel(string fileName)
		{
			Competition competition = new Competition();

			competition.ExcelName = fileName;

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage(fileName))
			{
				if (package == null || package.Workbook == null || package.Workbook.Worksheets == null || package.Workbook.Worksheets.Count == 0)
				{
					LastError = "Не найдено ни одной таблицы";
					return null;
				}	
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
					while (true)
					{
						var cell1 = sheet.Cells[1, startColumn + index];
						if (cell1 == null || cell1.Value == null)
							break;
						index++;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				int lastExamColNum = startColumn + index - 1;

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
						{
							int rowIndex = lastExamColNum + 1;
							var cell = sheet.Cells[startRow + index, rowIndex];
							if (cell != null && cell.Value != null && cell.Value is double)
							{
								pair.PenaltyValue = (double)(cell.Value);
								pair.PenaltyIsSet = pair.PenaltyValue >= 0;
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

		private void CopyStyle(ExcelRange range, ExcelStyle style)
		{
			range.Style.Font.Size = style.Font.Size;
			range.Style.Font.Bold = style.Font.Bold;
			range.Style.Font.Family = style.Font.Family;
			range.Style.Border.BorderAround(style.Border.Top.Style);
			range.Style.Fill.PatternType = style.Fill.PatternType;
			range.Style.Fill.SetBackground(Color.White);
		}

		public bool WriteExcel(string fileName)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			var competition = Model.Competition;
			string templateFile = Model.Competition.ExcelName == null ? App.ExcelTemplate : competition.ExcelName;
			//competition.Name = Path.GetFileNameWithoutExtension(fileName);

			try
			{
				Stream templateStream = null;
				Stream resultStream = null;
				if (!File.Exists(fileName))
				{
					var assembly = Assembly.GetExecutingAssembly();
					var resName = assembly.GetName().Name.ToString() + ".Res.Template.xlsx";
					templateStream = assembly.GetManifestResourceStream(resName);
					resultStream = File.Create(fileName);
				}


				using (var package = templateStream == null ? new ExcelPackage(fileName) : new ExcelPackage(resultStream, templateStream))
				{
					var sheet = package.Workbook.Worksheets[0];
					int startColumn = 12;
					int lastColNum = 12;
					for (int i = 0; i < competition.Examinations.Count; i++)
					{
						sheet.Cells[1, startColumn + i].Value = i + 1;
						sheet.Cells[2, startColumn + i].Value = competition.Examinations[i].Name;
						sheet.Cells[3, startColumn + i].Value = competition.Examinations[i].Multiplier;
						lastColNum = startColumn + i;
					}
					for (int i = competition.Examinations.Count; i < Competition.MaxExamsCount; i++)
					{
						if (sheet.Cells[1, startColumn + i].Value != null)
						{
							sheet.Cells[1, startColumn + i].Value = i + 1;
							sheet.Cells[2, startColumn + i].Value = null;
							sheet.Cells[3, startColumn + i].Value = null;
							lastColNum = startColumn + i;
						}
					}
					int lastMarkColNum = lastColNum;
					while (true)
					{
						var cell = sheet.Cells[2, lastColNum + 1];
						if (cell != null && cell.Value != null)
							lastColNum++;
						else
							break;
					}
					ExcelStyle[] styles = new ExcelStyle[12];
					double rowHeight1 = 0;
					double rowHeight2 = 0;
					ExcelStyle style1 = null;
					ExcelStyle style2 = null;
					string formula = null;
					for (int i = 0; i < competition.Pairs.Count; i++)
					{
						int rowNum = 4 + i * 2;
						if (i == 0)
						{
							rowHeight1 = sheet.Rows[rowNum + 0].Height;
							rowHeight2 = sheet.Rows[rowNum + 1].Height;
							style1 = sheet.Cells[rowNum + 0, 12].Style;
							style2 = sheet.Cells[rowNum + 1, 12].Style;
							for (int j = 1; j <= 11; j++)
								styles[j] = sheet.Cells[rowNum, j].Style;
							formula = sheet.Cells[rowNum + 1, 12].Formula;
						}
						else
						{
							sheet.Rows[rowNum].Height = rowHeight1;
							for (int j = 1; j <= 11; j++)
								CopyStyle(sheet.Cells[rowNum, j], styles[j]);
							sheet.Cells[rowNum + 0, 6].Style.Numberformat.Format = styles[6].Numberformat.Format;
							for (int j = 12; j <= lastColNum; j++)
							{
								CopyStyle(sheet.Cells[rowNum + 0, j], sheet.Cells[rowNum - 2 + 0, j].Style);
								CopyStyle(sheet.Cells[rowNum + 1, j], sheet.Cells[rowNum - 2 + 1, j].Style);
								sheet.Cells[rowNum + 0, j].FormulaR1C1 = sheet.Cells[rowNum - 2 + 0, j].FormulaR1C1;
								sheet.Cells[rowNum + 1, j].FormulaR1C1 = sheet.Cells[rowNum - 2 + 1, j].FormulaR1C1;
							}
						}
						Pair pair = competition.Pairs[i];
						sheet.Cells[rowNum, 1].Value = i + 1;
						sheet.Cells[rowNum, 2].Value = pair.Handler;
						sheet.Cells[rowNum, 3].Value = pair.DogKind;
						sheet.Cells[rowNum, 4].Value = pair.DogName;
						sheet.Cells[rowNum, 5].Value = pair.SelectedGender;
						sheet.Cells[rowNum, 6].Value = pair.BirthDate;
						sheet.Cells[rowNum, 7].Value = pair.Pedigree;
						sheet.Cells[rowNum, 8].Value = pair.QualiBook;
						sheet.Cells[rowNum, 9].Value = pair.ChipNumber;
						sheet.Cells[rowNum, 10].Value = pair.Owner;
						sheet.Cells[rowNum, 11].Value = pair.OwnedBy;
						for (int j = 12; j < 12 + pair.Marks.Count; j++)
						{
							Mark mark = pair.Marks[j - 12];
							if (mark.IsSet)
								sheet.Cells[rowNum, j].Value = mark.Value;
							else
								sheet.Cells[rowNum, j].Value = null;
						}
						for (int j = 12 + pair.Marks.Count; j <= lastMarkColNum; j++)
						{
							sheet.Cells[rowNum + 0, j].Value = null;
							sheet.Cells[rowNum + 1, j].Value = null;
						}
						{
							int j = lastMarkColNum + 1;
							if (pair.PenaltyIsSet)
								sheet.Cells[rowNum, j].Value = pair.PenaltyValue;
							else
								sheet.Cells[rowNum, j].Value = null;
						}
					}

					sheet = package.Workbook.Worksheets[1];
					sheet.Cells[14, 1].Value = competition.Club;
					sheet.Cells[15, 3].Value = competition.Place;
					sheet.Cells[24, 4].Value = competition.Level;
					sheet.Cells[27, 2].Value = competition.Judge;
					sheet.Cells[32, 2].Value = competition.Secretary;
					sheet.Cells[35, 1].Value = competition.Director;
					sheet.Cells[37, 9].Value = competition.Date;
					sheet.Cells[37, 9].Style.Numberformat.Format = "dd.MM.yyyy";

					package.Save();
					competition.ExcelName = fileName;
					if (templateStream != null)
						templateStream.Close();
					if (resultStream != null)
						resultStream.Close();
					return true;
				}
			}
			catch (Exception e)
			{
				LastError = e.Message;
				return false;
			}
		}

		public string LastError;
	}
}
