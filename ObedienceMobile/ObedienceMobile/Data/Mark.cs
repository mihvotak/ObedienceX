using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace ObedienceX.Data
{
	public class Mark : INotifyPropertyChanged
	{
		
		public double Value { get; set; }

		public bool IsSet { get; set; }

		[JsonIgnore]
		public MarksSet MarksSet { get; set; }

		[JsonIgnore]
		public bool IsValid { get { return Value == 0 || (Value % .5 == 0 && Value >= 5 && Value <= 10); } }


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
				if (value == "")
				{
					Value = 0;
					IsSet = false;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueStr)));
					MarksSet.RecalculateMidValue(true);
					MarksSet.Pair.Competition.Saved = false;
				}
				else if (isSet && (Value != newVal || !IsSet))
				{
					Value = newVal;
					IsSet = isSet;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueStr)));
					MarksSet.RecalculateMidValue(true);
					MarksSet.Pair.Competition.Saved = false;
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

	}
}
