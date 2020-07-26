namespace LiftKata
{
	public class LiftLocationStatus
	{
		private readonly Lift lift;
		private Direction direction;

		public LiftLocationStatus(Lift lift, int destinationFloor, Direction direction)
		{
			this.lift = lift;
			this.direction = direction;
			DestinationFloor = destinationFloor;
		}

		public int CurrentFloor
		{
			get => lift.CurrentFloor;
		}

		public int DestinationFloor
		{
			get;
			private set;
		}

		public bool HasArrived()
		{
			return direction.HasArrived();
		}

		public bool IsMovingUp()
		{
			return direction.IsMovingUp();
		}

		public bool IsMovingDown()
		{
			return direction.IsMovingDown();
		}

		internal Direction Direction
		{
			get => direction;
			set => this.direction = value;
		}
	}
}
