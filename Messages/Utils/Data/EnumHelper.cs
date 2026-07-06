using System;

namespace Messages.Utils.Data;

public static class EnumHelper
{
	public static bool TryGetEnum<TEnum>(int value, out TEnum result) where TEnum : struct, Enum
	{
		if (Enum.IsDefined(typeof(TEnum), value))
		{
			result = (TEnum)(object)value;
			return true;
		}

		result = default;
		return false;
	}
}