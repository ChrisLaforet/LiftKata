using System.Collections.Generic;
using System.Linq;

namespace LiftKata
{
	public class Lift
	{
		private List<LiftLocationStatus> dispatchQueue = new List<LiftLocationStatus>();
		private Direction currentDirection = new Direction();

		public Lift(int totalFloors) => TotalFloors = totalFloors;

		public int TotalFloors
		{
			get;
			private set;
		}

		public int CurrentFloor
		{
			get;
			private set;
		}

		public int TopFloor
		{
			get => TotalFloors - 1;
		}

		public bool IsAlreadyOn(int floor)
		{
			if (floor < 0 || floor >= TotalFloors)
			{
				throw new InvalidFloorException(floor);
			}
			return floor == CurrentFloor;
		}

		public LiftLocationStatus SummonTo(int floor)
		{
			LiftLocationStatus liftLocation = new LiftLocationStatus(this, floor, DetermineDirectionTowards(floor));
			if (currentDirection.HasArrived())
			{
				currentDirection = new Direction(liftLocation.Direction);
			} 
			//else
			//{
			//	currentDirection = new Direction(liftLocation.IsMovingUp());
			//}

			dispatchQueue.Add(liftLocation);
			return liftLocation;
		}

		public void ClickTimeUnit()
		{
			List<LiftLocationStatus> toRemove = new List<LiftLocationStatus>();
			foreach (LiftLocationStatus liftLocation in dispatchQueue)
			{
				if (liftLocation.IsMovingUp() && currentDirection.IsMovingUp())
				{
					if (CurrentFloor < TopFloor)
						CurrentFloor = CurrentFloor + 1;
				}
				else if (liftLocation.IsMovingDown() && currentDirection.IsMovingDown())
				{
					if (CurrentFloor > 0)
						CurrentFloor = CurrentFloor - 1;
				}
				
				if (CurrentFloor == liftLocation.DestinationFloor)
				{
					liftLocation.Direction = new Direction();
					toRemove.Add(liftLocation);
				}
			}

			if (toRemove.Count > 0)
			{
				dispatchQueue = dispatchQueue.Where(x => !toRemove.Contains(x)).ToList();
			}
		}

		private Direction DetermineDirectionTowards(int floor)
		{
			if (floor == CurrentFloor)
			{
				return new Direction();
			}
			return new Direction(floor > CurrentFloor);
		}
	}

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
