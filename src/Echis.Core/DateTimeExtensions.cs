using System.Diagnostics.CodeAnalysis;

namespace System
{
	/// <summary>
	/// Contains utility extension methods for System namespace objects.
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Determines if the Date falls within the day(s) specified.
		/// </summary>
		/// <param name="dateValue">The date to check.</param>
		/// <param name="day">The day(s) of the week.</param>
		/// <returns>Returns true if the date falls within the day(s) specified.</returns>
		public static bool IsDayOfTheWeek(this DateTime dateValue, DaysOfTheWeek day)
		{
			switch (dateValue.DayOfWeek)
			{
				case DayOfWeek.Sunday:
					return (day & DaysOfTheWeek.Sunday) != 0;
				case DayOfWeek.Monday:
					return (day & DaysOfTheWeek.Monday) != 0;
				case DayOfWeek.Tuesday:
					return (day & DaysOfTheWeek.Tuesday) != 0;
				case DayOfWeek.Wednesday:
					return (day & DaysOfTheWeek.Wednesday) != 0;
				case DayOfWeek.Thursday:
					return (day & DaysOfTheWeek.Thursday) != 0;
				case DayOfWeek.Friday:
					return (day & DaysOfTheWeek.Friday) != 0;
				case DayOfWeek.Saturday:
					return (day & DaysOfTheWeek.Saturday) != 0;
				default:
					return false;
			}
		}
	}

	/// <summary>
	/// Day of the Week.
	/// </summary>
	[Flags]
	[SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames",
		Justification = "Name is plural.")]
	public enum DaysOfTheWeek
	{
		/// <summary>
		/// No days selected.
		/// </summary>
		None = 0,
		/// <summary>
		/// Sunday
		/// </summary>
		Sunday = 1,
		/// <summary>
		/// Monday
		/// </summary>
		Monday = 2,
		/// <summary>
		/// Tuesday
		/// </summary>
		Tuesday = 4,
		/// <summary>
		/// Wednesday
		/// </summary>
		Wednesday = 8,
		/// <summary>
		/// Thursday
		/// </summary>
		Thursday = 16,
		/// <summary>
		/// Friday
		/// </summary>
		Friday = 32,
		/// <summary>
		/// Monday through Friday
		/// </summary>
		Weekdays = 62,
		/// <summary>
		/// Saturday
		/// </summary>
		Saturday = 64,
		/// <summary>
		/// Saturday and Sunday
		/// </summary>
		Weekends = 65,
		/// <summary>
		/// Sunday through Saturday
		/// </summary>
		Everyday = 127
	}
}
