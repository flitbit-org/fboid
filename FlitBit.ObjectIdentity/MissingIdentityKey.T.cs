using System;
using FlitBit.Core;

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	/// Implementation of IdentityKey for types that don't declare an identity key.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class MissingIdentityKey<T> : IdentityKey<T>
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public MissingIdentityKey()
		{
		}

		/// <summary>
		/// Gets an instance's identity key.
		/// </summary>
		/// <param name="instance">and instance of T</param>
		/// <returns>the instance's identity key (as an untyped object).</returns>
		/// <exception cref="InvalidOperationException">thrown when target type T does not define an identity key.</exception>
		public override object UntypedKey(T instance)
		{
			throw new InvalidOperationException(String.Concat("Identity key not defined for type: ", typeof(T).GetReadableFullName(), "."));
		}
	}
}
