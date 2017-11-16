﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            public Button Remove { get; private set; }

            ListBox parent;

            public ATagRange(ListBox correlationBox)
            {
                Grid theGrid = new Grid();
                this.Content = theGrid;
                int lastEndBib = 0, lastEndChip = 0;
                parent = correlationBox;
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
                    Text = String.Format("{0}", lastEndBib + 1),
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
                    Content = String.Format("{0}", lastEndChip + 1),
                    Margin = new Thickness(2, 2, 2, 2)
                };
                Remove = new Button
                {
                    Content = "Remove",
                    Height = 25,
                    Width = 65
                };
                Remove.Click += new RoutedEventHandler(this.Remove_Click);
                theGrid.Children.Add(StartBib);
                theGrid.Children.Add(EndBib);
                theGrid.Children.Add(StartChip);
                theGrid.Children.Add(EndChip);
                theGrid.Children.Add(Remove);
                Grid.SetColumn(StartBib, 0);
                Grid.SetColumn(EndBib, 1);
                Grid.SetColumn(StartChip, 2);
                Grid.SetColumn(EndChip, 3);
                Grid.SetColumn(Remove, 4);
            }

            private void Remove_Click(object sender, EventArgs e)
            {
                Log.D("Removing an item.");
                try
                {
                    parent.Items.Remove(this);
                }
                catch { }
            }

            private void StartBib_TextChanged(object sender, EventArgs e)
            {
                string replaceStr = StartBib.Text.Replace(" ", "");
                if (StartBib.Text.Length != replaceStr.Length)
                {
                    StartBib.Text = replaceStr;
                }
                int startVal = -1;
                int.TryParse(StartBib.Text, out startVal);
                if (string.CompareOrdinal(StartBib.Text, startVal.ToString()) != 0)
                {
                    StartBib.Text = startVal.ToString();
                }
                UpdateEndChip();
            }

            private void EndBib_TextChanged(object sender, EventArgs e)
            {
                string replaceStr = EndBib.Text.Replace(" ", "");
                if (EndBib.Text.Length != replaceStr.Length)
                {
                    EndBib.Text = replaceStr;
                }
                int endVal = -1;
                int.TryParse(EndBib.Text, out endVal);
                if (string.CompareOrdinal(EndBib.Text, endVal.ToString()) != 0)
                {
                    EndBib.Text = endVal.ToString();
                }
                UpdateEndChip();
            }

            private void StartChip_TextChanged(object sender, EventArgs e)
            {
                string replaceStr = StartChip.Text.Replace(" ", "");
                if (StartChip.Text.Length != replaceStr.Length)
                {
                    StartChip.Text = replaceStr;
                }
                int startChip = -1;
                int.TryParse(StartChip.Text, out startChip);
                if (string.CompareOrdinal(StartChip.Text, startChip.ToString()) != 0)
                {
                    StartChip.Text = startChip.ToString();
                }
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

        private void Reset()
        {
            correlationBox.Items.Clear();
            correlationBox.Items.Add(new ATagRange(correlationBox));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            List<Range> ranges = new List<Range>();
            foreach (ATagRange tag in correlationBox.Items)
            {
                int startBib = -1, endBib = -1, startChip = -1, endChip = -1;
                int.TryParse(tag.StartBib.Text, out startBib);
                int.TryParse(tag.EndBib.Text, out endBib);
                int.TryParse(tag.StartChip.Text, out startChip);
                int.TryParse(tag.EndChip.Content.ToString(), out endChip);
                Log.D("StartBib " + startBib + " EndBib " + endBib + " StartChip " + startChip + " EndChip " + endChip);
                Range curRange = new Range
                {
                    StartBib = startBib,
                    EndBib = endBib,
                    StartChip = startChip,
                    EndChip = endChip
                };
                bool conflicts = !curRange.IsValid();
                foreach (Range r in ranges)
                {
                    if (r.Violates(curRange))
                    {
                        conflicts = true;
                    }
                }
                if (conflicts)
                {
                    MessageBox.Show("One or more values is in conflict. Please fix the error and try again.");
                    return;
                }
                ranges.Add(curRange);
            }
            // Create file in correct place.
            String fileName = "Tag File";
            String fileExt = ".txt";
            String filePath = "";
            switch (SavePlace.SelectedIndex)
            {
                case 0: // HERE
                    filePath = fileName + fileExt;
                    int Number = 1;
                    while (File.Exists(filePath))
                    {
                        filePath = fileName + " (" + Number++ + ")" + fileExt;
                    }
                    break;
                case 1: // Public Docs
                    String directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "TagTool");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    filePath = Path.Combine(directory, fileName + fileExt);
                    int nNumber = 1;
                    while (File.Exists(filePath))
                    {
                        filePath = Path.Combine(directory, fileName + " (" + nNumber++ + ")" + fileExt);
                    }
                    break;
                case 2: // Custom
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Txt files (*.txt)|*.txt";
                    saveFileDialog.FileName = fileName;
                    Nullable<bool> result = saveFileDialog.ShowDialog();
                    if (result != null && result == true)
                    {
                        filePath = saveFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                    break;
                default:
                    return;
            }
            ranges.Sort();
            using (FileStream outFile = File.Create(filePath))
            {
                String format = "{0},{1}"; // BIB, TAG
                using (StreamWriter outWriter = new StreamWriter(outFile))
                {
                    foreach (Range r in ranges)
                    {
                        for (int bib = r.StartBib, tag = r.StartChip; bib <= r.EndBib && tag <= r.EndChip; bib++, tag++)
                        {
                            outWriter.WriteLine(String.Format(format, bib, tag));
                        }
                    }
                }
            }
            Reset();
        }
    }
}
