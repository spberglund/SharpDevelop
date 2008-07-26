﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.WpfDesign.PropertyGrid;
using System.Windows.Controls.Primitives;

namespace ICSharpCode.WpfDesign.PropertyGrid.Editors
{
	[TypeEditor(typeof(Enum))]
	public partial class ComboBoxEditor
	{
		public ComboBoxEditor()
		{
			InitializeComponent();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			var popup = (Popup)Template.FindName("PART_Popup", this);
			popup.SetValue(FontWeightProperty, FontWeights.Normal);
		}
	}
}
