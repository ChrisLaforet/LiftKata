
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LiftKata
{

	public class LiftDispatcher
	{
		private readonly List<Car> cars = new List<Car>();

		private readonly Queue<Summon> summonsQueue = new Queue<Summon>();

		internal LiftDispatcher(Lift parentLift)
		{
			ParentLift = parentLift;
		}

		internal Car CreateLiftCar(int totalFloors)
		{
			Car car = new Car(ParentLift, totalFloors);
			cars.Add(car);
			return car;
		}

		internal void ValidateFloorIsReachable(int floor)
		{
			foreach (var car in cars)
			{
				if (!car.IsOffline && car.DoesCarService(floor))
					return;
			}
			throw new InvalidFloorException(floor);
		}

		internal LiftLocationStatus SummonTo(Summon summon)
		{
			ValidateFloorIsReachable(summon.Floor);

			// determine if a car is already there
			foreach (var currentCar in cars)
			{
				if (currentCar.IsAvailable(summon))
					return new LiftLocationStatus(currentCar, summon.Floor, new Direction(CarDirection.STOPPED));
			}

			// does a matching summon already exist - return its status and find the car it has been assigned to


			// if car is not already there, are any on the way or get a parked car?
			Car car = FindClosestAvailableCar(summon);
			if (car != null)
			{
// TODO: add stop to car
				return new LiftLocationStatus(car, summon.Floor, new Direction(car.CarDirectionTo(summon.Floor)));
			}
			return null;
		}

		private Car FindClosestAvailableCar(Summon summon)
		{
			List<Car> stoppedCars = new List<Car>();
			List<Car> possibleCars = new List<Car>();
			List<Car> remainingCars = new List<Car>();
			foreach (var currentCar in cars)
			{
				if (currentCar.IsOffline)
					continue;

				if (currentCar.IsStopped)
					stoppedCars.Add(currentCar);
				else if (currentCar.CanMovingCarServiceSummons(summon))
					possibleCars.Add(currentCar);
				else
					remainingCars.Add(currentCar);
			}

			Car car = possibleCars
							.OrderBy(x => x.FloorsAwayFrom(summon.Floor))
							.FirstOrDefault();
			if (car != null)
				return car;

			car = stoppedCars
						.OrderBy(x => x.FloorsAwayFrom(summon.Floor))
						.FirstOrDefault();
			if (car != null)
				return car;

// TODO: find the remaining car with the closest destination floor and allocate it

			return null;
		}

		public Lift ParentLift
		{
			get;
			private set;
		}

		private LinkedList<LiftLocationStatus> dispatchQueue = new LinkedList<LiftLocationStatus>();

	}
}
