using System.Collections.Generic;

namespace LiftKata
{
	public class CarSchedule
	{
		private LinkedList<ICarStatus> stops = new LinkedList<ICarStatus>();

		public CarSchedule(DesiredDirection direction) => Direction = direction;

		public DesiredDirection Direction
		{
			get;
			private set;
		}

		public ICarStatus NextStop
		{
			get
			{
				return stops.Count > 0 ? stops.First.Value : null;
			}
		}

		public bool ContainsStop(Summon summon)
		{
			if (Direction != summon.DesiredDirection)
				return false;
			foreach (ICarStatus current in stops)
			{
				if (current.DestinationFloor == summon.Floor)
					return true;
			}
			return false;
		}

		public bool ContainsStop(ICarStatus stop)
		{
			foreach (ICarStatus current in stops)
			{
				if (current.DestinationFloor == stop.DestinationFloor && current.IsRequest == stop.IsRequest)
					return true;
			}
			return false;
		}

		public void EnqueueStop(ICarStatus stop)
		{
			if (stops.Count == 0)
			{
				stops.AddFirst(stop);
				return;
			}

			if (Direction == DesiredDirection.DOWN)
				EnqueueDownwardStop(stop);
			EnqueueUpwardStop(stop);
		}

		private void EnqueueDownwardStop(ICarStatus stop)
		{
			ICarStatus previous = null;
			foreach (ICarStatus current in stops)
			{
				if (current.DestinationFloor < stop.DestinationFloor)
					break;
				previous = current;
			}

			if (previous == null)
				stops.AddFirst(stop);
			else
			{
				LinkedListNode<ICarStatus> node = stops.FindLast(previous);
				stops.AddAfter(node, stop);
			}
		}

		private void EnqueueUpwardStop(ICarStatus stop)
		{
			ICarStatus previous = null;
			foreach (ICarStatus current in stops)
			{
				if (current.DestinationFloor > stop.DestinationFloor)
					break;
				previous = current;
			}

			if (previous == null)
				stops.AddFirst(stop);
			else
			{
				LinkedListNode<ICarStatus> node = stops.FindLast(previous);
				stops.AddAfter(node, stop);
			}
		}
	}
}
