namespace LiftKata
{
	public class Summon
	{
		private Summon(int floor, DesiredDirection direction)
		{
			Floor = floor;
			Direction = direction;
		}

		public int Floor
		{
			get;
			set;
		}

		public DesiredDirection Direction
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
