﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Siegfried Pammer" email="siegfriedpammer@gmail.com" />
//     <version>$Revision$</version>
// </file>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Gui;

namespace ICSharpCode.SharpDevelop.Gui.OptionPanels
{
	public partial class SelectCulturePanel : OptionPanel
	{
		public SelectCulturePanel()
		{
			InitializeComponent();
			listView.ItemsSource = LanguageService.Languages;
		}
		
		static readonly string langPropName = "CoreProperties.UILanguage";
		
		public static Language CurrentLanguage {
			get { return GetCulture(PropertyService.Get(langPropName, "en")); }
			set { PropertyService.Set(langPropName, value.Code); }
		}
		
		static Language GetCulture(string languageCode)
		{
			return LanguageService.Languages.FirstOrDefault(x => x.Code.StartsWith(languageCode))
				?? LanguageService.Languages.First(x => x.Code.StartsWith("en"));
		}
	}
	
	public class ImageView : ViewBase
	{
		protected override object DefaultStyleKey
		{
			get { return new ComponentResourceKey(GetType(), "ImageView"); }
		}
		
		protected override object ItemContainerDefaultStyleKey
		{
			get { return new ComponentResourceKey(GetType(), "ImageViewItem"); }
		}
	}
}