using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs
{
	class SportsmenRepository
	{
		private List<Sportsman> _sportsmen;

		public SportsmenRepository(IEnumerable<Sportsman> sportsmen)
		{
			_sportsmen = sportsmen.ToList();
		}

		public IEnumerable<Sportsman> Where(Func<Sportsman, bool> predicate)
		{
			return _sportsmen.Where(predicate);
		}
	}
}
