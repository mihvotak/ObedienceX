using System;

namespace ObedienceX.Data
{
	internal class Model
	{
		private static Competition _competition;
		public static Competition Competition
		{
			get 
			{
				if (_competition == null)
					_competition = new Competition();
				return _competition;
			}
			set 
			{
				_competition = value;
			}
		}
	}
}
