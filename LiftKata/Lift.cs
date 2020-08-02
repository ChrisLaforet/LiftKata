using System.Collections.Generic;

namespace LiftKata
{
	public class Lift
	{
		private readonly LiftDispatcher dispatcher;

		public Lift() => dispatcher = new LiftDispatcher(this);

		public Car CreateLiftCar(int totalFloors)
		{
			return dispatcher.CreateLiftCar(totalFloors);
		}

		public void TakeCarOffline(Car car)
		{
			car.TakeOffline();
		}


		public LiftLocationStatus SummonTo(Summon summon)
		{
			return dispatcher.SummonTo(summon);
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
