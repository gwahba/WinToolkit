using System;

using System.Reflection;


namespace WinToolkit
{
	public static class ReflectionManagement
	{
		public static PropertyInfo[] GetAllProperties(object obj)
		{
			return obj.GetType().GetProperties();
		}

		public static PropertyInfo GetPropertyInfo(object obj, string propetyName)
		{
			if (obj == null) { return null; }

			Type type = obj.GetType();
			PropertyInfo info = type.GetProperty(propetyName);
			return info;
		}

		private static object GetProperty(object obj, string AttrName)
		{
			if (obj == null) { return null; }

			Type type = obj.GetType();
			PropertyInfo info = type.GetProperty(AttrName);
			if (info == null) { return null; }

			obj = info.GetValue(obj, null);
			return obj;
		}
		public static AttributeType GetPropValue<AttributeType>(this Object obj, String name)
		{
			Object retval = GetProperty(obj, name);
			if (retval == null) { return default(AttributeType); }

			// throws InvalidCastException if types are incompatible
			return (AttributeType)retval;
		}
		public static bool Compare<T>(T x, T y) where T : class
		{
			return x == y;
		}
		public static ObjectType SetProbertyValue<ObjectType>(this object obj, string attrName, ObjectType val)
		{
			if (obj == null) { return default(ObjectType); }

			Type type = obj.GetType();
			PropertyInfo info = type.GetProperty(attrName);

			if (info == null) { return default(ObjectType); }
			info.SetValue(obj, val);

			return val;
		}
	}
}
