namespace LiftKata
{
	public class Request : ICarStatus
	{
		public Request(int floor) => DestinationFloor = floor;

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
