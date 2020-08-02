using System;
using System.Collections.Generic;
using System.Text;

namespace LiftKata
{
	public class CarSchedule
	{
		private Queue<ICarStatus> stops = new Queue<ICarStatus>();

		public CarSchedule(DesiredDirection direction) => Direction = direction;

		public DesiredDirection Direction
		{
			get;
			private set;
		}
	}
}
