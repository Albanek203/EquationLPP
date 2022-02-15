using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EquationLPP.ViewModel;
using System.Windows.Shapes;

namespace EquationLPP.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow(MainViewModel viewModel) {
            InitializeComponent();
            DataContext = viewModel;
            Task.Factory.StartNew(() => {
                for (int i = 0, margin = 10; i < 79; i++, margin += 10) {
                    if (margin == 400) continue;
                    Dispatcher.Invoke(() => {
                        var line1 = new Line {
                            Stroke = Brushes.Gray, Y1 = 600, Y2 = 0, Margin = new Thickness(margin, 0, 0, 0)
                        };
                        Canvas.Children.Add(line1);
                    });
                }
                for (int i = 0, margin = 10; i < 59; i++, margin += 10) {
                    if (margin == 300) continue;
                    Dispatcher.Invoke(() => {
                        var line = new Line {
                            Stroke = Brushes.Gray, X1 = 800, Y1 = 0, Margin = new Thickness(0, margin, 0, 0)
                        };
                        Canvas.Children.Add(line);
                    });
                }
            });
            Task.Factory.StartNew(() => {
                for (int i = 0, marginT = 26, digit = 30; i < 60; i++, marginT += 10, digit--) {
                    var text = "–";
                    var marginL = 65;
                    if (digit % 5 == 0) {
                        text = $"{digit} ––";
                        marginL = digit.ToString().Length == 1 ? 38 : 28;
                        if (digit < 0) { marginL = digit == -5 ? 30 : 20; }
                    }
                    Dispatcher.Invoke(() => {
                        LeftNumbers.Children.Add(new TextBlock {
                            Text = text, Margin = new Thickness(marginL, marginT, 0, 0), FontSize = 20
                        });
                    });
                }
                for (int i = 0, marginL = 92, digit = -40; i < 80; i++, marginL += 10, digit++) {
                    var text = "–";
                    var marginT = 640;
                    Dispatcher.Invoke(() => {
                        if (digit % 5 == 0) {
                            text = $"––";
                            var k = digit.ToString().Length == 1 ? 22 : 27;
                            if (digit < 0) { k = System.Math.Abs(digit).ToString().Length == 1 ? 30 : 35; }
                            ButtonNumbers.Children.Add(new TextBlock {
                                Text = digit.ToString(), Margin = new Thickness(marginL - k, marginT + 15, 0, 0)
                              , FontSize = 20
                            });
                        }
                        ButtonNumbers.Children.Add(new TextBlock {
                            Text = text, Margin = new Thickness(marginL, marginT, 0, 0), FontSize = 20
                          , RenderTransform = new RotateTransform(90)
                        });
                    });
                }
            });
        }
    }
}