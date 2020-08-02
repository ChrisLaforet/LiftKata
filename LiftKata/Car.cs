using System.Collections.Generic;

namespace LiftKata
{
	public class Car
	{
		public enum CAR_STATE { PARKED, STOPPED, MOVING_UP, MOVING_DOWN, OFFLINE };

		public enum CAR_PENDING_STATE { NONE, MOVE_UP, MOVE_DOWN };

		public enum DOOR_STATE { OPEN, OPENING, CLOSED, CLOSING };

		private readonly Lift parentLift;

		private CarDirection currentDirection = CarDirection.STOPPED;

		private readonly Queue<Request> requestQueue = new Queue<Request>();

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

		public Lift ParentLift
		{
			get => parentLift;
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

		public int BottomFloor
		{
			// Note: may modify later to support cars that have different lowest floors (e.g. Basement)
			get => 0;
		}

		public int TopFloor
		{
			get => TotalFloors - 1;
		}

		public bool IsAlreadyOn(int floor)
		{
			return CurrentFloor == floor && IsStopped;
		}

		public bool IsStopped
		{
			get => CarState == CAR_STATE.PARKED ||
				(CarState == CAR_STATE.STOPPED && CarPendingState == CAR_PENDING_STATE.NONE);
		}

		public bool IsVisiting(DesiredDirection direction)
		{
			CAR_PENDING_STATE matchingState = direction == DesiredDirection.UP ?
				CAR_PENDING_STATE.MOVE_UP :
				CAR_PENDING_STATE.MOVE_DOWN;

			return CarState == CAR_STATE.STOPPED && CarPendingState == matchingState;
		}

		public bool IsAvailable(Summon summon)
		{
			if (summon.Floor != CurrentFloor) {
				return false;
			}
			return IsStopped || IsVisiting(summon.Direction);
		}

		public bool DoesCarService(int floor)
		{
			return floor >= BottomFloor && floor <= TopFloor;
		}

		public bool IsOffline
		{
			get => CarState == CAR_STATE.OFFLINE;
		}

		public CAR_STATE CarState
		{
			get;
			private set;
		}

		public CAR_PENDING_STATE CarPendingState
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
