
namespace LiftKata
{
	public class Car
	{
		public enum CAR_STATE { PARKED, STOPPED, MOVING_UP, MOVING_DOWN, OFFLINE };

		public enum DOOR_STATE { OPEN, OPENING, CLOSED, CLOSING };

		private readonly Lift parentLift;

		private Direction currentDirection = new Direction();

		internal Car(Lift parentLift, int totalFloors)
		{
			this.parentLift = parentLift;
			TotalFloors = totalFloors;
			CarState = CAR_STATE.PARKED;
			DoorState = DOOR_STATE.OPEN;
		}

		internal void TakeOffline()
		{
			DoorState = DOOR_STATE.CLOSED;
			CarState = CAR_STATE.OFFLINE;
		}

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

		public CAR_STATE CarState
		{
			get;
			private set;
		}

		public DOOR_STATE DoorState
		{
			get;
			private set;
		}
	}
}
