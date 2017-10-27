using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TagTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            correlationBox.Items.Add(new ATagRange(correlationBox));
        }

        private class ATagRange : ListBoxItem
        {
            public TextBox StartBib { get; private set; }
            public TextBox EndBib { get; private set; }
            public TextBox StartChip { get; private set; }
            public Label EndChip { get; private set; }

            public ATagRange(ListBox correlationBox)
            {
                Grid theGrid = new Grid();
                this.Content = theGrid;
                int lastEndBib = 0, lastEndChip = 0;
                if (correlationBox.Items.Count > 0)
                {
                    ATagRange lastItem = (ATagRange) correlationBox.Items.GetItemAt(correlationBox.Items.Count - 1);
                    try
                    {
                        int.TryParse(lastItem.EndBib.Text, out lastEndBib);
                        int.TryParse(lastItem.EndChip.Content.ToString(), out lastEndChip);
                    }
                    catch {}
                }
                theGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                theGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                theGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                theGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                StartBib = new TextBox
                {
                    Text = String.Format("{0}", lastEndBib + 1),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(2, 2, 2, 2)
                };
                StartBib.TextChanged += new TextChangedEventHandler(this.StartBib_TextChanged);
                StartBib.KeyDown += new KeyEventHandler(this.KeyPressHandler);
                EndBib = new TextBox
                {
                    Text = String.Format("{0}", lastEndBib + 2),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(2, 2, 2, 2)
                };
                EndBib.TextChanged += new TextChangedEventHandler(this.EndBib_TextChanged);
                EndBib.KeyDown += new KeyEventHandler(this.KeyPressHandler);
                StartChip = new TextBox
                {
                    Text = String.Format("{0}", lastEndChip + 1),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(2, 2, 2, 2)
                };
                StartChip.TextChanged += new TextChangedEventHandler(this.StartChip_TextChanged);
                StartChip.KeyDown += new KeyEventHandler(this.KeyPressHandler);
                EndChip = new Label
                {
                    Content = String.Format("{0}", lastEndChip + 2),
                    Margin = new Thickness(2, 2, 2, 2)
                };
                theGrid.Children.Add(StartBib);
                theGrid.Children.Add(EndBib);
                theGrid.Children.Add(StartChip);
                theGrid.Children.Add(EndChip);
                Grid.SetColumn(StartBib, 0);
                Grid.SetColumn(EndBib, 1);
                Grid.SetColumn(StartChip, 2);
                Grid.SetColumn(EndChip, 3);
            }

            private void StartBib_TextChanged(object sender, EventArgs e)
            {
                StartBib.Text = StartBib.Text.Replace(" ", "");
                int startVal = -1, endVal = -1;
                int.TryParse(StartBib.Text, out startVal);
                int.TryParse(EndBib.Text, out endVal);
                if (startVal >= endVal)
                {
                    endVal = startVal + 1;
                }
                EndBib.Text = endVal.ToString();
                UpdateEndChip();
            }

            private void EndBib_TextChanged(object sender, EventArgs e)
            {
                EndBib.Text = EndBib.Text.Replace(" ", "");
                int startVal = -1, endVal = -1;
                int.TryParse(StartBib.Text, out startVal);
                int.TryParse(EndBib.Text, out endVal);
                if (startVal >= endVal)
                {
                    startVal = endVal - 1;
                }
                StartBib.Text = startVal.ToString();
                UpdateEndChip();
            }

            private void StartChip_TextChanged(object sender, EventArgs e)
            {
                StartChip.Text = StartChip.Text.Replace(" ", "");
                UpdateEndChip();
            }

            private void UpdateEndChip()
            {
                int startBib = -1, endBib = -1, startChip = -1, endChip;
                int.TryParse(StartBib.Text, out startBib);
                int.TryParse(EndBib.Text, out endBib);
                int.TryParse(StartChip.Text, out startChip);
                endChip = endBib - startBib + startChip;
                EndChip.Content = endChip.ToString();
            }

            private void KeyPressHandler(object sender, KeyEventArgs e)
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9) {}
                else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) {}
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void AddRange_Click(object sender, RoutedEventArgs e)
        {
            correlationBox.Items.Add(new ATagRange(correlationBox));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
