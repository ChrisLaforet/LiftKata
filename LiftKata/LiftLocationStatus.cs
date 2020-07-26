namespace LiftKata
{
	public class LiftLocationStatus
	{
		private readonly Car car;

		public LiftLocationStatus(Car car, int destinationFloor, Direction direction)
		{
			this.car = car;
			this.Direction = direction;
			DestinationFloor = destinationFloor;
		}

		public int CurrentFloor
		{
			get => car.CurrentFloor;
		}

		public int DestinationFloor
		{
			get;
			private set;
		}

		public bool HasArrived()
		{
			return Direction.HasArrived();
		}

		public bool IsMovingUp()
		{
			return Direction.IsMovingUp();
		}

		public bool IsMovingDown()
		{
			return Direction.IsMovingDown();
		}

		internal Direction Direction
		{
			get;
			set;
		}
	}
}
