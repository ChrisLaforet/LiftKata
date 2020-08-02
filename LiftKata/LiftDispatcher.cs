
using System.Collections.Generic;

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
			foreach (var car in cars)
			{
				if (car.IsAvailable(summon))
					return new LiftLocationStatus(car, summon.Floor, new Direction(CarDirection.STOPPED));
			}

			// does a matching summon already exist - return its status


			// if car is not already there, are any on the way?


			// if not, summon a parked car
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
