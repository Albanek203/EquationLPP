using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using EquationLPP.Infrastructure;
using EquationLPP.models;
using Button = System.Windows.Controls.Button;

namespace EquationLPP.ViewModel {
    public class MainViewModel : BaseViewModel {
        private DrawingManager _drawingManager;
        public MainViewModel() {
            SystemEquations = new SystemEquations {
                ListEquation = new ObservableCollection<Equation> { new(DrawingManager.GetRandomSolidColorBrush()) }
            };
        }
        private Canvas _canvas;
        public Canvas Canvas {
            get => _canvas;
            set {
                _canvas ??= value;
                _drawingManager ??= new DrawingManager(value);
                OnPropertyChanged(nameof(Canvas));
            }
        }
        private readonly SystemEquations _systemEquations;
        public SystemEquations SystemEquations {
            get => _systemEquations;
            init {
                _systemEquations = value;
                OnPropertyChanged(nameof(SystemEquations));
            }
        }
        private RelayCommand _addEquation;
        public ICommand AddEquation => _addEquation ??= new RelayCommand(ExecuteAddEquation);
        private void ExecuteAddEquation(object obj) {
            SystemEquations.ListEquation.Add(new Equation(DrawingManager.GetRandomSolidColorBrush()));
        }
        private RelayCommand _removeEquation;
        public ICommand RemoveEquation =>
            _removeEquation ??= new RelayCommand(ExecuteRemoveEquation, CanExecuteRemoveEquation);
        private async void ExecuteRemoveEquation(object obj) {
            SystemEquations.ListEquation.RemoveAt(SystemEquations.ListEquation.Count - 1);
            await _drawingManager.DrawLinesAsync(SystemEquations.ListEquation);
        }
        private bool CanExecuteRemoveEquation(object obj) {
            var work = SystemEquations.ListEquation.Count > 1;
            ((Button)obj).Visibility = work ? Visibility.Visible : Visibility.Collapsed;
            return true;
        }
        private RelayCommand _changeLineColor;
        public ICommand ChangeLineColor => _changeLineColor ??= new RelayCommand(ExecuteChangeLineColor);
        private async void ExecuteChangeLineColor(object obj) {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() != DialogResult.OK) return;
            var newColor =
                new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            var equationLst = SystemEquations.ListEquation.Where(x => x.Color == ((Button)obj).Background).ToList();
            if (equationLst.Count == 0) { return; }
            equationLst[0].Color = newColor;
            ((Button)obj).Background = newColor;
            await _drawingManager.DrawLinesAsync(SystemEquations.ListEquation);
        }
        private RelayCommand _showResult;
        public ICommand ShowResult => _showResult ??= new RelayCommand(ExecuteShowResult);
        private async void ExecuteShowResult(object obj) {
            await _drawingManager.DrawLinesAsync(SystemEquations.ListEquation);
            if (SystemEquations.ParseFunctionF()) {
                await _drawingManager.DrawVectorNAsync(SystemEquations.EndPointFunctionF);
                var minMaxValues = _drawingManager.CalculateMinMax(SystemEquations);
                if (!_drawingManager.IsClosedArea(SystemEquations.ListEquation)) {
                    ZMin = "-";
                    ZMax = minMaxValues[1].ToString(CultureInfo.InvariantCulture);
                }
                else {
                    ZMin = minMaxValues[0].ToString(CultureInfo.InvariantCulture);
                    ZMax = minMaxValues[1].ToString(CultureInfo.InvariantCulture);
                }
            }
            else
                await _drawingManager.RemoveVectorNAsync();
        }
        private string _zMin;
        public string ZMin {
            get => _zMin;
            set {
                _zMin = value;
                OnPropertyChanged(nameof(ZMin));
            }
        }
        private string _zMax;
        public string ZMax {
            get => _zMax;
            set {
                _zMax = value;
                OnPropertyChanged(nameof(ZMax));
            }
        }
    }
}