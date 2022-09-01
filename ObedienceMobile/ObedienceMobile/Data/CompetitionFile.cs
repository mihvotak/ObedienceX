using System;

public class CompetitionFile
{
	public bool IsFile { get; set; }
	public string FileName { get; set; }
	public string ShortName { get; set; }
	public DateTime Date { get; set; }
	public string DateStr { get { return Date.ToString("dd:MM:yyyy hh:mm:ss"); } }
}
