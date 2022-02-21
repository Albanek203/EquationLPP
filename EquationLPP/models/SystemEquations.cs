using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace EquationLPP.models {
    public class SystemEquations {
        public ObservableCollection<Equation> ListEquation { get; set; }
        public string FunctionF { get; set; }
        public Point EndPointFunctionF { get; set; }
        public int CoefficientX1 { get; set; }
        public int CoefficientX2 { get; set; }
        public int CoefficientC { get; set; }
        public bool ParseFunctionF() {
            if (string.IsNullOrWhiteSpace(FunctionF)) return false;
            var groupX1 = new Regex(@"([+-]?\d*)x1").Match(FunctionF).Groups[1];
            var groupX2 = new Regex(@"([+-]?\d*)x2").Match(FunctionF).Groups[1];
            var groupC = new Regex(@"x2([+-]?\d*)").Match(FunctionF).Groups[1];
            if (!groupX1.Success || !groupX2.Success || !groupC.Success) { return false; }
            int.TryParse(groupX1.ToString(), out var tmp);
            CoefficientX1 = tmp == 0 ? 1 : tmp;
            int.TryParse(groupX2.ToString(), out tmp);
            CoefficientX2 = tmp == 0 ? 1 : tmp;
            int.TryParse(groupC.ToString(), out tmp);
            CoefficientC = tmp == 0 ? 1 : tmp;
            EndPointFunctionF = new Point(CoefficientX1, CoefficientX2);
            return true;
        }
    }
}