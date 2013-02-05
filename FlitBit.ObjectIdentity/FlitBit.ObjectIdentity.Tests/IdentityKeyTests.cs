using System;
using FlitBit.Emit;
using FlitBit.IoC;
using FlitBit.ObjectIdentity.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlitBit.Core;
using FlitBit.Wireup;

namespace FlitBit.ObjectIdentity.Tests
{	
		
	[TestClass]
	public class IdentityKeyTests
	{
		[TestInitialize]
		public void Init()
		{
			WireupCoordinator.SelfConfigure();
		}

		[TestMethod]
		public void IdentityKey_ProperlyIdentifiesIdentityKeyMemberDeclaredOnPoco()
		{
			// Ensure the dynamic assembly gets dumped to disk so we can inspect it...
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;

			var rand = new Random();
			var factory = FactoryFactory.Instance;
			var ik = factory.CreateInstance<IdentityKey<My, string>>();
			
			Assert.IsTrue(ik.HasKey);
			Assert.AreEqual(typeof(String), ik.KeyType);
			Assert.AreEqual("Name", ik.KeyName);

			for (int i = 0; i < 100; ++i)
			{
				var r = rand.Next();
				var name = String.Concat("This is the ", r, " name");
				var my = new My { Name = name, Tag = Convert.ToString(r) };
				Assert.AreEqual(name, ik.Key(my));
			}
		}

		[TestMethod]
		public void IdentityKey_ProperlyIdentifiesIdentityKeyMemberDeclaredOnInterface()
		{
			// Ensure the dynamic assembly gets dumped to disk so we can inspect it...
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;

			var rand = new Random();
			var factory = FactoryFactory.Instance;
			var ik = factory.CreateInstance<IdentityKey<IMy, string>>();
			
			Assert.IsTrue(ik.HasKey);
			Assert.AreEqual(typeof(String), ik.KeyType);
			Assert.AreEqual("Tag", ik.KeyName);

			// Use the strongly typed IDK...
			for (int i = 0; i < 100; ++i)
			{
				var r = rand.Next();
				var name = String.Concat("This is the ", r, " name");
				var my = Create.New<IMy>();
				my.Name = name;
				my.Tag = Convert.ToString(r);
				Assert.AreEqual(Convert.ToString(r), ik.Key(my));
			}		
		}

		[TestMethod]
		public void IdentityKey_ProperlyIdentifiesIdentityKeyMemberDeclaredOnInheritedInterface()
		{
			// Ensure the dynamic assembly gets dumped to disk so we can inspect it...
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;

			var rand = new Random();
			var factory = FactoryFactory.Instance;
			var ik = factory.CreateInstance<IdentityKey<IDerived, string>>();

			Assert.IsTrue(ik.HasKey);
			Assert.AreEqual(typeof(String), ik.KeyType);
			Assert.AreEqual("Tag", ik.KeyName);

			// Use the strongly typed IDK...
			for (int i = 0; i < 100; ++i)
			{
				var r = rand.Next();
				var name = String.Concat("This is the ", r, " name");
				var my = Create.New<IDerived>();
				my.Name = name;
				my.Tag = Convert.ToString(r);
				my.Description = String.Concat("Derived: ", name);
				Assert.AreEqual(Convert.ToString(r), ik.Key(my));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityKey_KnowsWhenIdentityKeyNotDefined()
		{
			// Ensure the dynamic assembly gets dumped to disk so we can inspect it...
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;

			var rand = new Random();
			var factory = FactoryFactory.Instance;
			var ik = factory.CreateInstance<IdentityKey<NoKey>>();
						
			Assert.IsFalse(ik.HasKey);
			Assert.IsNull(ik.KeyType);
			Assert.IsNull(ik.KeyName);

			var nk = new NoKey();
			var k = ik.UntypedKey(nk);
		}
	}
}
