namespace LiftKata
{ 
	public enum CarDirection 
	{
		MOVING_UP,
		PENDING_MOVE_UP,
		STOPPED,
		PENDING_MOVE_DOWN,
		MOVING_DOWN 
	}

	public enum DesiredDirection
	{
		UP,
		DOWN
	}

	public class Direction
	{
		private readonly CarDirection direction;
		
		public Direction() => direction = CarDirection.STOPPED;

		public Direction(CarDirection carDirection) => direction = carDirection;

		public Direction(Direction other)
		{
			direction = other.direction;
		}

		public bool IsStopped
		{
			get => direction == CarDirection.STOPPED;
		}

		public bool IsPaused
		{
			get => direction == CarDirection.PENDING_MOVE_DOWN ||
				direction == CarDirection.PENDING_MOVE_UP;
		}

		public bool IsMovingUp
		{
			get => direction == CarDirection.MOVING_UP ||
				direction == CarDirection.PENDING_MOVE_UP;
		}

		public bool IsMovingDown
		{
			get => direction == CarDirection.MOVING_DOWN ||
				direction == CarDirection.PENDING_MOVE_DOWN;
		}
	}
}
