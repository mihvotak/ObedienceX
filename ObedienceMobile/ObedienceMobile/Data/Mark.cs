using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ObedienceX.Data
{
	public class Mark : INotifyPropertyChanged
	{
		
		public double Value { get; set; }

		public bool IsSet { get; set; }

		[JsonIgnore]
		public bool IsValid { get { return Value == 0 || (Value % .5 == 0 && Value >= 5 && Value <= 10); } }

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
				return IsSet ? Value.ToString() : "";
			}
			set
			{
				float newVal;
				bool isSet = float.TryParse(value, out newVal);
				if (isSet && (Value != newVal || !IsSet))
				{
					Value = newVal;
					IsSet = isSet;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MultipliedValueStr)));
					Pair.RecalculateSum();
					Pair.Competition.Saved = false;
				}
			}
		}

		[JsonIgnore]
		public double MultipliedValue { get { return Value * ExamMultiplier; } }

		[JsonIgnore]
		public string MultipliedValueStr { get { return IsSet ? MultipliedValue.ToString() : ""; } }

		public event PropertyChangedEventHandler PropertyChanged;

	}
}
