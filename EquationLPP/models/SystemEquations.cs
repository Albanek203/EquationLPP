using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace EquationLPP.models {
    public class SystemEquations {
        public ObservableCollection<Equation> ListEquation { get; set; }
        public string FunctionF { get; set; }
        public Point EndPointFunctionF { get; set; }
        public bool ParseFunctionF() {
            if (string.IsNullOrWhiteSpace(FunctionF)) return false;
            var groupX1 = new Regex(@"([+-]?\d*)x1").Match(FunctionF).Groups[1];
            var groupX2 = new Regex(@"([+-]?\d*)x2").Match(FunctionF).Groups[1];
            var groupC = new Regex(@"x2([+-]?\d*)").Match(FunctionF).Groups[1];
            if (!groupX1.Success || !groupX2.Success || !groupC.Success) { return false; }
            var tmp = int.Parse(groupX1.ToString());
            var coefficientX1 = tmp == 0 ? 1 : tmp;
            tmp = int.Parse(groupX2.ToString());
            var coefficientX2 = tmp == 0 ? 1 : tmp;
            EndPointFunctionF = new Point(coefficientX1, coefficientX2);
            return true;
        }
    }
}