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
				int startColumn12 = 12;
				int startColumn = startColumn12;
				int startRow = 4;
				int judgesCount = 1;
				bool started = false;
				try
				{
					while (true)
					{
						var cell1 = sheet.Cells[1, startColumn + index];
						var cell2 = sheet.Cells[2, startColumn + index];
						var cell3 = sheet.Cells[3, startColumn + index];
						if (cell1 == null
							|| cell2 == null
							|| cell3 == null)
							break;
						int num = cell1.Value == null ? 0 : Convert.ToInt32((double)(cell1.Value));
						string name = cell2.Value == null ? "" : (string)cell2.Value;
						int multiplier = cell3.Value == null ? 0 : Convert.ToInt32((double)cell3.Value);
						if (!started && num == 0)
						{
							startColumn++;
							if (startColumn > 32)
							{
								startColumn = startColumn12;
								break;
							}
							continue;
						}
						else
							started = true;
						if (num <= 0)
							break;
						if (string.IsNullOrEmpty(name) || name.Length < 2)
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
					int down = 1;
					if (startColumn > startColumn12)
					{
						while (true)
						{
							var cell1 = sheet.Cells[startRow + down, 1];
							if (cell1 == null)
								break;
							if (cell1.Value != null)
							{
								int num = Convert.ToInt32((double)(cell1.Value));
								if (num > 0)
								{
									if (down > 1)
										judgesCount = down - 1;
									break;
								}
							}
							down++;
							if (down - 1 > Competition.MaxJudgesCount)
								break;
						}
						var cell3 = sheet.Cells[3, startColumn - 1];
						if (cell3 != null && cell3.Value != null)
						{
							int num = Convert.ToInt32((double)(cell3.Value));
							if (num > 0)
							{
								judgesCount = num;
							}
						}
						for (int i = 0; i < judgesCount; i++)
						{
							var cellJ = sheet.Cells[startRow, startColumn - 1];
							string name = cellJ?.Value as string;
							Judge judge = new Judge() { Name = name };
							if (i > competition.Judges.Count - 1)
								competition.Judges.Add(judge);
                            else
								competition.Judges[i] = judge;
						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				int lastExamColNum = startColumn + index - 1;

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
						Pair pair = new Pair() { Competition = competition };
						competition.Pairs.Add(pair);

						DateTime date = DateTime.Now;
						pair.StartNumber = competition.Pairs.Count;
						for (int i = 1; i <= 11; i++)
						{
							var cell = sheet.Cells[startRow + index, i];
							if (i == 1)
							{
								try
								{
									var doubleValue = (double)cell.Value;
									pair.StartNumber = (int)doubleValue;
								}
								catch (Exception e) 
								{
									Console.WriteLine(e.Message);
								}
							}
							else if (i == 6)
							{
								try
								{
									if (cell != null && cell.Value != null)
									date = (DateTime)cell.Value;
								}
								catch (Exception e)
								{
									Console.WriteLine(e.Message);
								}
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
						pair.Marks = new ObservableCollection<MarksSet>();

						for (int i = startColumn; i < startColumn + competition.Examinations.Count; i++)
						{
							MarksSet marksSet = new MarksSet();
							marksSet.Examination = competition.Examinations[pair.Marks.Count];
							pair.Marks.Add(marksSet);
							for (int j = 0; j < competition.JudgesCount; j++)
							{
								var cell = sheet.Cells[startRow + index + j, i];
								Mark mark = new Mark();
								marksSet.Marks.Add(mark);
								if (cell != null && cell.Value != null && cell.Value is double)
								{
									mark.Value = (double)(cell.Value);
									mark.IsSet = mark.IsValid;
									marksSet.IsSet |= mark.IsSet;
								}
							}
							marksSet.RecalculateMidValue(false);
						}
						{
							int colIndex = lastExamColNum + 1;
							var cell = sheet.Cells[startRow + index, colIndex];
							if (cell != null && cell.Value != null && cell.Value is double)
							{
								pair.PenaltyValue = (double)(cell.Value);
								pair.PenaltyIsSet = pair.PenaltyValue >= 0;
							}
							var cellS = sheet.Cells[startRow + index + 1, colIndex];
							if (cellS != null && cellS.Value != null && cellS.Value is string)
							{
								pair.SpecialStatus = (string)(cellS.Value);
							}
						}
						pair.RecalculateSum();

						index += judgesCount + 1;
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
			competition.Saved = true;
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
				int judgesCount = competition.JudgesCount;
				if (!File.Exists(fileName))
				{
					var assembly = Assembly.GetExecutingAssembly();
					string name = judgesCount > 1 ? "Template" + judgesCount : "Template";
					var resName = assembly.GetName().Name.ToString() + ".Res." + name + ".xlsx";
					templateStream = assembly.GetManifestResourceStream(resName);
					resultStream = File.Create(fileName);
				}


				using (var package = templateStream == null ? new ExcelPackage(fileName) : new ExcelPackage(resultStream, templateStream))
				{
					if (package.Workbook.Worksheets.Count == 0)
					{
						File.Delete(fileName);
						throw new Exception("Попытка сохранить в пустой файл. Попробуйте снова.");
					}
					var sheet = package.Workbook.Worksheets[0];
					int startColumn = judgesCount > 1 ? 13 : 12;
					int lastColNum = startColumn;
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
					double rowHeight1 = 0;
					double rowHeight2 = 0;
					ExcelStyle style1 = null;
					ExcelStyle style2 = null;
					string formula = null;
					ExcelStyle[] styles = new ExcelStyle[startColumn];
					int delta = judgesCount + 1;
					for (int i = 0; i < competition.Pairs.Count; i++)
					{
						int rowNum = 4 + i * (judgesCount + 1);
						if (i == 0)
						{
							rowHeight1 = sheet.Rows[rowNum + 0].Height;
							rowHeight2 = sheet.Rows[rowNum + judgesCount].Height;
							style1 = sheet.Cells[rowNum + 0, startColumn].Style;
							style2 = sheet.Cells[rowNum + judgesCount, startColumn].Style;
							for (int j = 1; j <= startColumn - 1; j++)
								styles[j] = sheet.Cells[rowNum, j].Style;
							formula = sheet.Cells[rowNum + judgesCount, startColumn].Formula;
						}
						else
						{
							sheet.Rows[rowNum].Height = rowHeight1;
							for (int j = 1; j <= startColumn - 1; j++)
								CopyStyle(sheet.Cells[rowNum, j], styles[j]);
							sheet.Cells[rowNum + 0, 6].Style.Numberformat.Format = styles[6].Numberformat.Format;
							for (int j = 12; j <= lastColNum; j++)
							{
								for (int k = 0; k < judgesCount; k++)
									CopyStyle(sheet.Cells[rowNum + k, j], sheet.Cells[rowNum - delta + k, j].Style);
								CopyStyle(sheet.Cells[rowNum + judgesCount, j], sheet.Cells[rowNum - delta + judgesCount, j].Style);
								for (int k = 0; k < judgesCount; k++)
									sheet.Cells[rowNum + k, j].FormulaR1C1 = sheet.Cells[rowNum - delta + k, j].FormulaR1C1;
								sheet.Cells[rowNum + judgesCount, j].FormulaR1C1 = sheet.Cells[rowNum - delta + judgesCount, j].FormulaR1C1;
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
						if (judgesCount > 1)
							for (int j = 0; j < judgesCount; j++)
								sheet.Cells[rowNum + j, 12].Value = competition.Judges[j].Name;
						for (int j = startColumn; j < startColumn + pair.Marks.Count; j++)
						{
							var marksSet = pair.Marks[j - startColumn];
							for (int k = 0; k < judgesCount; k++)
							{
								Mark mark = marksSet.Marks[k];
								if (mark.IsSet)
									sheet.Cells[rowNum + k, j].Value = mark.Value;
								else
									sheet.Cells[rowNum + k, j].Value = null;
							}
						}
						for (int j = startColumn + pair.Marks.Count; j <= lastMarkColNum; j++)
						{
							for (int k = 0; k < judgesCount + 1; k++)
								sheet.Cells[rowNum + k, j].Value = null;
						}
						{
							int j = lastMarkColNum + 1;
							if (pair.PenaltyIsSet)
								sheet.Cells[rowNum, j].Value = pair.PenaltyValue;
							else
								sheet.Cells[rowNum, j].Value = null;
							if (!string.IsNullOrEmpty(pair.SpecialStatus))
								sheet.Cells[rowNum + judgesCount, j].Value = pair.SpecialStatus;
							else
								sheet.Cells[rowNum + judgesCount, j].Value = null;
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
					competition.Saved = true;
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
