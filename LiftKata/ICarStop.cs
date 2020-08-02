using System;
using System.Collections.Generic;
using System.Text;

namespace LiftKata
{
	public interface ICarStop
	{
		public int DestinationFloor
		{
			get;
		}

		void NotifyArrival();
	}
}
