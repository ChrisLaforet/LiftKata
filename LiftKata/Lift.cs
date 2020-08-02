using System.Collections.Generic;

namespace LiftKata
{
	public class Lift
	{
		private List<LiftLocationStatus> dispatchQueue = new List<LiftLocationStatus>();
		private Direction currentDirection = new Direction();

		private List<Car> cars = new List<Car>();

		public Lift() { }

		public Car CreateLiftCar(int totalFloors)
		{
			Car car = new Car(this, totalFloors);
			cars.Add(car);
			return car;
		}

		public void TakeCarOffline(Car car)
		{
			car.TakeOffline();
		}

		private void validateFloorIsReachable(int floor)
		{
			foreach (var car in cars)
			{
				if (!car.IsOffline && car.DoesCarService(floor))
					return;
			}
			throw new InvalidFloorException(floor);
		}

		public LiftLocationStatus SummonTo(Summon summon)
		{
			validateFloorIsReachable(summon.Floor);

			// determine if a car is already there
			foreach (var car in cars)
			{
				if (car.IsAvailable(summon))
					return new LiftLocationStatus(car, summon.Floor, new Direction(CarDirection.STOPPED));
			}

			// if car is not already there, is any on the way?

			// does a match summon already exist 

			// if not, summon a parked car


			//LiftLocationStatus liftLocation = new LiftLocationStatus(this, floor, DetermineDirectionTowards(floor));
			//if (currentDirection.HasArrived())
			//{
			//	currentDirection = new Direction(liftLocation.Direction);
			//}
			////else
			////{
			////	currentDirection = new Direction(liftLocation.IsMovingUp());
			////}

			//dispatchQueue.Add(liftLocation);
			//return liftLocation;
return null;
		}

		//public void ClickTimeUnit()
		//{
		//	List<LiftLocationStatus> toRemove = new List<LiftLocationStatus>();
		//	foreach (LiftLocationStatus liftLocation in dispatchQueue)
		//	{
		//		if (liftLocation.IsMovingUp() && currentDirection.IsMovingUp())
		//		{
		//			if (CurrentFloor < TopFloor)
		//				CurrentFloor = CurrentFloor + 1;
		//		}
		//		else if (liftLocation.IsMovingDown() && currentDirection.IsMovingDown())
		//		{
		//			if (CurrentFloor > 0)
		//				CurrentFloor = CurrentFloor - 1;
		//		}

		//		if (CurrentFloor == liftLocation.DestinationFloor)
		//		{
		//			liftLocation.Direction = new Direction();
		//			toRemove.Add(liftLocation);
		//		}
		//	}

		//	if (toRemove.Count > 0)
		//	{
		//		dispatchQueue = dispatchQueue.Where(x => !toRemove.Contains(x)).ToList();
		//	}
		//}

		//private Direction DetermineDirectionTowards(int floor)
		//{
		//	if (floor == CurrentFloor)
		//	{
		//		return new Direction();
		//	}
		//	return new Direction(floor > CurrentFloor);
		//}
	}
}
