using System;
using System.Collections.Generic;
using System.Text;

namespace LiftKata
{
	public class LiftSchedule
	{
		private Queue<> stops = new Queue<>();

		public LiftSchedule(DesiredDirection direction) => Direction = direction;

		public DesiredDirection Direction
		{
			get;
			private set;
		}

	}
}
