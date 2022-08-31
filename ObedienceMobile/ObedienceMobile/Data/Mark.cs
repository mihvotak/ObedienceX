using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ObedienceX.Data
{
	public class Mark : INotifyPropertyChanged
	{
		public float Value { get; set; }

		[JsonIgnore]
		public Pair Pair { get; set; }
		[JsonIgnore]
		public Examination Examination { get; set; }

		[JsonIgnore]
		public int ExamNumber { get { return Examination.Number; } }
		[JsonIgnore]
		public string ExamName { get { return Examination.Name; } }
		[JsonIgnore]
		public int ExamMultiplier { get { return Examination.Multiplier; } }

		static private List<string> AllValues
		{
			get
			{
				return new List<string> { "0", "5", "5.5", "6", "6.5", "7", "7.5", "8", "8.5", "9", "9.5", "10" };
			}
		}
		[JsonIgnore]
		public List<string> Values { get { return AllValues; } }

		[JsonIgnore]
		public string ValueStr
		{
			get
			{
				return Value == 0 ? "" : Value.ToString();
			}
			set
			{
				float newVal;
				if (float.TryParse(value, out newVal))
				{
					Value = newVal;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MultipliedValueStr)));
					Pair.RecalculateSum();
				}
			}
		}

		[JsonIgnore]
		public float MultipliedValue { get { return Value * ExamMultiplier; } }

		[JsonIgnore]
		public string MultipliedValueStr { get { return Value == 0 ? "" : MultipliedValue.ToString(); } }

		public event PropertyChangedEventHandler PropertyChanged;

	}
}
