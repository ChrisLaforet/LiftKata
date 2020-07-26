using LiftKata;
using NUnit.Framework;

namespace LiftKataTests
{
	public class LiftTests
	{
		const int TOTAL_FLOORS = 11;
		const int GROUND_FLOOR = 0;
		const int TOP_FLOOR = TOTAL_FLOORS - 1;

		private static Lift CreateLift() => new Lift(TOTAL_FLOORS);

		[Test]
		public void GivenALift_WhenAssignedANumberOfFloors_ReturnsNumberOfFloorsAsTotalFloors()
		{
			Assert.AreEqual(TOTAL_FLOORS, CreateLift().TotalFloors);
		}

		[Test]
		public void GivenANewLift_WhenQueriedForCurrentFloor_ReturnsFloorZero()
		{
			Assert.AreEqual(GROUND_FLOOR, CreateLift().CurrentFloor);
		}

		[Test]
		public void GivenANewLift_WhenQueriedForTopFloor_ReturnsTopFloor()
		{
			Assert.AreEqual(TOP_FLOOR, CreateLift().TopFloor);
		}

		[Test]
		public void GivenANewLift_WhenQueriedIfOnFloorZero_ReturnsTrue()
		{
			Lift lift = CreateLift();
			Assert.IsTrue(lift.IsAlreadyOn(GROUND_FLOOR));
		}

		[Test]
		public void GivenANewLift_WhenQueriedIfOnTopFloor_ReturnsFalse()
		{
			Lift lift = CreateLift();
			Assert.IsFalse(lift.IsAlreadyOn(TOP_FLOOR));
		}

		[Test]
		public void GivenALift_WhenQueriedForFloorBelowZero_ThrowsInvalidFloorException()
		{
			Lift lift = CreateLift();
			Assert.Throws(Is.TypeOf<InvalidFloorException>(), 
				delegate { lift.IsAlreadyOn(GROUND_FLOOR - 1); });
		}

		[Test]
		public void GivenALift_WhenQueriedForFloorAboveTopFloor_ThrowsInvalidFloorException()
		{
			Lift lift = CreateLift();
			Assert.Throws(Is.TypeOf<InvalidFloorException>(),
				delegate { lift.IsAlreadyOn(TOP_FLOOR + 1); });
		}

		[Test]
		public void GivenALift_WhenSummonedForGroundFloor_ReturnsLiftLocationAlreadyThere()
		{
			Lift lift = CreateLift();
			LiftLocationStatus location = lift.SummonTo(GROUND_FLOOR);
			Assert.IsTrue(location.HasArrived());
			Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);
			Assert.AreEqual(GROUND_FLOOR, location.DestinationFloor);
		}

		[Test]
		public void GivenALift_WhenSummonedForTopFloor_ReturnsLiftLocationMovingUp()
		{
			Lift lift = CreateLift();
			LiftLocationStatus location = lift.SummonTo(TOP_FLOOR);
			Assert.IsTrue(location.IsMovingUp());
			Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);
			Assert.AreEqual(TOP_FLOOR, location.DestinationFloor);
		}

		[Test]
		public void GivenALift_WhenSummonedForFirstFloor_ReturnsLiftArrivesOnFloorAfterOneTimeUnit()
		{
			Lift lift = CreateLift();
			LiftLocationStatus location = lift.SummonTo(1);
			Assert.IsTrue(location.IsMovingUp());
			Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);

			lift.ClickTimeUnit();
			Assert.IsTrue(location.HasArrived());
			Assert.AreEqual(1, location.CurrentFloor);
		}

		[Test]
		public void GivenALift_WhenSummonedForTopFloor_ReturnsLiftArrivesOnFloorSeveralTimeUnit()
		{
			Lift lift = CreateLift();
			LiftLocationStatus location = lift.SummonTo(TOP_FLOOR);
			Assert.IsTrue(location.IsMovingUp());
			Assert.AreEqual(GROUND_FLOOR, location.CurrentFloor);

			lift.ClickTimeUnit();
			Assert.IsFalse(location.HasArrived());
			Assert.AreEqual(1, location.CurrentFloor);

			for (int click = location.CurrentFloor; click < TOTAL_FLOORS; click++)
			{
				lift.ClickTimeUnit();
			}
			Assert.IsTrue(location.HasArrived());
			Assert.AreEqual(TOP_FLOOR, location.CurrentFloor);
		}

		[Test]
		public void GivenALift_WhenSummonedForSecondFloorThenFirstFloor_ReturnsLiftArrivesAtSecondFloor()
		{
			Lift lift = CreateLift();
			LiftLocationStatus location2 = lift.SummonTo(2);
			Assert.IsTrue(location2.IsMovingUp());
			Assert.AreEqual(GROUND_FLOOR, location2.CurrentFloor);

			lift.ClickTimeUnit();
			Assert.IsFalse(location2.HasArrived());
			Assert.AreEqual(1, location2.CurrentFloor);
			LiftLocationStatus location1 = lift.SummonTo(1);

			lift.ClickTimeUnit();
			Assert.IsTrue(location2.HasArrived());
			Assert.AreEqual(2, location2.CurrentFloor);

//			Assert.IsFalse(location1.HasArrived());
		}
	}
}