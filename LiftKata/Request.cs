namespace LiftKata
{
	public class Request : ICarStop
	{
		public Request(int floor) => DestinastionFloor = floor;

		public int DestinationFloor
		{
			get;
			private set;
		}

		public void NotifyArrival()
		{
		
		}
	}
}
