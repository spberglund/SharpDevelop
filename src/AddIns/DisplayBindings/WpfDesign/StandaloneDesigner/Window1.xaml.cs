﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

using ICSharpCode.WpfDesign;
using ICSharpCode.WpfDesign.Designer;
using ICSharpCode.WpfDesign.Designer.Xaml;
using ICSharpCode.WpfDesign.PropertyGrid;
using System.Threading;
using System.Windows.Threading;

namespace StandaloneDesigner
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			DragDropExceptionHandler.HandleException = delegate (Exception ex) {
				MessageBox.Show(ex.ToString());
			};
			try {
				InitializeComponent();
				CodeTextBox.Text = File.ReadAllText("Files/1.xaml");
				foreach (object o in toolBar.Items) {
					if (o is Button) {
						(o as Button).CommandTarget = designSurface;
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
				Close();
			}
		}
		
		void tabControlSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (e.Source != tabControl) return;
			if (tabControl.SelectedItem == designTab) {
				XamlLoadSettings settings = new XamlLoadSettings();
				settings.CustomServiceRegisterFunctions.Add(
					delegate(XamlDesignContext context) {
						context.Services.AddService(typeof(IEventHandlerService), new EventHandlerService(this));
					});
				
				designSurface.LoadDesigner(new XmlTextReader(new StringReader(CodeTextBox.Text)), settings);
				designSurface.DesignContext.Services.Selection.SelectionChanged += OnSelectionChanged;
				toolbox.ToolService = designSurface.DesignContext.Services.Tool;
			} else {
				if (designSurface.DesignContext != null) {
					propertyGridView.PropertyGrid.SelectedItems = null;
					
					using (StringWriter writer = new StringWriter()) {
						using (XmlTextWriter xmlWriter = new XmlTextWriter(writer)) {
							xmlWriter.Formatting = Formatting.Indented;
							designSurface.SaveDesigner(xmlWriter);
						}
						CodeTextBox.Text = writer.ToString();
					}
				}
				designSurface.UnloadDesigner();
				toolbox.ToolService = null;
			}
		}
		
		ICollection<DesignItem> oldItems = new DesignItem[0];
		
		void OnSelectionChanged(object sender, DesignItemCollectionEventArgs e)
		{
			ISelectionService selectionService = designSurface.DesignContext.Services.Selection;
			ICollection<DesignItem> items = selectionService.SelectedItems;
			if (!IsCollectionWithSameElements(items, oldItems)) {
				propertyGridView.PropertyGrid.SelectedItems = items;
				oldItems = items;
			}
		}
		
		static bool IsCollectionWithSameElements(ICollection<DesignItem> a, ICollection<DesignItem> b)
		{
			return ContainsAll(a, b) && ContainsAll(b, a);
		}
		
		static bool ContainsAll(ICollection<DesignItem> a, ICollection<DesignItem> b)
		{
			foreach (DesignItem item in a) {
				if (!b.Contains(item))
					return false;
			}
			return true;
		}

		void TestButtonClick(object sender, EventArgs e)
		{
			DesignItem[] c = new List<DesignItem>(designSurface.DesignContext.Services.Selection.SelectedItems).ToArray();
			if (c.Length < 2) return;
			int index = 0;
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			timer.Tick += delegate {
				index++;
				designSurface.DesignContext.Services.Selection.SetSelectedComponents(new DesignItem[] { c[index % c.Length] });
			};
			timer.Start();
		}
	}
}