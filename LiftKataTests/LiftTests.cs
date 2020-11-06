using LiftKata;
using NUnit.Framework;
using static LiftKata.Car;

namespace LiftKataTests
{
	public class LiftTests
	{
		const int TOTAL_FLOORS = 11;
		const int GROUND_FLOOR = 0;
		const int TOP_FLOOR = TOTAL_FLOORS - 1;

		private static Lift lift;

		private static Lift CreateLift() => new Lift();

		private static Car CreateLiftCar()
		{
			lift = CreateLift();
			return lift.CreateLiftCar(TOTAL_FLOORS);
		}

		[Test]
		public void GivenALift_WhenAssignedANumberOfFloors_ReturnsNumberOfFloorsAsTotalFloors()
		{
			Assert.AreEqual(TOTAL_FLOORS, CreateLiftCar().TotalFloors);
		}

		[Test]
		public void GivenANewLift_WhenQueriedForCurrentFloor_ReturnsFloorZero()
		{
			Assert.AreEqual(GROUND_FLOOR, CreateLiftCar().CurrentFloor);
		}

		[Test]
		public void GivenANewLift_WhenQueriedForCarState_ReturnsParked()
		{
			Assert.AreEqual(CarStatus.PARKED, CreateLiftCar().CarState);
		}

		[Test]
		public void GivenANewLift_WhenTakenOfflineAndQueriedForCarState_ReturnsOffline()
		{
			Car car = CreateLiftCar();
			lift.TakeCarOffline(car);
			Assert.AreEqual(CarStatus.OFFLINE, car.CarState);
		}

		[Test]
		public void GivenANewLift_WhenQueriedForDoorState_ReturnsDoorsOpen()
		{
			Assert.AreEqual(DoorStatus.OPEN, CreateLiftCar().DoorState);
		}

		[Test]
		public void GivenANewLift_WhenTakenOfflineAndQueriedForDoorState_ReturnsDoorsClosed()
		{
			Car car = CreateLiftCar();
			lift.TakeCarOffline(car);
			Assert.AreEqual(DoorStatus.CLOSED, car.DoorState);
		}

		[Test]
		public void GivenANewLift_WhenQueriedForTopFloor_ReturnsTopFloor()
		{
			Assert.AreEqual(TOP_FLOOR, CreateLiftCar().TopFloor);
		}

		[Test]
		public void GivenANewLift_WhenQueriedIfOnFloorZero_ReturnsTrue()
		{
			Car car = CreateLiftCar();
			Assert.IsTrue(car.IsAlreadyOn(GROUND_FLOOR));
		}

		[Test]
		public void GivenANewLift_WhenQueriedIfOnTopFloor_ReturnsFalse()
		{
			Car car = CreateLiftCar();
			Assert.IsFalse(car.IsAlreadyOn(TOP_FLOOR));
		}

		[Test]
		public void GivenALift_WhenSummonedToFloorBelowZero_ThrowsInvalidFloorException()
		{
			Car car = CreateLiftCar();
			Assert.Throws(Is.TypeOf<InvalidFloorException>(), 
				delegate { car.ParentLift.SummonTo(Summon.GoingUp(GROUND_FLOOR - 1)); });
		}

		[Test]
		public void GivenALift_WhenSummonedToFloorAboveTopFloor_ThrowsInvalidFloorException()
		{
			Car car = CreateLiftCar();
			Assert.Throws(Is.TypeOf<InvalidFloorException>(),
				delegate { car.ParentLift.SummonTo(Summon.GoingDown(TOP_FLOOR + 1)); });
		}

		[Test]
		public void GivenALift_WhenSummonedForGroundFloor_ReturnsLiftLocationAlreadyThere()
		{
			Car car = CreateLiftCar();
			LocationStatus location = car.ParentLift.SummonTo(Summon.GoingUp(GROUND_FLOOR));
			Assert.IsTrue(location.IsStopped);
			Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);
			Assert.AreEqual(GROUND_FLOOR, location.DestinationFloor);
		}

