﻿using System;
using FlitBit.Core;
using FlitBit.Emit;
using FlitBit.ObjectIdentity.Tests.Models;
using FlitBit.Wireup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlitBit.ObjectIdentity.Tests
{	
		
	[TestClass]
	public class IdentityKeyTests
	{
		public TestContext TestContext { get; set; }


		[TestMethod]
		public void IdentityKey_ProperlyIdentifiesIdentityKeyMemberDeclaredOnPoco()
		{
			// Ensure the dynamic assembly gets dumped to disk so we can inspect it...
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;

			var rand = new Random();
			var factory = FactoryProvider.Factory;
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
			var factory = FactoryProvider.Factory;
			var ik = factory.CreateInstance<IdentityKey<IMy, string>>();
			
			Assert.IsTrue(ik.HasKey);
			Assert.AreEqual(typeof(String), ik.KeyType);
			Assert.AreEqual("Tag", ik.KeyName);

			// Use the strongly typed IDK...
			for (int i = 0; i < 100; ++i)
			{
				var r = rand.Next();
				var name = String.Concat("This is the ", r, " name");
				var my = factory.CreateInstance<IMy>();
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
			var factory = FactoryProvider.Factory;
			var ik = factory.CreateInstance<IdentityKey<IDerived>>();

			Assert.IsTrue(ik.HasKey);
			Assert.AreEqual(typeof(String), ik.KeyType);
			Assert.AreEqual("Tag", ik.KeyName);

			// Use the strongly typed IDK...
			for (int i = 0; i < 100; ++i)
			{
				var r = rand.Next();
				var name = String.Concat("This is the ", r, " name");
				var my = factory.CreateInstance<IDerived>();
				my.Name = name;
				my.Tag = Convert.ToString(r);
				my.Description = String.Concat("Derived: ", name);
				Assert.AreEqual(Convert.ToString(r), ik.UntypedKey(my));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IdentityKey_KnowsWhenIdentityKeyNotDefined()
		{
			// Ensure the dynamic assembly gets dumped to disk so we can inspect it...
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;

			var factory = FactoryProvider.Factory;
			var ik = factory.CreateInstance<IdentityKey<NoKey>>();
						
			Assert.IsFalse(ik.HasKey);
			Assert.IsNull(ik.KeyType);
			Assert.IsNull(ik.KeyName);

			var nk = new NoKey();
			Assert.IsNull(ik.UntypedKey(nk), "Causes the exception to be raised.");
		}
	}
}
