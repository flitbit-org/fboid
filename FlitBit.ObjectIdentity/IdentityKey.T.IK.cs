using System;
using System.Diagnostics.Contracts;
using FlitBit.Core;

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	/// Utility class for working with a type's identity.
	/// </summary>
	/// <typeparam name="T">target type T</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	[IdentityKeyAutoGenerate2]
	[ContractClass(typeof(CodeContracts.ContractForIdentityKey<,>))]
	public abstract class IdentityKey<T, IK> : IdentityKey<T>, IEquatable<IdentityKey<T, IK>>
	{
		static readonly int CHashCodeSeed = typeof(IdentityKey<T, IK>).AssemblyQualifiedName.GetHashCode();

		/// <summary>
		/// Creates an instance.
		/// </summary>
		/// <param name="name">the key's name.</param>
		protected IdentityKey(string name)
			: base(typeof(IK), name)
		{
		}
		
		/// <summary>
		/// Gets an instance's identity key.
		/// </summary>
		/// <param name="instance">and instance of T</param>
		/// <returns>the instance's identity key (as an untyped object).</returns>
		public override object UntypedKey(T instance)
		{
			return Key(instance);
		}

		/// <summary>
		/// Gets an instance's identity key.
		/// </summary>
		/// <param name="instance">and instance of T</param>
		/// <returns>the instance's identity key.</returns>
		public abstract IK Key(T instance);

		/// <summary>
		/// Compares this object to another for equality.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em>.</returns>
		public override bool Equals(object obj)
		{
			return obj is IdentityKey<T, IK>
				&& Equals((IdentityKey<T, IK>)obj);
		}

		/// <summary>
		/// Compares this object to another for equality.
		/// </summary>
		/// <param name="other">the other object</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em>.</returns>
		public bool Equals(IdentityKey<T, IK> other)
		{
			// We already know T and IK are equal,
			// so if it refers to the same property then we're equal.
			return other != null
				&& String.Equals(this.KeyName, other.KeyName);
		}

		/// <summary>
		/// Generates a hashcode identifying the instance.
		/// </summary>
		/// <returns>a hashcode</returns>
		public override int GetHashCode()
		{
			var prime = Constants.NotSoRandomPrime;
			var res = CHashCodeSeed * prime;
			res ^= base.GetHashCode() * prime;
			return res;
		}
	}

	namespace CodeContracts
	{
		/// <summary>
		/// CodeContracts Class for IModel
		/// </summary>
		[ContractClassFor(typeof(IdentityKey<,>))]
		public abstract class ContractForIdentityKey<T, IK> : IdentityKey<T, IK>
		{
			private ContractForIdentityKey() : base("") { }

			/// <summary>
			/// Gets an instance's identity key.
			/// </summary>
			/// <param name="instance">and instance of T</param>
			/// <returns>the instance's identity key.</returns>
			public override IK Key(T instance)
			{
				Contract.Requires<ArgumentNullException>(instance != null);

				throw new NotImplementedException();
			}
		}
	}
}
