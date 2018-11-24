using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RegexChecker
{
	static class Utils
	{
		public static void Raise(this PropertyChangedEventHandler handler, object sender, [CallerMemberName]string propertyName = "") => handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));

		public static void SetProperty<T>(ref T storage, T value, Action<string> raiser, [CallerMemberName]string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(storage, value))
				return;
			storage = value;
			raiser(propertyName);
		}

		public static void SetProperty<T>(ref T storage, T value, PropertyChangedEventHandler handler, object sender, [CallerMemberName]string propertyName = "") => SetProperty(ref storage, value, name => handler(sender, new PropertyChangedEventArgs(name)), propertyName);
	}
}
