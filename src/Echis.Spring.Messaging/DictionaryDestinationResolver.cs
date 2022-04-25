using System;
using System.Collections.Generic;
using Apache.NMS;
using Spring.Messaging.Nms.Support.Destinations;

namespace System.Spring.Messaging
{
	/// <summary>
	/// Resolves Destinations by looking up the name from Dictionaries.
	/// </summary>
	[CLSCompliant(false)]
	public class DictionaryDestinationResolver : DynamicDestinationResolver
	{
		/// <summary>
		/// Creates a new instance of the Dictionary Destination Resolver
		/// </summary>
		public DictionaryDestinationResolver()
		{
			Topics = new Dictionary<string, IDestination>();
			Queues = new Dictionary<string, IDestination>();
			PassThroughIfNotFound = true;
		}

		/// <summary>
		/// Gets the dictionary instance containing Message Topic destinations
		/// </summary>
		public IDictionary<string, IDestination> Topics { get; private set; }

		/// <summary>
		/// Gets the dictionary instance containing Message Queue destinations
		/// </summary>
		public IDictionary<string, IDestination> Queues { get; private set; }

		/// <summary>
		/// Gets a flag determining behavior if the destination name is not found in the dictionary.
		/// </summary>
		public bool PassThroughIfNotFound { get; set; }

		/// <summary>
		/// Resolves the given destination name to a Queue.
		/// </summary>
		/// <param name="session">The current NMS Session</param>
		/// <param name="queueName">The name of the desired Queue.</param>
		protected override IDestination ResolveQueue(ISession session, string queueName)
		{
			if (Queues.ContainsKey(queueName))
			{
				return Queues[queueName];
			}
			else
			{
				if (!PassThroughIfNotFound) throw new ArgumentException("The specified Topic is not configured.");
				return base.ResolveQueue(session, queueName);
			}
		}

		/// <summary>
		/// Resolves the given destination name to a Topic.
		/// </summary>
		/// <param name="session">The current NMS Session</param>
		/// <param name="topicName">The name of the desired Topic.</param>
		protected override IDestination ResolveTopic(ISession session, string topicName)
		{
			if (Topics.ContainsKey(topicName))
			{
				return Topics[topicName];
			}
			else
			{
				if (!PassThroughIfNotFound) throw new ArgumentException("The specified Topic is not configured.");
				return base.ResolveTopic(session, topicName);
			}
		}
	}
}
