using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace ObedienceX.Data
{
	public class MarksSet : INotifyPropertyChanged
	{
		public ObservableCollection<Mark> Marks { get; set; } = new ObservableCollection<Mark>();

		[JsonIgnore]
		public bool IsSet { get; set; }

		[JsonIgnore]
		public double MidValue { get; set; }

		[JsonIgnore]
		public double MultipliedValue { get { return MidValue * ExamMultiplier; } }

		[JsonIgnore]
		public string MultipliedValueStr { get { return IsSet ? MultipliedValue.ToString() : ""; } }


		[JsonIgnore]
		public Pair Pair { get; set; }
		[JsonIgnore]
		public Examination Examination { get; set; }

		[JsonIgnore]
		public int ExamNumber { get { return Examination.Number; } }

		[JsonIgnore]
		public int ExamAndJudgeNumber0 { get { return Examination.Number + 100 * 0; } }
		[JsonIgnore]
		public int ExamAndJudgeNumber1 { get { return Examination.Number + 100 * 1; } }
		[JsonIgnore]
		public int ExamAndJudgeNumber2 { get { return Examination.Number + 100 * 2; } }
		[JsonIgnore]
		public string ExamName { get { return Examination.Name; } }
		[JsonIgnore]
		public int ExamMultiplier { get { return Examination.Multiplier; } }
		[JsonIgnore]
		public string ValueStr0
		{
			get { return Marks[0].ValueStr; }
			set { Marks[0].ValueStr = value; }
		}
		[JsonIgnore]
		public string ValueStr1
		{
			get { return Marks.Count > 1 ? Marks[1].ValueStr : ""; }
			set { if (Marks.Count > 1) Marks[1].ValueStr = value; }
		}
		[JsonIgnore]
		public string ValueStr2
		{
			get { return Marks.Count > 2 ? Marks[2].ValueStr : ""; }
			set { if (Marks.Count > 2) Marks[2].ValueStr = value; }
		}
		public GridLength Width0 { get { return (GridLength)50; } }
		public GridLength Width1 { get { return (GridLength)(Model.Competition.JudgesCount > 1 ? 50 : 0); } }
		public GridLength Width2 { get { return (GridLength)(Model.Competition.JudgesCount > 2 ? 50 : 0); } }

		public void RecalculateMidValue(bool withPair)
		{
			double sum = 0;
			bool isSet = false;
			for (int i = 0; i < Marks.Count; i++)
			{
				isSet |= Marks[i].IsSet;
				sum += Marks[i].Value;
			}
			IsSet = isSet;
			MidValue = sum / Marks.Count;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MultipliedValue)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MultipliedValueStr)));
			if (withPair)
				Pair.RecalculateSum();
		}

		public void DispatchValueChanged(int index)
		{
			if (index == 0)
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueStr0)));
			if (index == 1)
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueStr1)));
			if (index == 2)
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueStr2)));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
