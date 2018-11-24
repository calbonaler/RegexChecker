using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RegexChecker
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow() { InitializeComponent(); }

		void TreeView_Loaded(object sender, RoutedEventArgs e)
		{
			var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(TreeView));
			dpd.RemoveValueChanged(sender, TreeView_ItemsSourceChanged);
			dpd.AddValueChanged(sender, TreeView_ItemsSourceChanged);
		}

		void TreeView_ItemsSourceChanged(object sender, EventArgs e)
		{
			var tv = (TreeView)sender;
			for (int i = 0; i < tv.Items.Count; i++)
				((TreeViewItem)tv.ItemContainerGenerator.ContainerFromIndex(i)).ExpandSubtree();
		}
	}
}
