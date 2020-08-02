﻿using System;
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

		private readonly Queue<Request> requestQueue = new Queue<Request>();

		internal Car(Lift parentLift, int totalFloors)
		{
			this.parentLift = parentLift;
			TotalFloors = totalFloors;
			CarState = CarStatus.PARKED;
			DoorState = DoorStatus.OPEN;
		}

		internal void TakeOffline()
		{
			DoorState = DoorStatus.CLOSED;
			CarState = CarStatus.OFFLINE;
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
