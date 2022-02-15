using System.Collections.ObjectModel;
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
        private readonly DrawingManager _drawingManager;
        public MainViewModel(DrawingManager drawingManager) {
            _drawingManager = drawingManager;
            SystemEquations = new SystemEquations {
                ListEquation = new ObservableCollection<Equation> { new(DrawingManager.GetRandomSolidColorBrush()) }
            };
        }
        private SystemEquations _systemEquations;
        public SystemEquations SystemEquations {
            get => _systemEquations;
            set {
                _systemEquations = value;
                OnPropertyChanged(nameof(SystemEquations));
            }
        }

        // === Add Equation Button ===
        private RelayCommand _addEquation;
        public ICommand AddEquation => _addEquation ??= new RelayCommand(ExecuteAddEquation, CanExecuteAddEquation);
        private void ExecuteAddEquation(object obj) {
            SystemEquations.ListEquation.Add(new Equation(DrawingManager.GetRandomSolidColorBrush()));
        }
        private bool CanExecuteAddEquation(object obj) => true;
        // === End ===

        // === Remove Equation ===
        private RelayCommand _removeEquation;
        public ICommand RemoveEquation =>
            _removeEquation ??= new RelayCommand(ExecuteRemoveEquation, CanExecuteRemoveEquation);
        private async void ExecuteRemoveEquation(object obj) {
            SystemEquations.ListEquation.RemoveAt(SystemEquations.ListEquation.Count - 1);
            await _drawingManager.UpdateAsync(SystemEquations.ListEquation);
        }
        private bool CanExecuteRemoveEquation(object obj) {
            var work = SystemEquations.ListEquation.Count > 1;
            ((Button)obj).Visibility = work ? Visibility.Visible : Visibility.Collapsed;
            return true;
        }
        // === End ===

        // === Show Result ===
        private RelayCommand _showResult;
        public ICommand ShowResult => _showResult ??= new RelayCommand(ExecuteShowResult, CanExecuteShowResult);
        private async void ExecuteShowResult(object obj) {
            await _drawingManager.DrawLinesAsync(SystemEquations.ListEquation, (Canvas)obj);
            if (SystemEquations.ParseFunctionF())
                await _drawingManager.DrawVectorNAsync(SystemEquations.EndPointFunctionF, (Canvas)obj);
            else
                await _drawingManager.RemoveVectorNAsync();
        }
        private bool CanExecuteShowResult(object obj) => true;
        // === End ===

        // === Change Color ===
        private RelayCommand _changeLineColor;
        public ICommand ChangeLineColor =>
            _changeLineColor ??= new RelayCommand(ExecuteChangeLineColor, CanExecuteChangeLineColor);
        private async void ExecuteChangeLineColor(object obj) {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() != DialogResult.OK) return;
            var newColor =
                new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            SystemEquations.ListEquation.Where(x => x.Color == ((Button)obj).Background).ToList()[0].Color = newColor;
            ((Button)obj).Background = newColor;
            await _drawingManager.UpdateAsync(SystemEquations.ListEquation);
        }
        private bool CanExecuteChangeLineColor(object obj) => true;
        // === End ===
    }
}