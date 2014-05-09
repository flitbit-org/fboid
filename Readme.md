# fboid - FlitBit.ObjectIdentity

A small framework for working with object identities in .NET.

## What

When dealing with objects that represent underlying data, it is useful to distinguish which property uniquely identifies the instance. This small framework provides an `IdentityKeyAttribute` for identifying such a property.

But it goes further than simply identifying the property. It produces run-time implementations of two abstract classes, `IdentityKey<TTarget>` and `IdentityKey<TTarget, TIdentityKey>`, which allows higher-order frameworks and user code to interact with the object's identity without having development time knowledge of the key.

## Why

Over time we noticed that we had produced more than one pattern for working with object identities in an abstract way. Each of these prior implementations relied on an interface something like:

```csharp
public interface IIdentifiable<TKey>
{
  TKey ID { get; }
}
```

This implementation worked just fine but as you can see, it requires that the identity property have the name `ID`, which is not very natural for most objects. In fact, in our opinion it looks suspiciously like a DBMS `surrogate key` bleeding on our object-oriented code. It makes sense for the `data model`; not so much for the `domain model` or our `view model`.

`ObjectIdentity` as its own small framework allows us to use `natural identities` where they appear in our object graph, for instance contrast the following declarations:

These interface declarations are a possible contract for the language subtags in the IANA language subtag registry. 

```csharp
public interface ILanguage
{
  [IdentityKey]
  string Subtag { get; }

  string Scope { get; }
  string PreferredValue { get; }
  string MacroLanguage { get; }
  string SuppressScript { get; }
  IEnumerable<string> Descriptions { get; }
  IEnumerable<string> Comments { get; }
  DateTime Added { get; }
  DateTime Deprecated { get; }
}
```

```c#
public interface ILanguage : IIdentifiable<string>
{
  string ID { get; }

  string Scope { get; }
  string PreferredValue { get; }
  string MacroLanguage { get; }
  string SuppressScript { get; }
  IEnumerable<string> Descriptions { get; }
  IEnumerable<string> Comments { get; }
  DateTime Added { get; }
  DateTime Deprecated { get; }
}
```

In the first declaration, it reads that the `Subtag` property is an instance's natural identity.

In the second declaration, making the interface conform to the implemented `IIdentifiable<string>` contract, the natural meaning of the `ID` property is lost. Obviously we can document the fact that `ID` means `Subtag`, but we think the former declaration conveys the meaning more efficiently because _it is what it means, it means what it is_.

## How

To use the framework you simply apply the `IdentityKey` attribute to the property that is the natural, identifying property. Not all objects will have a natural, identifying property -- but most will.

Once your objects declare thier identifying property, you can build utility classes and higher-order frameworks to make use of that information. For instance, I put together this simple, generic cache to illustrate:

```c#
  /// <summary>
  ///   Caches instances of type TTarget according to its identifying property.
  /// </summary>
  /// <typeparam name="TTarget"></typeparam>
  public class IdentifiableCache<TTarget>
  {
    readonly IEqualityComparer<TTarget> _comparer = EqualityComparer<TTarget>.Default;
    readonly ConcurrentDictionary<object, TTarget> _localCache = new ConcurrentDictionary<object, TTarget>();

    /// <summary>
    ///   Creates a new instance with the specified helper.
    /// </summary>
    /// <param name="helper">the target type's identity key helper</param>
    public IdentifiableCache(IdentityKey<TTarget> helper)
    {
      if (helper == null)
      {
        throw new ArgumentNullException("helper");
      }
      if (!helper.HasKey)
      {
        throw new InvalidOperationException(
          "Target type does not declare an identifying property: "
          + typeof(TTarget).Name
          );
      }
      this.Helper = helper;
    }

    /// <summary>
    ///   The target type's identity key helper.
    /// </summary>
    public IdentityKey<TTarget> Helper { get; private set; }

    /// <summary>
    ///   Put the instance in the cache, possibly overwriting prior instance.
    /// </summary>
    /// <param name="instance">the instance to cache</param>
    /// <returns>the instance (for chaining calls)</returns>
    public TTarget Put(TTarget instance)
    {
      if (_comparer.Equals(default(TTarget), instance))
      {
        throw new ArgumentException(
          "Cannot cache a default("
          + typeof(TTarget).Name + ")."
          );
      }

      var key = Helper.UntypedKey(instance);
      return _localCache.AddOrUpdate(key, instance, (k, prior) => instance);
    }

    /// <summary>
    ///   Gets an instance from the cache by key.
    /// </summary>
    /// <param name="key">an identifying key</param>
    /// <returns>
    ///   the identified instance if it is in the cache;
    ///   otherwise default(TTarget).
    /// </returns>
    public TTarget Get(object key)
    {
      if (key == null)
      {
        throw new ArgumentNullException("key");
      }

      TTarget res;
      if (_localCache.TryGetValue(key, out res))
      {
        return res;
      }
      return default(TTarget);
    }
  }
```

Constructing this class requires an instance of the appropriate `IdentityKey<TTarget>` helper which is dynamically emitted. If you also use [FlitBit.IoC](https://github.com/flitbit-org/fbioc) then everything will just work when you `Create.New<IdentifiableCache<ILanguage>>()`. 

If you use another IoC then you'll have to construct the dynamically emitted implementation and register it with your container. You can also fall back to [FlitBit.Core's](https://github.com/flitbit-org/fbcore) basic `FactoryProvider` which will ensure the implementation gets emitted.

```c#
using FlitBit.Core;

// ...

var factory = FactoryProvider.Factory;

// When you've got a factory you can get the dynamically generated type.
// Just register it with your IoC container of choice.

var ikType = factory.GetImplementationType<IdentityKey<ILanguage>>();

// or create one using the factory...

var ik = factory.CreateInstance<IdentityKey<ILanguage>>();

```

```
// TODO: Lots more documentation
```