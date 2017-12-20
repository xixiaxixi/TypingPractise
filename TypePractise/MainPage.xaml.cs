using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace TypePractise
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int letterNum = 5;
        private string line = "qwer tyuiop,.;'";
        private string practiseLine = null;
        DispatcherTimer timer;
        double time;

        private static ObservableCollection<ListViewItem> items = new ObservableCollection<ListViewItem>();

        private bool isEditingLine = false;
        private bool isStart = false;

        private SolidColorBrush black = new SolidColorBrush(Colors.Black);
        private SolidColorBrush red = new SolidColorBrush(Colors.Red);
        private SolidColorBrush green = new SolidColorBrush(Colors.Green);



        public MainPage()
        {
            this.InitializeComponent();

            textBox_practising.IsSpellCheckEnabled = false;
            textBox_line.Text = line;
            textBox_letterNum.Text = letterNum.ToString();

            time = 0;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(1000000);
            timer.Tick += (s, e) =>
            {
                time += 0.1;
                textBlock_time.Text = time.ToString("#.#");
            };

            InitBlock();

            listView_timelist.ItemsSource = items;

        }

        private void InitBlock()
        {
            textBox_practising.Text = "";
            textBlock.Inlines.Clear();

            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < letterNum; i++)
            {
                sb.Append(line[r.Next(line.Length)]);
            }
            practiseLine = sb.ToString().Replace("  ", " ").Trim();
            foreach (var c in practiseLine)
            {
                textBlock.Inlines.Add(item: new Run { Text = c.ToString(), Foreground = black });
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isCorrect = true;

            if (!isStart)
            {
                timer.Start();
            }
            isStart = true;

            string txt = ((TextBox)sender).Text;
            int oriTxtLength = textBlock.Inlines.Count;
            int length = txt.Length < oriTxtLength ? txt.Length : oriTxtLength;
            if (length == 0)
            {
                time = 0;
                timer.Stop();
                isStart = false;
            }
            for (int i = 0; i < length; i++)
            {
                if (txt[i] == practiseLine[i])
                {
                    textBlock.Inlines[i].Foreground = green;
                }
                else
                {
                    isCorrect = false;
                    textBlock.Inlines[i].Foreground = red;
                }
            }
            for(int i=length;i< oriTxtLength; i++)
            {
                textBlock.Inlines[i].Foreground = black;
            }

            if (isCorrect && oriTxtLength == length)
            {
                timer.Stop();
                ListViewItem item = new ListViewItem();
                item.Content = time.ToString("#.#");
                items.Insert(0, item);
                button_change_line_Click(0, null);
            }

        }

        private void button_change_line_Click(object sender, RoutedEventArgs e)
        {
            if (isEditingLine || sender is int)
            {
                isEditingLine = !isEditingLine;
                button_change_line.Content = "ChangeLetter";
                textBlock_time.Text = "0";
                textBox_line.IsEnabled = false;
                textBox_practising.IsEnabled = true;
                line = textBox_line.Text;
                letterNum = Convert.ToInt32(textBox_letterNum.Text);
                InitBlock();
                isStart = false;
            }
            else
            {
                isEditingLine = !isEditingLine;
                timer.Stop();
                button_change_line.Content = "Change";
                textBox_line.IsEnabled = true;
                textBox_practising.IsEnabled = false;
            }
        }


    }
}
