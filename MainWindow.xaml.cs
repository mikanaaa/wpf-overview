﻿using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfOverview.Models;
using WpfOverview.Views;
using WpfOverview.Visual;

namespace WpfOverview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<LineModel> lines;

        public MainWindow()
        {
            InitializeComponent();
            spData.DataContext = new Person() { FirstName = "Will", LastName = "Smith" };
            ObservableCollection<Person> people = new ObservableCollection<Person>() { };
            dgPeople.ItemsSource = people;
            dgPeople2.ItemsSource = people;

            people.Add(new Person() { FirstName = "Will", LastName = "Smith" });
            people.Add(new Person() { FirstName = "Will2", LastName = "Smith2" });
            people.Add(new Person() { FirstName = "Will3", LastName = "Smith3" });

            ObservableCollection<SuperPerson> superPeople = new ObservableCollection<SuperPerson>() { };
            dgOrganized.ItemsSource = superPeople;
            superPeople.Add(new SuperPerson("John", "Doe", 42, true));
            superPeople.Add(new SuperPerson("Josh", "Billy", 812, false));
            superPeople.Add(new SuperPerson("Janny", "Johnsy", 52, false));
            superPeople.Add(new SuperPerson("Jill", "Ariel", 22, true));

            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((sender as Button).Content as TextBlock).Text);
            MessageBox.Show(e.ToString());
            MessageBox.Show(e.RoutedEvent.RoutingStrategy.ToString());
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;
            MessageBox.Show(tb.IsChecked.ToString());
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Checked!");
            ToggleButton tb = sender as ToggleButton;
            MessageBox.Show(tb.IsChecked.ToString());
        }
        int counter;
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            tbCounter.Text = counter.ToString();
        }

        private void MoveVideoToStart(object sender, RoutedEventArgs e)
        {
            //meSpongebob.Position = new TimeSpan(0);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            spData.Resources["drColor"] = Brushes.Blue;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            spData.Resources["drColor"] = Brushes.Red;
        }
        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show($"{e.RoutedEvent.RoutingStrategy.ToString()} - Routing Strategy.");
            MessageBox.Show("Button down.");
        }


        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(MainWindow), new PropertyMetadata(200));

        private void lbPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbSelectedLb.Text = "Selected listbox item: " + (e.AddedItems[0] as Person).ToString();
        }

        private void cbSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = (e.AddedItems[0] as string);
            if (selected != "None")
            {
                CollectionView cw = (CollectionView)CollectionViewSource.GetDefaultView(dgOrganized.ItemsSource);
                cw.SortDescriptions.Clear();
                cw.SortDescriptions.Add(new System.ComponentModel.SortDescription(selected, System.ComponentModel.ListSortDirection.Ascending));
            }
        }

        private void StackPanel_Checked(object sender, RoutedEventArgs e)
        {
            string selected = (e.OriginalSource as RadioButton).Content as String;
            CollectionView cw = (CollectionView)CollectionViewSource.GetDefaultView(dgOrganized.ItemsSource);

            switch (selected)
            {
                case "Is alive":
                    cw.Filter = (o) => ((SuperPerson)o).IsAlive;
                    break;
                case "Is dead":
                    cw.Filter = (o) => !((SuperPerson)o).IsAlive;
                    break;
                default:
                    cw.Filter = o => true;
                    break;
            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            CollectionView cw = (CollectionView)CollectionViewSource.GetDefaultView(dgOrganized.ItemsSource);
            cw.GroupDescriptions.Add(new PropertyGroupDescription("IsAlive"));
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            CollectionView cw = (CollectionView)CollectionViewSource.GetDefaultView(dgOrganized.ItemsSource);
            cw.GroupDescriptions.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RandomWindow rw = new RandomWindow();
            rw.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var mbr = MessageBox.Show("Basic message box", "Caption", MessageBoxButton.YesNoCancel);
            tbMessageBoxResult.Text = "Message box result: " + mbr;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();

            if (sfd.ShowDialog() == true)
            {
                tbSaveFileResult.Text = sfd.FileName;
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Routed command's hello.");
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(5000);
            tboxFreezeThread.Text = new Random().NextDouble().ToString();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                Thread.Sleep(5000);
                tboxDontFreezeThread.Dispatcher.Invoke(() => tboxDontFreezeThread.Text = new Random().NextDouble().ToString());
            }).Start();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                tboxDontFreezeTask.Dispatcher.Invoke(() => tboxDontFreezeTask.Text = new Random().NextDouble().ToString());
            });
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            await Task.Delay(5000);
            tboxDontFreezeTask2.Dispatcher.Invoke(() => tboxDontFreezeTask2.Text = new Random().NextDouble().ToString());
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                Thread.Sleep(5000);
                tboxDontFreezeBW.Dispatcher.Invoke(() => tboxDontFreezeBW.Text = new Random().NextDouble().ToString());
            };
            bw.RunWorkerAsync();
        }

        private async void Button_Click_9(object sender, RoutedEventArgs e)
        {
            tboxRestClientContainer.Text = "Loading ...";
            HttpClient client = new HttpClient();
            try
            {

                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    tboxRestClientContainer.Text = responseText;
                }
                else
                {
                    MessageBox.Show($"Failed: {response.Headers}");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"Failed: {err.Message}");
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            var height = canvasDraw.ActualHeight;
            var width = canvasDraw.ActualWidth;
            // Drawingvisua, DrawingContext, 
            canvasDraw.Children.Add(new VisualHost(height, width));


            //Random random = new Random();
            //for (int i = 0; i < 100000; i++)
            //{
            //    var line = new Line() { X1 = random.NextDouble() * width, X2 = random.NextDouble() * width, Y1 = random.NextDouble() * height, Y2 = random.NextDouble() * height, StrokeThickness = 1, Stroke = System.Windows.Media.Brushes.Red };
            //    canvasDraw.Children.Add(line);
            //}
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            canvasDraw.Children.Clear();
        }
    }
}
