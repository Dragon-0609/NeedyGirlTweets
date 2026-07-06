using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Messages.Utils;

internal static class ReflectionExtensions
{
	private static readonly Dictionary<MemberKey, FieldInfo> _fieldCache = new();
	private static readonly Dictionary<MemberKey, PropertyInfo> _propertyCache = new();

	public static void SetPrivateExplicit<T>(this T self, string name, object value)
	{
		MemberKey key = new MemberKey(typeof(T), name);
		FieldInfo field;
		if (!_fieldCache.TryGetValue(key, out field))
		{
			field = key.type.GetField(key.name,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_fieldCache.Add(key, field);
		}

		field.SetValue(self, value);
	}

	public static void SetPrivate(this object self, string name, object value)
	{
		MemberKey key = new MemberKey(self.GetType(), name);
		FieldInfo field;
		if (!_fieldCache.TryGetValue(key, out field))
		{
			field = key.type.GetField(key.name,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_fieldCache.Add(key, field);
		}

		field.SetValue(self, value);
	}

	public static object GetPrivateExplicit<T>(this T self, string name)
	{
		MemberKey key = new MemberKey(typeof(T), name);
		FieldInfo field;
		if (!_fieldCache.TryGetValue(key, out field))
		{
			field = key.type.GetField(key.name,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_fieldCache.Add(key, field);
		}

		return field.GetValue(self);
	}

	public static object GetPrivate(this object self, string name)
	{
		MemberKey key = new MemberKey(self.GetType(), name);
		FieldInfo field;
		if (!_fieldCache.TryGetValue(key, out field))
		{
			field = key.type.GetField(key.name,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_fieldCache.Add(key, field);
		}

		return field.GetValue(self);
	}

	public static void SetPrivateProperty(this object self, string name, object value)
	{
		MemberKey key = new MemberKey(self.GetType(), name);
		PropertyInfo property;
		if (!_propertyCache.TryGetValue(key, out property))
		{
			property = key.type.GetProperty(key.name,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_propertyCache.Add(key, property);
		}

		property.SetValue(self, value, null);
	}

	public static object GetPrivateProperty(this object self, string name)
	{
		MemberKey key = new MemberKey(self.GetType(), name);
		PropertyInfo property;
		if (!_propertyCache.TryGetValue(key, out property))
		{
			property = key.type.GetProperty(key.name,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_propertyCache.Add(key, property);
		}

		return property.GetValue(self, null);
	}

	public static void SetPrivate(this Type self, string name, object value)
	{
		MemberKey key = new MemberKey(self, name);
		FieldInfo field;
		if (!_fieldCache.TryGetValue(key, out field))
		{
			field = key.type.GetField(key.name,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_fieldCache.Add(key, field);
		}

		field.SetValue(null, value);
	}

	public static object GetPrivate(this Type self, string name)
	{
		MemberKey key = new MemberKey(self, name);
		FieldInfo field;
		if (!_fieldCache.TryGetValue(key, out field))
		{
			field = key.type.GetField(key.name,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_fieldCache.Add(key, field);
		}

		return field.GetValue(null);
	}

	public static void SetPrivateProperty(this Type self, string name, object value)
	{
		MemberKey key = new MemberKey(self, name);
		PropertyInfo property;
		if (!_propertyCache.TryGetValue(key, out property))
		{
			property = key.type.GetProperty(key.name,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_propertyCache.Add(key, property);
		}

		property.SetValue(null, value, null);
	}

	public static object GetPrivateProperty(this Type self, string name)
	{
		MemberKey key = new MemberKey(self, name);
		PropertyInfo property;
		if (!_propertyCache.TryGetValue(key, out property))
		{
			property = key.type.GetProperty(key.name,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			_propertyCache.Add(key, property);
		}

		return property.GetValue(null, null);
	}

	public static object CallPrivate(this object self, string name, params object[] p)
	{
		return self.GetType().GetMethod(name,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
			?.Invoke(self, p);
	}

	public static object CallPrivate(this Type self, string name, params object[] p)
	{
		return self.GetMethod(name,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
			?.Invoke(null, p);
	}

	public static void LoadWith<T>(this T to, T from)
	{
		foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public |
		                                                BindingFlags.NonPublic))
		{
			if (field.FieldType.IsArray)
			{
				Array array = (Array)field.GetValue(from);
				Array instance = Array.CreateInstance(field.FieldType.GetElementType(), array.Length);
				for (int index = 0; index < array.Length; ++index)
					instance.SetValue(array.GetValue(index), index);
			}
			else
				field.SetValue(to, field.GetValue(from));
		}
	}

	public static MethodInfo GetCoroutineMethod(this Type objectType, string name)
	{
		Type type = null;
		name = "+<" + name + ">";
		foreach (Type nestedType in objectType.GetNestedTypes(BindingFlags.NonPublic))
		{
			if (nestedType.FullName.Contains(name))
			{
				type = nestedType;
				break;
			}
		}

		return type?.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public static byte[] GetResource(this Assembly a, string resourceName)
	{
		using (Stream manifestResourceStream = a.GetManifestResourceStream(resourceName))
		{
			byte[] buffer = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(buffer, 0, buffer.Length);
			return buffer;
		}
	}

	private struct MemberKey
	{
		public readonly Type type;
		public readonly string name;
		private readonly int _hashCode;

		public MemberKey(Type inType, string inName)
		{
			type = inType;
			name = inName;
			_hashCode = type.GetHashCode() ^ name.GetHashCode();
		}

		public override int GetHashCode() => _hashCode;
	}
}