using System;
using System.Collections.Generic;

namespace LiftKata
{
	public class Car
	{
		public enum CarStatus { PARKED, STOPPED, MOVING_UP, MOVING_DOWN, OFFLINE };

		public enum CarPendingStatus { NONE, MOVE_UP, MOVE_DOWN };

		public enum DoorStatus { OPEN, OPENING, CLOSED, CLOSING };

		public const int RESPONSE_TIME_IN_FLOORS = 2;		// Eventually needs to be configured by the lift - this varies based on the speed of car types

		private readonly Lift parentLift;

		private CarDirection currentDirection = CarDirection.STOPPED;

		private readonly Queue<CarSchedule> schedules = new Queue<CarSchedule>();

		private CarSchedule activeSchedule;

		internal Car(Lift parentLift, int totalFloors)
		{
			this.parentLift = parentLift;
			TotalFloors = totalFloors;
			CarState = CarStatus.PARKED;
			DoorState = DoorStatus.OPEN;
			parentLift.TickEvent += this.TickHandler;
		}

		internal void TakeOffline()
		{
			DoorState = DoorStatus.CLOSED;
			CarState = CarStatus.OFFLINE;
		}

		private void TickHandler()
		{
			if ((schedules.Count == 0 && activeSchedule == null) ||
				CarState == CarStatus.OFFLINE)
				return;

			if (CarState == CarStatus.STOPPED ||
				CarState == CarStatus.PARKED)
			{
				HandleScheduleFromStop();
			}
			else if (CarState == CarStatus.MOVING_UP)
			{
				HandleScheduleWhileMovingUp();
			}
			else if (CarState == CarStatus.MOVING_DOWN)
			{
				HandleScheduleWhileMovingDown();
			}
		}

		private void HandleScheduleFromStop()
		{
			activeSchedule = schedules.Dequeue();
			ICarStatus stop = activeSchedule.NextStop;

			DoorState = DoorStatus.CLOSING;

			
			CarDirection direction = CarDirectionTo(stop.DestinationFloor);
			if (direction == CarDirection.MOVING_DOWN)
			{
				CarPendingState = CarPendingStatus.MOVE_DOWN;
				CarState = CarStatus.MOVING_DOWN;
			} 
			else if (direction == CarDirection.MOVING_UP)
			{
				CarPendingState = CarPendingStatus.MOVE_UP;
				CarState = CarStatus.MOVING_UP;
			}
		}

		private void HandleScheduleWhileMovingUp()
		{
			if (DoorState == DoorStatus.CLOSING)
			{
				DoorState = DoorStatus.CLOSED;
			}
			else if (DoorState == DoorStatus.OPENING)
			{
				DoorState = DoorStatus.OPEN;
			}

			// STEP
		}

		private void HandleScheduleWhileMovingDown()
		{
			if (DoorState == DoorStatus.CLOSING)
			{
				DoorState = DoorStatus.CLOSED;
			}
			else if (DoorState == DoorStatus.OPENING)
			{
				DoorState = DoorStatus.OPEN;
			}

			// STEP

		}


		public void AddStop(ICarStatus stop, DesiredDirection direction)
		{
			// if the car is stopped and nothing in queue, add to queue and start process
			if (this.IsStopped)
			{
				CarSchedule carSchedule = new CarSchedule(direction);
				carSchedule.EnqueueStop(stop);
				schedules.Enqueue(carSchedule);
				return;
			}

			// if the car is scheduled and moving towards the stop in the same direction and is within RESPONSE TIME, insert into queue

			// if the car is scheduled and not moving in the same direction, add to the next queue spot when moving in the same direction

		}

		public void AddStop(ICarStatus stop)
		{

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
			get => CarState == CarStatus.PARKED ||
				(CarState == CarStatus.STOPPED && CarPendingState == CarPendingStatus.NONE);
		}

		public bool IsVisiting(DesiredDirection direction)
		{
			CarPendingStatus matchingState = direction == DesiredDirection.UP ?
				CarPendingStatus.MOVE_UP :
				CarPendingStatus.MOVE_DOWN;

			return CarState == CarStatus.STOPPED && CarPendingState == matchingState;
		}

		public bool IsAvailable(Summon summon)
		{
			if (summon.Floor != CurrentFloor) {
				return false;
			}
			return IsStopped || IsVisiting(summon.DesiredDirection);
		}

		public bool CanMovingCarServiceSummons(Summon summon)
		{
			if (!DoesCarService(summon.Floor) || IsStopped)
				return false;

			bool isMovingTheSameDirection = summon.DesiredDirection == DesiredDirection.DOWN ?
				CarState == CarStatus.MOVING_DOWN || CarPendingState == CarPendingStatus.MOVE_DOWN :
				CarState == CarStatus.MOVING_UP || CarPendingState == CarPendingStatus.MOVE_UP;

			int decisionFloor = summon.DesiredDirection == DesiredDirection.DOWN ?
				summon.Floor + RESPONSE_TIME_IN_FLOORS :
				summon.Floor - RESPONSE_TIME_IN_FLOORS;

			if (!isMovingTheSameDirection)
				return false;
			//else if (CurrentFloor == summon.Floor && IsVisiting(summon.DesiredDirection))
			//	return true;
			else if (summon.DesiredDirection == DesiredDirection.DOWN &&
				CurrentFloor >= decisionFloor)
				return true;
			else if (summon.DesiredDirection == DesiredDirection.UP &&
				CurrentFloor <= decisionFloor)
				return true;
			return true;
		}

		public int FloorsAwayFrom(int floor)
		{
			return Math.Abs(floor - CurrentFloor);
		}

		public CarDirection CarDirectionTo(int floor)
		{
			if (floor < CurrentFloor)
				return CarDirection.MOVING_DOWN;
			else if (floor > CurrentFloor)
				return CarDirection.MOVING_UP;
			return CarDirection.STOPPED;
		}

		public bool DoesCarService(int floor)
		{
			return floor >= BottomFloor && floor <= TopFloor;
		}

		public bool ContainsStop(Summon summon)
		{
			if (schedules.Count == 0)
				return false;

			foreach (var schedule in schedules)
			{
				if (schedule.ContainsStop(summon))
					return true;
			}
			return false;
		}

		public bool ContainsStop(ICarStatus stop)
		{
			if (schedules.Count == 0)
				return false;

			foreach (var schedule in schedules)
			{
				if (schedule.ContainsStop(stop))
					return true;
			}
			return false;
		}

		public bool IsOffline
		{
			get => CarState == CarStatus.OFFLINE;
		}

		public CarStatus CarState
		{
			get;
			private set;
		}

		public CarPendingStatus CarPendingState
		{
			get;
			private set;
		}

		public DoorStatus DoorState
		{
			get;
			private set;
		}
	}
}
