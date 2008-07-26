﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Collections;

namespace ICSharpCode.WpfDesign.Designer.Converters
{
	public class IntFromEnumConverter : IValueConverter
	{
		public static IntFromEnumConverter Instance = new IntFromEnumConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Enum.ToObject(targetType, (int)value);
		}
	}

	public class HiddenWhenFalse : IValueConverter
	{
		public static HiddenWhenFalse Instance = new HiddenWhenFalse();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? Visibility.Visible : Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class LevelConverter : IValueConverter
	{
		public static LevelConverter Instance = new LevelConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new Thickness(2 + 14 * (int)value, 0, 0, 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class CollapsedWhenEmptyCollection : IValueConverter
	{
		public static CollapsedWhenEmptyCollection Instance = new CollapsedWhenEmptyCollection();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ICollection col = value as ICollection;
			if (col != null && col.Count == 0) {
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class FalseWhenNull : IValueConverter
	{
		public static FalseWhenNull Instance = new FalseWhenNull();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class BoldWhenTrue : IValueConverter
	{
		public static BoldWhenTrue Instance = new BoldWhenTrue();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? FontWeights.Bold : FontWeights.Normal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