		[Test]
		public void GivenALift_WhenSummonedForTopFloor_ReturnsLiftLocationMovingUp()
		{
			Car car = CreateLiftCar();
			LocationStatus location = car.ParentLift.SummonTo(Summon.GoingDown(TOP_FLOOR));
			Assert.IsTrue(location.IsMovingUp);
			Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);
			Assert.AreEqual(TOP_FLOOR, location.DestinationFloor);
		}

		[Test]
		public void GivenALift_WhenSummonedForFirstFloor_ReturnsLiftClosesDoorsAfterOneTick()
		{
			Car car = CreateLiftCar();
			Assert.AreEqual(GROUND_FLOOR, car.CurrentFloor);
			Assert.AreEqual(DoorStatus.OPEN, car.DoorState);
			Assert.AreEqual(CarPendingStatus.NONE, car.CarPendingState);

			LocationStatus location = car.ParentLift.SummonTo(Summon.GoingDown(1));
			lift.SendTick();

			Assert.AreEqual(CarPendingStatus.MOVE_UP, car.CarPendingState);
			Assert.IsTrue(location.IsMovingUp);
			Assert.AreEqual(DoorStatus.CLOSING, car.DoorState);

			lift.SendTick();

			Assert.AreEqual(DoorStatus.CLOSED, car.DoorState);
			Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);
		}

		//[Test]
		//public void GivenALift_WhenSummonedForFirstFloor_ReturnsLiftArrivesOnFirstFloorAfterTwoTicks()
		//{
		//	Car car = CreateLiftCar();
		//	LocationStatus location = car.ParentLift.SummonTo(Summon.GoingDown(1));
		//	Assert.IsTrue(location.IsMovingUp);
		//	Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);

		//	lift.SendTick();
		//	Assert.IsTrue(location.HasArrived());
		//	Assert.AreEqual(1, location.CurrentFloor);
		//}

		//[Test]
		//public void GivenALift_WhenSummonedForFirstFloor_ReturnsLiftOpensDoorsOnFirstFloorAfterThreeTicks()
		//{
		//	Car car = CreateLiftCar();
		//	LocationStatus location = car.ParentLift.SummonTo(Summon.GoingDown(1));
		//	Assert.IsTrue(location.IsMovingUp);
		//	Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);

		//	lift.SendTick();
		//	Assert.IsTrue(location.HasArrived());
		//	Assert.AreEqual(1, location.CurrentFloor);
		//}
		//		[Test]
		//		public void GivenALift_WhenSummonedForTopFloor_ReturnsLiftArrivesOnFloorSeveralTick()
		//		{
		//			Lift lift = CreateLift();
		//			LiftLocationStatus location = lift.SummonTo(TOP_FLOOR);
		//			Assert.IsTrue(location.IsMovingUp());
		//			Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);

		//			lift.ClickTick();
		//			Assert.IsFalse(location.HasArrived());
		//			Assert.AreEqual(1, location.CurrentFloor);

		//			for (int click = location.CurrentFloor; click < TOTAL_FLOORS; click++)
		//			{
		//				lift.ClickTick();
		//			}
		//			Assert.IsTrue(location.HasArrived());
		//			Assert.AreEqual(TOP_FLOOR, location.CurrentFloor);
		//		}

		//		[Test]
		//		public void GivenALift_WhenSummonedForSecondFloorThenFirstFloor_ReturnsLiftArrivesAtSecondFloor()
		//		{
		//			Lift lift = CreateLift();
		//			LiftLocationStatus location2 = lift.SummonTo(2);
		//			Assert.IsTrue(location2.IsMovingUp());
		//			Assert.AreEqual(GROUND_FLOOR, location2.CurrentFloor);

		//			lift.ClickTick();
		//			Assert.IsFalse(location2.HasArrived());
		//			Assert.AreEqual(1, location2.CurrentFloor);
		//			LiftLocationStatus location1 = lift.SummonTo(1);

		//			lift.ClickTick();
		//			Assert.IsTrue(location2.HasArrived());
		//			Assert.AreEqual(2, location2.CurrentFloor);

		////			Assert.IsFalse(location1.HasArrived());
		//		}
	}
}