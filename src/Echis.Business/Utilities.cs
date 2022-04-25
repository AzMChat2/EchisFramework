using System.Data.Objects;

namespace System.Data.Objects.DataClasses
{
	/// <summary>
	/// Extension Methods for Entity Framework
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// Adds or Attaches an entity object depending upon if it is New or Existing.
		/// </summary>
		/// <typeparam name="T">The type of Entity object to be added or attached.</typeparam>
		/// <param name="objectSet">The set of objects to which the entity will be added or attached.</param>
		/// <param name="entity">The entity object to be added or attached.</param>
		public static void AddOrAttach<T>(this ObjectSet<T> objectSet, T entity)
			where T : EntityObject, IBusinessObject
		{
			if (objectSet == null) throw new ArgumentNullException("objectSet");
			if (entity == null) throw new ArgumentNullException("entity");

			if (entity.EntityKey == null)
			{
				objectSet.AddObject(entity);
			}
			else
			{
				objectSet.Attach(entity);
				objectSet.Context.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
			}
		}
	}
}
