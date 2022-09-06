using System;
using System.IO;
using static System.Net.WebRequestMethods;

public class CompetitionFile
{
	public bool IsFile { get; set; }
	public string FileName { get; set; }
	public string ShortName { get; set; }
	public DateTime Date { get; set; }
	public string DateStr { get { return Date.ToString("dd:MM:yyyy hh:mm:ss"); } }
	public bool IsExcel { get { return Path.GetExtension(FileName) == ".xlsx"; } }
	public bool IsXls { get { return Path.GetExtension(FileName) == ".xls"; } }
	public bool IsFolder { get { return !IsFile; } }
}
