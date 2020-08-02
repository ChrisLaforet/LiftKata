namespace LiftKata
{
	public class Request
	{
		public Request(int floor) => DestinationFloor = floor;

		public int DestinationFloor
		{
			get;
			private set;
		}
	}
}
