using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> solutions = new List<string>();
        List<string> symbols = new List<string>();
        List<int> indexes = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            Create();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            char number = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
            if (Convert.ToInt32(number) > 95 && Convert.ToInt32(number) < 106)
            {
                e.Handled = false;
            }
        }

        private void num_Click(object sender, RoutedEventArgs e)
        {
            string num = (sender as Button).Content.ToString();
            (solution).Text += num;

        }

        private void sym_Click(object sender, RoutedEventArgs e)
        {
            string resultTB = result.Text;
            for (int i = 0; i < resultTB.Length; i++)
            {
                if (resultTB[i] == '=')
                {
                    result.Text = "";
                }
            }
            string symbol = (sender as Button).Content.ToString();
            string text = (solution).Text;
            if (text == "")
            {
                solution.Text = "0";
            }
            text = (solution as TextBlock).Text;
            solution.Text = "";
            result.Text += text + " " + symbol + " ";
            if (symbol != "=")
            {
                solutions.Add(text);
                symbols.Add(symbol);
            }
            else
            {
                solutions.Add(text);
                double res = 0;
                for (int i = 0; i < symbols.Count; i++)
                {
                    if (symbols[i] == "/" || symbols[i] == "*")
                    {
                        indexes.Add(i);
                    }
                }
                int index = 0;
                for (int i = 0; i < solutions.Count; i++)
                {
                    if (indexes.Count != 0 && indexes.Count != index && i == indexes[index])
                    {
                        try
                        {
                            res = double.Parse(solutions[i]);
                            switch (symbols[indexes[index]])
                            {
                                case "/":
                                    res /= double.Parse(solutions[i + 1]);
                                    break;
                                case "*":
                                    res *= double.Parse(solutions[i + 1]);
                                    break;
                            }
                            solutions[i] = res.ToString();
                            solutions.RemoveAt(i + 1);
                        }
                        catch
                        {
                            res = double.Parse(solutions[i - 1]);
                            switch (symbols[indexes[index]])
                            {
                                case "/":
                                    res /= double.Parse(solutions[i]);
                                    break;
                                case "*":
                                    res *= double.Parse(solutions[i]);
                                    break;
                            }
                            solutions[i - 1] = res.ToString();
                            solutions.RemoveAt(i);
                        }
                        index++;
                    }
                }
                for (int i = 0; i < symbols.Count; i++)
                {
                    if (symbols[i] == "*" || symbols[i] == "/")
                        symbols.RemoveAt(i);
                }
                int k = 0;
                double finalResult = double.Parse(solutions[0]);
                for (int i = 0; i < solutions.Count; i++)
                {
                    if (i != solutions.Count - 1)
                    {
                        switch (symbols[k])
                        {
                            case "+":
                                finalResult += double.Parse(solutions[i + 1]);
                                break;
                            case "-":
                                finalResult -= double.Parse(solutions[i + 1]);
                                break;
                        }
                        k++;
                    }
                }
                result.Text += finalResult;
                solutions.Clear();
                symbols.Clear();
                indexes.Clear();
            }
        }
        void Create()
        {
            List<int> nums = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            List<string> syms = new List<string>() { "+", "-", "*", "/", "=" };
            Grid grid = new Grid();
            int num = 0, sym = 0;
            Button button = new Button();
            for (int row = 0; row < 5; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60) });
                for (int col = 0; col < 4; col++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
                    if (col == 3)
                    {
                        button = new Button() { Content = syms[sym], Width = 50, Height = 50 };
                        button.Click += sym_Click;
                        sym++;
                        Grid.SetRow(button, row);
                        Grid.SetColumn(button, col);
                        grid.Children.Add(button);
                    }
                    else
                    {
                        if (num < 9)
                        {
                            button = new Button() { Content = nums[num].ToString(), Width = 50, Height = 50 };
                            button.Click += num_Click;
                            num++;
                            Grid.SetRow(button, row);
                            Grid.SetColumn(button, col);
                            grid.Children.Add(button);
                        }
                        else
                        {
                            if (row == 3 && col == 1)
                            {
                                button = new Button() { Content = nums[num].ToString(), Width = 50, Height = 50 };
                                button.Click += num_Click;
                                Grid.SetRow(button, row);
                                Grid.SetColumn(button, col);
                                grid.Children.Add(button);
                            }
                        }
                    }
                }
            }
            calc.Children.Add(grid);
        }
    }
}
