using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Collections
{
	/// <summary>
	/// 
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class StringBlockingQueue : BlockingQueue<string>, IXmlSerializable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		public void ReadXml(XmlReader reader)
		{
			reader.WhileRead(Enqueue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		private void Enqueue(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			if (reader.IsStartElement("Item")) Enqueue(reader.ReadContentAsString());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			string item;
			while ((item = Dequeue()) != null)
			{
				writer.WriteElementString("Item", item);
			}
		}
	}

	namespace Generic
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
		public class BlockingQueue<T> where T : class
		{
			/// <summary>
			/// 
			/// </summary>
			private Queue<T> _innerQueue = new Queue<T>();
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public T Dequeue()
			{
				lock (_innerQueue)
				{
					if (_innerQueue.Count == 0) return default(T);
					DequeuedCount++;
					return _innerQueue.Dequeue();
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="item"></param>
			public void Enqueue(T item)
			{
				lock (_innerQueue)
				{
					AddItem(item);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="item"></param>
			private void AddItem(T item)
			{
				if (item == null) return;
				EnqueuedCount++;
				_innerQueue.Enqueue(item);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
			public void Enqueue(IEnumerable<T> collection)
			{
				if (collection == null) throw new ArgumentNullException("collection");
				lock (_innerQueue)
				{
					collection.ForEach(AddItem);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public void Clear()
			{
				lock (_innerQueue)
				{
					_innerQueue.Clear();
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public int Count
			{
				get
				{
					return _innerQueue.Count;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Enqueued")]
			public int EnqueuedCount { get; private set; }

			/// <summary>
			/// 
			/// </summary>
			[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dequeued")]
			public int DequeuedCount { get; private set; }

			/// <summary>
			/// 
			/// </summary>
			public bool Completed
			{
				get
				{
					if (EnqueuedCount == 0) return false;
					return DequeuedCount == EnqueuedCount;
				}
			}
		}
	}
}