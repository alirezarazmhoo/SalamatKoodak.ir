using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SalamatKoodak.Utility
{
	public static class Date
	{
		public static string ToPersianDateTime(this DateTime date)
		{
			PersianCalendar pc = new PersianCalendar();
			return string.Format("{0}/{1:d2}/{2:d2}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));
		}
	}
}