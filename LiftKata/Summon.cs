namespace LiftKata
{
	public class Summon
	{
		private Summon(int floor, DesiredDirection desiredDirection)
		{
			Floor = floor;
			DesiredDirection = desiredDirection;
		}

		public int Floor
		{
			get;
			set;
		}

		public DesiredDirection DesiredDirection
		{
			get;
			set;
		}

		static public Summon GoingUp(int floor)
		{
			return new Summon(floor, DesiredDirection.UP);
		}

		static public Summon GoingDown(int floor)
		{
			return new Summon(floor, DesiredDirection.DOWN);
		}
	}
}
