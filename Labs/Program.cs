using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Labs
{
	class Program
	{
		static void Main(string[] args)
		{
			Person[] persons = new Person[]
			{
				new Person{Name = "Sergey", LastName = "Voroshilin", Age = 30, BirthDate = DateTime.Today.Subtract(TimeSpan.FromDays(100)), Logo = 'S'},
				new Person{Name = "Ivan", LastName = "Ivanov", Age = 15, BirthDate = DateTime.Today.Subtract(TimeSpan.FromDays(16)), Logo = 'F'},
				new Person{Name = "Petr", LastName = "Petrov", Age = 45, BirthDate = DateTime.Today.Subtract(TimeSpan.FromDays(140)), Logo = 'N'},
				new Person{Name = "Anna", LastName = "Anina", Age = 11, BirthDate = DateTime.Today.Subtract(TimeSpan.FromDays(66)), Logo = 'Q'},
				new Person{Name = "Ekaterina", LastName = "Katina", Age = 26, BirthDate = DateTime.Today.Subtract(TimeSpan.FromDays(44)), Logo = 'M'},
				new Person{Name = "Dmitry", LastName = "Dmitriev", Age = 14, BirthDate = DateTime.Today.Subtract(TimeSpan.FromDays(104340)), Logo = 'S'},
			};

			Console.WriteLine("Unordered:");
			PrintPersons(persons);
			Console.WriteLine();

			string[] propNames = MagicSorter<Person>.GetPropertyNames();

			for (int i = 0; i < propNames.Length; i++)
			{
				string propertyName = propNames[i];

				Console.WriteLine($"Ordered by {propertyName}:");
				var ordered = MagicSorter<Person>.OrderByPropertyName(persons, propertyName);
				PrintPersons(ordered);
				Console.WriteLine();
			}
			Console.ReadLine();
		}

		private static void PrintPersons(IEnumerable<Person> persons)
		{
			foreach (var person in persons)
			{
				Console.WriteLine(person.ToString());
			}
		}
	}

	class Person
	{
		public string Name { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public DateTime BirthDate { get; set; }
		public char Logo { get; set; }

		public override string ToString()
		{
			return $"{Name,-10}| {LastName,-11}| {Age,-3}| {BirthDate.ToShortDateString(),-11}| {Logo,-2}|";
		}
	}

	class MagicSorter<T>
	{
		private static Dictionary<string, Sorter<T>> _propNameToSelector = new Dictionary<string, Sorter<T>>();

		static MagicSorter()
		{
			var typeInfo = typeof(T);
			var propInfos = typeInfo.GetProperties();
			foreach (var propertyInfo in propInfos)
			{
				var parameters = Expression.Parameter(typeInfo);
				var ret = Expression.Property(parameters, typeInfo, propertyInfo.Name);

				var createSorterMethod = typeof(Sorter<T>).GetMethod(nameof(Sorter<T>.Create))
					.MakeGenericMethod(propertyInfo.PropertyType);

				var sorter =
					(Sorter<T>)createSorterMethod.Invoke(null, new[] { Expression.Lambda(ret, parameters).Compile() });
				_propNameToSelector.Add(propertyInfo.Name, sorter);
			}
		}

		public static IEnumerable<T> OrderByPropertyName(IEnumerable<T> list, string propName)
		{
			if (_propNameToSelector.ContainsKey(propName))
				return _propNameToSelector[propName].Order(list);

			throw new ArgumentException($@"Type ""{typeof(T).Name}"" does not have property ""{propName}""");
		}

		public static string[] GetPropertyNames()
		{
			return _propNameToSelector.Keys.ToArray();
		}

		#region Sorter
		private abstract class Sorter<T>
		{
			public static Sorter<T> Create<U>(Delegate d)
			{
				return new Sorter<T, U>(d as Func<T, U>);
			}

			public abstract IEnumerable<T> Order(IEnumerable<T> list);
		}

		private class Sorter<T, U> : Sorter<T>
		{
			public Func<T, U> _propertySelector;

			public Sorter(Func<T, U> selector)
			{
				this._propertySelector = selector;
			}

			public override IEnumerable<T> Order(IEnumerable<T> list)
			{
				return list.OrderBy(_propertySelector);
			}
		}
		#endregion
	}
}
