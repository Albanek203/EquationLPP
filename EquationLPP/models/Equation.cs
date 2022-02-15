using System.Text.RegularExpressions;
using System.Windows.Media;

namespace EquationLPP.models {
    public class Equation {
        public int CoefficientX1 { get; set; }
        public int CoefficientX2 { get; set; }
        public Signs Sign { get; set; }
        public int Equal { get; set; }
        public string Data { get; set; } = "";
        public Brush Color { get; set; }
        public Point[] TwoPintsLines = new Point[2];
        public string StringPoints { get; set; } = "hello";
        public Equation(SolidColorBrush color) { this.Color = color; }
        public bool ParseData() {
            Data = Data.Trim();
            var groupX1 = new Regex(@"([+-]?\d*)x1").Match(Data).Groups[1];
            var groupX2 = new Regex(@"([+-]?\d*)x2").Match(Data).Groups[1];
            var groupSign = new Regex(@"([<>]?=)").Match(Data).Groups[1];
            var groupEqual = new Regex(@"(?<=[<>]?=)[+-]?\d*").Match(Data);
            if (!groupX1.Success || !groupX2.Success || !groupSign.Success || !groupEqual.Success) { return false; }
            int.TryParse(groupX1.ToString(), out var tmp);
            CoefficientX1 = tmp == 0 ? 1 : tmp;
            int.TryParse(groupX2.ToString(), out tmp);
            CoefficientX2 = tmp == 0 ? 1 : tmp;
            int.TryParse(groupEqual.ToString(), out tmp);
            Equal = tmp;
            TwoPintsLines = new[]
                { new Point(0, (float)Equal / CoefficientX2), new Point((float)Equal / CoefficientX1, 0) };
            Sign = groupSign.ToString() switch {
                ">=" => Signs.GreaterThan
              , "<=" => Signs.LessThan
              , "="  => Signs.Equal
              , _    => Sign
            };
            return true;
        }
    }
}