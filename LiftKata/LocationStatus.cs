﻿namespace LiftKata
{
	public class LocationStatus : ICarStatus
	{
		private readonly Car car;

		public LocationStatus(Car car, int destinationFloor, Direction direction)
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

		public bool IsRequest
		{
			get => false;
		}

		public void NotifyArrival()
		{

		}

		public bool IsStopped
		{
			get => Direction.IsPaused || Direction.IsStopped;
		}

		public bool IsMovingUp
		{
			get => Direction.IsMovingUp;
		}

		public bool IsMovingDown
		{
			get => Direction.IsMovingDown;
		}

		internal Direction Direction
		{
			get;
			set;
		}
	}
}
