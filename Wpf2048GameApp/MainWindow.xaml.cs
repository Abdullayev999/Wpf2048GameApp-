using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Wpf2048GameApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static readonly Color COLOR_EMPTY = Color.FromRgb(204, 192, 179);
        private static readonly Color COLOR_2 = Color.FromRgb(238, 228, 218);
        private static readonly Color COLOR_4 = Color.FromRgb(237, 224, 200);
        private static readonly Color COLOR_8 = Color.FromRgb(242, 177, 121);
        private static readonly Color COLOR_16 = Color.FromRgb(245, 149, 99);
        private static readonly Color COLOR_32 = Color.FromRgb(246, 124, 95);
        private static readonly Color COLOR_64 = Color.FromRgb(246, 94, 59);
        private static readonly Color COLOR_128 = Color.FromRgb(237, 207, 114);
        private static readonly Color COLOR_256 = Color.FromRgb(237, 204, 97);
        private static readonly Color COLOR_512 = Color.FromRgb(237, 200, 80);
        private static readonly Color COLOR_1024 = Color.FromRgb(237, 197, 63);
        private static readonly Color COLOR_2048 = Color.FromRgb(237, 194, 46);

        private int score;
        private int best;

        public int Score { get => score; set => OnChanged(out score, value); }
        public int Best { get => best;  set => OnChanged(out best, value); }
        public int Size { get; set; }

        public List<Label> Labels;

        private readonly string path = "Best.txt";

        public MainWindow(int size)
        {
            
            InitializeComponent();
            DataContext = this;
            Size = size;
            uniFormGridBoard.Rows = Size;
            uniFormGridBoard.Columns = Size;
            
            Labels = new List<Label>();
            InitializeBoard();

            GenerationNum();
            Best = 0;
            Score = 0;

            try
            {
                if (System.IO.File.Exists(path))
                {
                    int.TryParse(System.IO.File.ReadAllText(path), out int result);
                    Best = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
        }


        public void InitializeBoard()
        {         
            for (int i = 0; i < Size * Size; i++)
            {
                var background = COLOR_EMPTY;
                var margin = new Thickness(2,2,0,0);
                var fontSize = 20;

                Labels.Add(new Label()
                {
                    Background = new SolidColorBrush(background),
                    Margin = margin,
                    FontSize = fontSize,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = " ",
                });
                uniFormGridBoard.Children.Add(Labels[i]);
            }
            Refresh();
        }


        public void Refresh()
        {
            uniFormGridBoard.Children.Clear();
            for (int i = 0; i < Size * Size; i++)
            {
                Labels[i].Background = new SolidColorBrush(ColorFill(Labels[i].Content.ToString()));
                uniFormGridBoard.Children.Add(Labels[i]);
            }
        }

        public Color ColorFill(string content)
        {
            var color = content switch
            {
                "2" => COLOR_2,
                "4" => COLOR_4,
                "8" => COLOR_8,
                "16" => COLOR_16,
                "32" => COLOR_32,
                "64" => COLOR_64,
                "128" => COLOR_128,
                "256" => COLOR_256,
                "512" => COLOR_512,
                "1028" => COLOR_1024,
                "2048" => COLOR_2048,
                _ => COLOR_EMPTY,
            };
            return color;
        }

        public void RightSwipe()
        {
            int num = 0;
            for (int a = 0; a < Size; a++)
            {
                int start = 0 + (a * Size);
                int end = Size + (a * Size) - 1;
                for (int i = start; i < end; i++)
                {
                    if (string.IsNullOrWhiteSpace(Labels[i].Content.ToString()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(Labels[i + 1].Content.ToString()))
                    {
                        var tmp = Labels[i];
                        Labels[i] = Labels[i + 1];
                        Labels[i + 1] = tmp;
                    }
                    else if (Labels[i].Content.ToString().Equals(Labels[i + 1].Content.ToString()))
                    {
                        if (!num.ToString().Equals(Labels[i].Content.ToString()))
                        {
                            Labels[i].Content = " ";
                            num = Convert.ToInt32(Labels[i + 1].Content.ToString());
                            num *= 2;
                            Score += num;
                            Labels[i + 1].Content = num.ToString();
                        }
                        else
                        {
                            num = 0;
                            i++;
                        }
                    }
                }
            }
                RightStep();    
        }

        public void RightStep()
        {
            for (int a = 0; a < Size; a++)
            {
                for (int i = 0 + (a * Size); i < Size + (a * Size) - 1; i++)
                {
                    if (string.IsNullOrWhiteSpace(Labels[i].Content.ToString()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(Labels[i + 1].Content.ToString()))
                    {
                        var tmp = Labels[i];
                        Labels[i] = Labels[i + 1];
                        Labels[i + 1] = tmp;
                        continue;
                    }
                }
            }
        }

        public void LeftSwipe()
        {
            int num = 0;
            for (int a = Size - 1; a >= 0; a--)
            {
                for (int i = (a * Size) + Size - 1; i > a * Size; i--)
                {
                    if (string.IsNullOrWhiteSpace(Labels[i].Content.ToString()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(Labels[i - 1].Content.ToString()))
                    {
                        var tmp = Labels[i];
                        Labels[i] = Labels[i - 1];
                        Labels[i - 1] = tmp;
                    }
                    else if (Labels[i].Content.ToString().Equals(Labels[i - 1].Content.ToString()))
                    {
                        if (!num.ToString().Equals(Labels[i].Content.ToString()))
                        {
                            Labels[i].Content = " ";
                            num = Convert.ToInt32(Labels[i - 1].Content.ToString());
                            num *= 2;
                            Score += num;
                            Labels[i - 1].Content = num.ToString();
                            
                        }
                        else
                        {
                            num = 0;
                            i--;
                        }
                    }
                }
            }
            Leftstep();
        }

        public void Leftstep()
        {
            for (int a = Size - 1; a >= 0; a--)
            {
                for (int i = (a * Size) + Size - 1; i > a * Size; i--)
                {
                    if (string.IsNullOrWhiteSpace(Labels[i].Content.ToString()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(Labels[i - 1].Content.ToString()))
                    {
                        var tmp = Labels[i];
                        Labels[i] = Labels[i - 1];
                        Labels[i - 1] = tmp;
                        continue;
                    }
                }
            }
        }


        public bool IsFinish()
        {
            foreach (var item in Labels)
            {
                if (string.IsNullOrWhiteSpace(item.Content.ToString()))
                {
                    return false;
                }
            }
            return true;
        }


        public void DownSwipe()
        {
            int num = 0;
            for (int a = 0; a < Size; a++)
            {
                for (int i = a; i < a + Size * Size- Size * 2 + 1; i += Size)
                {
                    if (string.IsNullOrWhiteSpace(Labels[i].Content.ToString()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(Labels[i + Size].Content.ToString()))
                    {
                        var tmp = Labels[i];
                        Labels[i] = Labels[i + Size];
                        Labels[i + Size] = tmp;
                    }
                    else if (Labels[i].Content.ToString().Equals(Labels[i + Size].Content.ToString()))
                    {
                        if (!num.ToString().Equals(Labels[i].Content.ToString()))
                        {
                            Labels[i].Content = " ";
                            num = Convert.ToInt32(Labels[i + Size].Content.ToString());
                            num *= 2;
                            Score += num;
                            Labels[i + Size].Content = num.ToString();                            
                        }
                        else
                        {
                            num = 0;
                            i--;
                        }          
                    }
                }
            }
            DownStep();
        }

        public void DownStep()
        {
            for (int a = 0; a < Size; a++)
            {
                for (int i = a; i <  Size * Size - Size * 2; i += Size)
                {
                    if (string.IsNullOrWhiteSpace(Labels[i].Content.ToString()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(Labels[i + Size].Content.ToString()))
                    {
                        var tmp = Labels[i];
                        Labels[i] = Labels[i + Size];
                        Labels[i + Size] = tmp;
                        continue;
                    }
                }
            }
        }

        public void UpSwipe()
        {
            int num = 0;
            for (int a = 0; a < Size; a++)
            {
                for (int i = Size * Size-1 - a; i >= Size * 2-1 - a; i -= Size)
                {
                    if (string.IsNullOrWhiteSpace(Labels[i].Content.ToString()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(Labels[i - Size].Content.ToString()))
                    {
                        var tmp = Labels[i];
                        Labels[i] = Labels[i - Size];
                        Labels[i - Size] = tmp;
                    }
                    else if (Labels[i].Content.ToString().Equals(Labels[i - Size].Content.ToString()))
                    {
                        if (!num.ToString().Equals(Labels[i].Content.ToString()))
                        {
                            Labels[i].Content = " ";
                            num = Convert.ToInt32(Labels[i - Size].Content.ToString());
                            num *= 2;
                            Score += num;
                            Labels[i - Size].Content = num.ToString();
                        }
                        else
                        {
                            num = 0;
                            i++;
                        }             
                    }
                }
            }

           UpStep();
        }

        public void UpStep()
        {
            for (int a = 0; a < Size; a++)
            {
                for (int i = Size * Size-1 - a; i >= Size * 2 - 1 - a; i -= Size)
                {
                    if (string.IsNullOrWhiteSpace(Labels[i].Content.ToString()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(Labels[i - Size].Content.ToString()))
                    {
                        var tmp = Labels[i];
                        Labels[i] = Labels[i - Size];
                        Labels[i - Size] = tmp;
                        continue;
                    }
                }
            }

        }

        public void GenerationLabel()
        {
            for (int i = 0; i < Size*Size; i++)
            {
                var background = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                var margin = new Thickness(2,2,0,0);
                var fontSize = 20;

                Labels.Add(new Label()
                {
                    Background = background,
                    Margin = margin,
                    FontSize = fontSize,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = " "
                });
                uniFormGridBoard.Children.Add(Labels[i]);
            }
        }

        private void GenerationNum()
        {
            Random random = new Random();

            do
            {
                int index = random.Next(0, Labels.Count);
                int num = random.Next(0, 101);

                if (string.IsNullOrWhiteSpace(Labels[index].Content.ToString()))
                {
                    if (num >= 90)
                    {
                        Labels[index].Content = 4;
                        Labels[index].Background = new SolidColorBrush(ColorFill(Labels[index].Content.ToString()));
                    }
                    else
                    {
                        Labels[index].Content = 2;
                        Labels[index].Background = new SolidColorBrush(ColorFill(Labels[index].Content.ToString()));
                    }
                    return;
                }
            } while (true);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                UpSwipe();
            }
            else if (e.Key == Key.Down)
            {
                DownSwipe();
            }
            else if (e.Key == Key.Left)
            {
                LeftSwipe();
            }
            else if (e.Key == Key.Right)
            {
                RightSwipe();
            }

            Refresh();

            if (IsFinish())
            {
                MessageBox.Show("Game Over","2048",MessageBoxButton.OK,MessageBoxImage.Information);
                if (Best < Score)
                {
                    Best = score;
                    try
                    {
                        System.IO.File.WriteAllText(path, Best.ToString());
                    }
                    catch (Exception ex) { }
                }
                Close();
            }
            GenerationNum();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (Best < Score)
            {
                Best = score;
                try
                {
                    System.IO.File.WriteAllText(path, Best.ToString());
                }
                catch (Exception ex) { }
            }
            Score = 0;
            Labels.Clear();
            InitializeBoard();
            GenerationNum();
        }

        protected void OnChanged<T>(out T prop, T value, [CallerMemberName] string propName = "")
        {
            prop = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
