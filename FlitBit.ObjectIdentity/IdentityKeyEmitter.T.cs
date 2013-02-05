using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading;
using FlitBit.Core;
using FlitBit.Emit;

namespace FlitBit.ObjectIdentity
{
	internal static class IdentityKeyEmitter
	{
		static readonly Lazy<EmittedModule> __module = new Lazy<EmittedModule>(() =>
		{ return RuntimeAssemblies.DynamicAssembly.DefineModule("ObjectIdentity", null); },
			LazyThreadSafetyMode.ExecutionAndPublication
			);

		static EmittedModule Module { get { return __module.Value; } }

		internal static MethodInfo TypeEmitter = typeof(IdentityKeyEmitter).GetGenericMethod("EmitType", BindingFlags.NonPublic | BindingFlags.Static, 0, 1);

		internal static void VerifyIdentityKeyType<T, IK>()
		{
			var keyProp = typeof(T).GetReadablePropertiesFromHierarchy(BindingFlags.Public | BindingFlags.Instance)
				.FirstOrDefault(p => p.IsDefined(typeof(IdentityKeyAttribute), true)
				);
			if (keyProp == null)
			{
				throw new InvalidOperationException(String.Concat("Identity key not defined for type: ",
					typeof(T).GetReadableFullName(), ".")
					);
			}
			if (typeof(IK) != keyProp.PropertyType)
			{
				throw new InvalidOperationException(String.Concat("Identity key type mismatch. ", 
					typeof(T).GetReadableFullName(), " declares an identity key on property `",
					keyProp.PropertyType, "` which doesn't agree with the requested type: ",
					typeof(IdentityKey<T,IK>).GetReadableFullName(), ".")
					);
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design.")]
		internal static Type EmitType<T>()
		{
			Contract.Ensures(Contract.Result<Type>() != null);

			var targetType = typeof(T);
			string typeName = RuntimeAssemblies.PrepareTypeName(targetType, "IdentityKey");

			var module = Module;
			lock (module)
			{
				Type type = module.Builder.GetType(typeName, false, false);
				if (type == null)
				{
					type = BuildType<T>(module, typeName);
				}
				return type;
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design.")]
		static Type BuildType<T>(EmittedModule module, string typeName)
		{
			Contract.Requires(module != null);
			Contract.Requires(typeName != null);
			Contract.Requires(typeName.Length > 0);
			Contract.Ensures(Contract.Result<Type>() != null);

			var keyProp = typeof(T).GetReadablePropertiesFromHierarchy(BindingFlags.Public | BindingFlags.Instance)
				.FirstOrDefault(p => p.IsDefined(typeof(IdentityKeyAttribute), true)
				);

			EmittedClass cls;
			if (keyProp == null)
			{
				return typeof(MissingIdentityKey<T>);
			}
			else
			{
				var supertype = typeof(IdentityKey<,>).MakeGenericType(typeof(T), keyProp.PropertyType);
				cls = module.DefineClass(typeName, EmittedClass.DefaultTypeAttributes, supertype, null);
				ConstructorInfo superCtor = supertype.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
						, null, new Type[] { typeof(string) }, null
						);
				cls.DefineCtor()
					.ContributeInstructions(
					(ctor, il) =>
					{
						il.LoadArg_0();
						il.LoadValue(keyProp.Name);
						il.Call(superCtor);
						il.Nop();
					});
				ImplementKeyMethod(cls, typeof(T), supertype, keyProp);			
			}
			cls.Attributes = TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.BeforeFieldInit;

			cls.Compile();
			return cls.Ref.Target;
		}

		private static void ImplementKeyMethod(EmittedClass cls, Type targetType, Type supertype, PropertyInfo keyProp)
		{
			Contract.Requires(cls != null);
			Contract.Requires(targetType != null);
			Contract.Requires(supertype != null);
			Contract.Requires(keyProp != null);
			
			cls.DefineOverrideMethod(supertype.GetMethod("Key", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { targetType }, null))
				.ContributeInstructions((m, il) =>
				{
					var exit = il.DefineLabel();
					il.DeclareLocal(keyProp.PropertyType);
					il.Nop();
					il.LoadArg_1();
					il.CallVirtual(keyProp.GetGetMethod());
					il.StoreLocal_0();
					il.Branch_ShortForm(exit);
					il.MarkLabel(exit);
					il.LoadLocal_0();
				});
		}

		
	}
}
