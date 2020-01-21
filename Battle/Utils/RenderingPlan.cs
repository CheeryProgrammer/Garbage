using Battle.Common;
using Battle.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle.Utils
{
	class RenderingPlan
	{
		private Queue<RenderingElement> _elements { get; }
		private Queue<PlannedElement> _planned { get; }

		public RenderingPlan()
		{
			_elements = new Queue<RenderingElement>();
			_planned = new Queue<PlannedElement>();
		}

		public void Add(Point2D from, Point2D to, IView view)
		{
			_elements.Enqueue(new RenderingElement(from, to, view));
			foreach(var planned in _planned.Where(t => t.RenderingElement.To.Equals(to)))
				planned.Cancel();
		}

		public IEnumerable<RenderingElement> GetElements()
		{
			int count = _elements.Count;
			for (int i = 0; i < count; i++)
			{
				yield return _elements.Dequeue();
			}
			DateTime now = DateTime.Now;

			while(_planned.Any() && _planned.Peek().DateTime < now)
			{
				PlannedElement task = _planned.Dequeue();
				if (!task.Cancelled)
					yield return task.RenderingElement;
			}
		}

		internal void AddTemporarily(Point2D position, IView view)
		{
			Add(position, position, view);
			PlanForLater(position, new ClearView(), TimeSpan.FromMilliseconds(300));
		}

		private void PlanForLater(Point2D position, IView view, TimeSpan delay)
		{
			_planned.Enqueue(new PlannedElement(new RenderingElement(position, position, view), DateTime.Now.Add(delay)));
		}
	}

	struct RenderingElement
	{
		public Point2D From { get; }
		public Point2D To { get; }
		public IView View { get; }

		internal RenderingElement(Point2D from, Point2D to, IView view)
		{
			From = from;
			To = to;
			View = view;
		}
	}

	class PlannedElement
	{
		public RenderingElement RenderingElement { get; }
		public DateTime DateTime { get; }
		public bool Cancelled { get; private set; }

		public PlannedElement(RenderingElement renderingElement, DateTime dateTime)
		{
			RenderingElement = renderingElement;
			DateTime = dateTime;
			Cancelled = false;
		}

		public void Cancel()
		{
			Cancelled = true;
		}
	}
}
