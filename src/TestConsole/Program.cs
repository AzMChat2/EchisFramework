using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data.Objects;
using System.Web.Script.Serialization;
using System.Text;

namespace TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Trace.Listeners.Add(new ConsoleTraceListener());

			try
			{
				Test();
			}
			catch (Exception ex)
			{
				TS.Logger.WriteException(ex);
			}
			finally
			{
				TS.Logger.Flush();
				Console.WriteLine("Done");
				Console.ReadLine();
			}
		}

		private static void Test()
		{
			long? nLong = null;

			List<object> list = new List<object>();
			list.Add("String");
			list.Add(12345L);
			list.Add(nLong);

			TS.Logger.WriteLine(XmlSerializer<List<object>>.SerializeToXml(list));

			TestClassCollection collection = new TestClassCollection();
			collection.Add(new TestClass()
			{
				Data = Encoding.ASCII.GetBytes("Test1"),
				Value = "Value1",
				Number = 1,
				Date = DateTime.Today,
				IsReady = true,
				NonProperty = "NonProp1",
				NNumber = 1,
				NDate = DateTime.Now,
				NIsReady = true
			});

			collection.Add(new TestClass()
			{
				Data = null,
				Value = null,
				Number = 2,
				Date = DateTime.Today,
				IsReady = false,
				NonProperty = null,
				NNumber = null,
				NDate = null,
				NIsReady = null
			});

			TestClass test3 = new TestClass()
			{
				Data = Encoding.ASCII.GetBytes("Test3"),
				Value = null,
				Number = 3,
				Date = DateTime.Today,
				IsReady = true,
				NonProperty = "NonProp3",
				NNumber = null,
				NDate = null,
				NIsReady = null
			};

			test3.Update();

			test3.Data = Encoding.ASCII.GetBytes("Test4");
			test3.Value = "Value4";
			test3.Number = 4;
			test3.Date = DateTime.Today.AddDays(1);
			test3.IsReady = false;
			test3.NonProperty = "NonProp4";
			test3.NNumber = 4;
			test3.NDate = DateTime.Now;
			test3.NIsReady = true;

			collection.Add(test3);

			string xmlData = XmlSerializer<TestClassCollection>.SerializeToXml(collection);
			TS.Logger.WriteLine(xmlData);

			JavaScriptSerializer serializer = new JavaScriptSerializer();
			TS.Logger.WriteLine(serializer.Serialize(collection));

			collection = XmlSerializer<TestClassCollection>.DeserializeFromXml(xmlData);
			collection.ForEach(Output);
		}

		static void Output(TestClass item)
		{
			TS.Logger.WriteLine(TS.Categories.Info, "{0}\t{1}\t{2}", item.Value, item.Number, item.NonProperty);
		}
	}

	public interface ITest : IBusinessObject
	{
		string Value { get; set; }
		byte[] Data { get; set; }
		long Number { get; set; }
		DateTime Date { get; set; }
		bool IsReady { get; set; }

		long? NNumber { get; set; }
		DateTime? NDate { get; set; }
		bool? NIsReady { get; set; }

		string NonProperty { get; set; }
	}

	public class TestClass : BusinessObject<ITest>, ITest
	{
		public TestClass()
		{
			base.AddProperties(
				new Property<string>() { DomainId = "TestClass", Name = "Value" },
				new Property<long>() { DomainId = "TestClass", Name = "Number" },
				new Property<DateTime>() { DomainId = "TestClass", Name = "Date" },
				new Property<bool>() { DomainId = "TestClass", Name = "IsReady" },
				new Property<long?>() { DomainId = "TestClass", Name = "NNumber" },
				new Property<DateTime?>() { DomainId = "TestClass", Name = "NDate" },
				new Property<bool?>() { DomainId = "TestClass", Name = "NIsReady" },
				new Property<byte[]>() { DomainId = "TestClass", Name = "Data" }
				);
		}

		protected override string DomainId { get { return "TestClass"; } }

		private Property<byte[]> _dataProperty;
		protected Property<byte[]> DataProperty
		{
			get
			{
				if (_dataProperty == null) _dataProperty = Properties["Data"] as Property<byte[]>;
				return _dataProperty;
			}
		}
		[XmlIgnore]
		public byte[] Data { get { return DataProperty.Value; } set { DataProperty.Value = value; } }
	
		private Property<string> _valueProperty;
		protected Property<string> ValueProperty
		{
			get
			{
				if (_valueProperty == null) _valueProperty = Properties["Value"] as Property<string>;
				return _valueProperty;
			}
		}
		[XmlIgnore]
		public string Value { get { return ValueProperty.Value; } set { ValueProperty.Value = value; } }

		private Property<long> _numberProperty;
		protected Property<long> NumberProperty 
		{
			get
			{
				if (_numberProperty == null) _numberProperty = Properties["Number"] as Property<long>;
				return _numberProperty;
			}
		}
		[XmlIgnore]
		public long Number { get { return NumberProperty.Value; } set { NumberProperty.Value = value; } }

		private Property<DateTime> _dateProperty;
		protected Property<DateTime> DateProperty
		{
			get
			{
				if (_dateProperty == null) _dateProperty = Properties["Date"] as Property<DateTime>;
				return _dateProperty;
			}
		}
		[XmlIgnore]
		public DateTime Date { get { return DateProperty.Value; } set { DateProperty.Value = value; } }

		private Property<bool> _isReadyProperty;
		protected Property<bool> IsReadyProperty
		{
			get
			{
				if (_isReadyProperty == null) _isReadyProperty = Properties["IsReady"] as Property<bool>;
				return _isReadyProperty;
			}
		}
		[XmlIgnore]
		public bool IsReady { get { return IsReadyProperty.Value; } set { IsReadyProperty.Value = value; } }

		private Property<long?> _nnumberProperty;
		protected Property<long?> NNumberProperty
		{
			get
			{
				if (_nnumberProperty == null) _nnumberProperty = Properties["NNumber"] as Property<long?>;
				return _nnumberProperty;
			}
		}
		[XmlIgnore]
		public long? NNumber { get { return NNumberProperty.Value; } set { NNumberProperty.Value = value; } }

		private Property<DateTime?> _ndateProperty;
		protected Property<DateTime?> NDateProperty
		{
			get
			{
				if (_ndateProperty == null) _ndateProperty = Properties["NDate"] as Property<DateTime?>;
				return _ndateProperty;
			}
		}
		[XmlIgnore]
		public DateTime? NDate { get { return NDateProperty.Value; } set { NDateProperty.Value = value; } }

		private Property<bool?> _nisReadyProperty;
		protected Property<bool?> NIsReadyProperty
		{
			get
			{
				if (_nisReadyProperty == null) _nisReadyProperty = Properties["NIsReady"] as Property<bool?>;
				return _nisReadyProperty;
			}
		}
		[XmlIgnore]
		public bool? NIsReady { get { return NIsReadyProperty.Value; } set { NIsReadyProperty.Value = value; } }

	
		[XmlAttribute]
		public string NonProperty { get; set; }
	}

	public class TestClassCollection : BusinessObjectCollection<TestClassCollection, TestClass, ITest>
	{
		protected override string DomainId { get { return "TestClass"; } }
	}
}
