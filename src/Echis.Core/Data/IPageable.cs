
namespace System.Data
{
	/// <summary>
	/// Defines a Pageable object.
	/// </summary>
	public interface IPageable
	{
		/// <summary>
		/// Gets the current page of data represented by the object
		/// </summary>
		int CurrentPage { get; }

		/// <summary>
		/// Gets the size of the page (e.g. Number of Records per page)
		/// </summary>
		int PageSize { get; }

		/// <summary>
		/// Gets the total number of records available across all pages
		/// </summary>
		int TotalCount { get; }
	}

}
