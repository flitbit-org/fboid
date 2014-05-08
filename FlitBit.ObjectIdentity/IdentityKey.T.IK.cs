#region COPYRIGHT© 2009-2014 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using System;
using System.Diagnostics.Contracts;
using FlitBit.Core;
using FlitBit.ObjectIdentity.CodeContracts;

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	///   Utility class for working with a type's identity.
	/// </summary>
	/// <typeparam name="T">target type T</typeparam>
	/// <typeparam name="TIdentityKey">identity key type IK</typeparam>
	[IdentityKeyAutoGenerate2, ContractClass(typeof(ContractForIdentityKey<,>))]
	public abstract class IdentityKey<T, TIdentityKey> : IdentityKey<T>, IEquatable<IdentityKey<T, TIdentityKey>>
	{
// ReSharper disable StaticFieldInGenericType
		static readonly int CHashCodeSeed = typeof(IdentityKey<T, TIdentityKey>).AssemblyQualifiedName.GetHashCode();
// ReSharper restore StaticFieldInGenericType

		/// <summary>
		///   Creates an instance.
		/// </summary>
		/// <param name="name">the key's name.</param>
		protected IdentityKey(string name)
			: base(typeof(TIdentityKey), name)
		{}

		/// <summary>
		///   Gets an instance's identity key.
		/// </summary>
		/// <param name="instance">and instance of T</param>
		/// <returns>the instance's identity key.</returns>
		public abstract TIdentityKey Key(T instance);

		/// <summary>
		///   Compares this object to another for equality.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns>
		///   <em>true</em> if equal; otherwise <em>false</em>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return obj is IdentityKey<T, TIdentityKey>
				&& Equals((IdentityKey<T, TIdentityKey>) obj);
		}

		/// <summary>
		///   Generates a hashcode identifying the instance.
		/// </summary>
		/// <returns>a hashcode</returns>
		public override int GetHashCode()
		{
			const int prime = Constants.NotSoRandomPrime;
			var res = CHashCodeSeed * prime;
			res ^= base.GetHashCode() * prime;
			return res;
		}

		/// <summary>
		///   Gets an instance's identity key.
		/// </summary>
		/// <param name="instance">and instance of T</param>
		/// <returns>the instance's identity key (as an untyped object).</returns>
		public override object UntypedKey(T instance)
		{
			return Key(instance);
		}

		#region IEquatable<IdentityKey<T,IK>> Members

		/// <summary>
		///   Compares this object to another for equality.
		/// </summary>
		/// <param name="other">the other object</param>
		/// <returns>
		///   <em>true</em> if equal; otherwise <em>false</em>.
		/// </returns>
		public bool Equals(IdentityKey<T, TIdentityKey> other)
		{
			// We already know T and IK are equal,
			// so if it refers to the same property then we're equal.
			return other != null
				&& String.Equals(this.KeyName, other.KeyName);
		}

		#endregion
	}

	namespace CodeContracts
	{
		/// <summary>
		///   CodeContracts Class for IModel
		/// </summary>
		[ContractClassFor(typeof(IdentityKey<,>))]
		public abstract class ContractForIdentityKey<T, TIdentityKey> : IdentityKey<T, TIdentityKey>
		{
			ContractForIdentityKey()
				: base("")
			{}

			/// <summary>
			///   Gets an instance's identity key.
			/// </summary>
			/// <param name="instance">and instance of T</param>
			/// <returns>the instance's identity key.</returns>
			public override TIdentityKey Key(T instance)
			{
				Contract.Requires<ArgumentNullException>(instance != null);

				throw new NotImplementedException();
			}
		}
	}
}