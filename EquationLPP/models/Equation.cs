using System;
using System.Collections.Generic;
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
        public readonly List<Point> DrawnPoints = new();
        public bool IsParsed = false;
        public bool IsFirstQuarterP1 = false;
        public bool IsFirstQuarterP2 = false;
        public Equation(SolidColorBrush color) { this.Color = color; }
        public bool ParseData() {
            Data = Data.Trim();
            var groupX1 = new Regex(@"([+-]?\d*)x1").Match(Data).Groups[1];
            var groupX2 = new Regex(@"([+-]?\d*)x2").Match(Data).Groups[1];
            var groupSign = new Regex(@"([<>]?=)").Match(Data).Groups[1];
            var groupEqual = new Regex(@"(?<=[<>]?=)[+-]?\d*").Match(Data);
            if (!groupX1.Success || !groupX2.Success || !groupSign.Success || !groupEqual.Success) {
                IsParsed = false;
                return false;
            }
            int.TryParse(groupX1.ToString(), out var tmp);
            CoefficientX1 = tmp == 0 ? 1 : tmp;
            int.TryParse(groupX2.ToString(), out tmp);
            CoefficientX2 = tmp == 0 ? 1 : tmp;
            int.TryParse(groupEqual.ToString(), out tmp);
            Equal = tmp;
            TwoPintsLines = new[] {
                new Point(0, (float)Math.Round((float)Equal / CoefficientX2, 2))
              , new Point((float)Math.Round((float)Equal / CoefficientX1, 2), 0)
            };
            switch (groupSign.ToString()) {
                case ">=": Sign = Signs.GreaterThan; break; 
                case "<=": Sign = Signs.LessThan; break;
                case "=": Sign = Signs.Equal; break;
                default: IsParsed = false; return false;
            }
            IsFirstQuarterP1 = TwoPintsLines[0].X >= 0 && TwoPintsLines[0].Y >= 0;
            IsFirstQuarterP2 = TwoPintsLines[1].X >= 0 && TwoPintsLines[1].Y >= 0;
            IsParsed = true;
            return true;
        }
        public Point? GetCrossing(Equation equation) {
            var cofX2 = -CoefficientX1 * equation.CoefficientX2 - -equation.CoefficientX1 * CoefficientX2;
            var constant = equation.CoefficientX2 * Equal - CoefficientX2 * equation.Equal;
            if (cofX2 == 0) { return null; }
            var x = (float)Math.Round((float)-constant / cofX2, 2);
            var y = (Equal - CoefficientX1 * x) / CoefficientX2;
            return new Point(x, (float)Math.Round(y, 2));
        }
        public void ClearDrawnPoints() {
            DrawnPoints.Clear();
        }
    }
}