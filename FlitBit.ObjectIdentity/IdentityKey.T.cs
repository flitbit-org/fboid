#region COPYRIGHT© 2009-2014 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using System;
using System.Diagnostics.Contracts;
using FlitBit.Core;

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	///   Utility class for working with a type's identity.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[IdentityKeyAutoGenerate]
	public abstract class IdentityKey<T>
	{
// ReSharper disable StaticFieldInGenericType
		static readonly int CHashCodeSeed = typeof(IdentityKey<T>).AssemblyQualifiedName.GetHashCode();
// ReSharper restore StaticFieldInGenericType

		/// <summary>
		///   Creates an instance.
		/// </summary>
		protected IdentityKey()
			: this(null, null)
		{}

		/// <summary>
		///   Creates an instance.
		/// </summary>
		/// <param name="type">the key's type.</param>
		/// <param name="name">the key's name.</param>
		protected IdentityKey(Type type, string name)
		{
			HasKey = type != null;
			KeyType = type;
			KeyName = name;
		}

		/// <summary>
		///   Indicates whether type T has an identity key.
		/// </summary>
		public bool HasKey { get; private set; }

		/// <summary>
		///   Indicates type T's identity key's name.
		/// </summary>
		public string KeyName { get; private set; }

		/// <summary>
		///   Indicates type T's identity key type.
		/// </summary>
		public Type KeyType { get; private set; }

		/// <summary>
		///   Gets an instance's identity key.
		/// </summary>
		/// <param name="instance">and instance of T</param>
		/// <returns>the instance's identity key (as an untyped object).</returns>
		/// <exception cref="InvalidOperationException">thrown when target type T does not define an identity key.</exception>
		public abstract object UntypedKey(T instance);

		/// <summary>
		///   Compares this object to another for equality.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns>
		///   <em>true</em> if equal; otherwise <em>false</em>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return obj is IdentityKey<T>
				&& Equals((IdentityKey<T>) obj);
		}

		/// <summary>
		///   Generates a hashcode identifying the instance.
		/// </summary>
		/// <returns>a hashcode</returns>
		public override int GetHashCode()
		{
			const int prime = Constants.NotSoRandomPrime;
			var res = CHashCodeSeed * prime;
			res ^= this.KeyName.GetHashCode() * prime;
			return res;
		}

		/// <summary>
		///   Compares this object to another for equality.
		/// </summary>
		/// <param name="other">the other object</param>
		/// <returns>
		///   <em>true</em> if equal; otherwise <em>false</em>.
		/// </returns>
		public bool Equals(IdentityKey<T> other)
		{
			// We already know T is equal,
			// so if it refers to the same property then we're equal.
			return other != null
				&& String.Equals(this.KeyName, other.KeyName);
		}
	}
}