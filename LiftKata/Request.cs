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

		public bool IsRequest
		{
			get => true;
		}

		public void NotifyArrival()
		{
		
		}
	}
}
