using Common;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labs
{
	class Program
	{
		static void Main(string[] args)
		{
			IStorage storage = new IStorage();
			var users = storage.GetUsers();
		}
	}

	interface IInterface
	{
		int this[int x] { get; }
		event EventHandler<string> Event;
		int MyProperty { get; }
		void CallHidden();
	}

	abstract class SuperBase: IInterface
	{
		public int this[int index]
		{
			get { return 0; }
			set { /* set the specified index to value here */ }
		}

		private int Prop;

		public event EventHandler<string> Event;

		public int MyProperty { get; protected set; }

		public abstract void CallHidden();
	}

	class PhysicalBody
	{
		public virtual void CallOverriden()
		{
			Console.WriteLine("Base overriden");
		}

		public void CallHidden()
		{
			Console.WriteLine("Base hidden");
		}
	}

	class ChildBody : PhysicalBody
	{
		public override void CallOverriden()
		{
			Console.WriteLine("Child overriden");
		}

		public new void CallHidden()
		{
			Console.WriteLine("Child hidden");
		}
	}
}
