namespace LiftKata
{
	public class Direction
	{
		private int direction;

		public Direction() => direction = 0;

		public Direction(Direction other)
		{
			direction = other.direction;
		}

		public Direction(bool isMovingUp)
		{
			direction = isMovingUp ? 1 : -1;
		}

		void SetArrived()
		{
			direction = 0;
		}

		public bool IsMovingUp()
		{
			return direction > 0;
		}

		public bool IsMovingDown()
		{
			return direction < 0;
		}

		public bool HasArrived()
		{
			return direction == 0;
		}
	}
}
