
using System;

namespace LiftKata
{
	public class InvalidFloorException : Exception
	{
		public InvalidFloorException(int floor) : base($"Invalid floor {floor} provided") {}
	}
}
