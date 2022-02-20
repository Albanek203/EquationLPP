using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EquationLPP.models {
    public class DrawingManager {
        private Canvas? _canvas;
        private Line _vectorN = new Line();
        private readonly List<Line> _listLines = new List<Line>();
        private readonly List<Polygon> _listPolygon = new List<Polygon>();
        private readonly List<Ellipse> _listPoints = new List<Ellipse>();
        public async Task DrawLinesAsync(ObservableCollection<Equation> listEquation, Canvas canvas) {
            _canvas = canvas;
            await ClearCanvasAsync();
            foreach (var elem in listEquation) {
                if (!elem.ParseData()) continue;
                var line = GetLineByPoints(new Point(elem.TwoPintsLines[0].X, elem.TwoPintsLines[0].Y)
                                         , new Point(elem.TwoPintsLines[1].X, elem.TwoPintsLines[1].Y), elem.Color);
                var polygon = new Polygon { Stroke = Brushes.Black, Opacity = 0.2, Fill = elem.Color };
                var pointsExpansion = new List<string> {
                    $"{line.X1},{line.Y1},{line.X2},{line.Y2},{line.X2 + 1000},{line.Y2 - 1000},{line.X1 + 1000},{line.Y1 - 1000}"
                  , $"{line.X1},{line.Y1},{line.X2},{line.Y2},{line.X2 + 1000},{line.Y2 + 1000},{line.X1 + 1000},{line.Y1 + 1000}"
                  , $"{line.X1},{line.Y1},{line.X2},{line.Y2},{line.X2 - 1000},{line.Y2 + 1000},{line.X1 - 1000},{line.Y1 + 1000}"
                  , $"{line.X1},{line.Y1},{line.X2},{line.Y2},{line.X2 - 1000},{line.Y2 - 1000},{line.X1 - 1000},{line.Y1 - 1000}"
                };
                _listLines.Add(line);
                _canvas!.Children.Add(line);
                if (elem.Sign == Signs.Equal) continue;
                if ((elem.CoefficientX1 > 0 && elem.CoefficientX2 < 0) ||
                    (elem.CoefficientX1 < 0 && elem.CoefficientX2 > 0)) {
                    polygon.Points = elem.CoefficientX2 switch {
                        < 0 when elem.Equal < 0 => elem.Sign == Signs.LessThan
                                                       ? PointCollection.Parse(pointsExpansion[1])
                                                       : PointCollection.Parse(pointsExpansion[3])
                      , > 0 when elem.Equal < 0 => elem.Sign == Signs.LessThan
                                                       ? PointCollection.Parse(pointsExpansion[3])
                                                       : PointCollection.Parse(pointsExpansion[1])
                      , > 0 when elem.Equal > 0 => elem.Sign == Signs.LessThan
                                                       ? PointCollection.Parse(pointsExpansion[1])
                                                       : PointCollection.Parse(pointsExpansion[3])
                      , < 0 when elem.Equal > 0 => elem.Sign == Signs.LessThan
                                                       ? PointCollection.Parse(pointsExpansion[3])
                                                       : PointCollection.Parse(pointsExpansion[1])
                      , _ => polygon.Points
                    };
                }
                else {
                    polygon.Points = elem.Sign == Signs.LessThan
                                         ? PointCollection.Parse(pointsExpansion[2])
                                         : PointCollection.Parse(pointsExpansion[0]);
                }
                _listPolygon.Add(polygon);
                _canvas.Children.Add(polygon);
                if (elem.IsFirstQuarterP1)
                    await DrawPointAsync(new Point(elem.TwoPintsLines[0].X, elem.TwoPintsLines[0].Y));
                if (elem.IsFirstQuarterP2)
                    await DrawPointAsync(new Point(elem.TwoPintsLines[1].X, elem.TwoPintsLines[1].Y));
            }
            foreach (var elem in listEquation) {
                if (!elem.IsParsed) continue;
                foreach (var item in listEquation) {
                    if (item == elem) continue;
                    var point = elem.GetCrossing(item);
                    if (point.X >= 0 && point.Y >= 0) await DrawPointAsync(point);
                }
            }
        }
        private async Task DrawPointAsync(Point point) {
            var toolTip = point.ToString();
            point = ConvertPoint(point);
            var ellipse = new Ellipse {
                Width = 10, Height = 10, Margin = new Thickness(point.X - 5, point.Y - 5, 0, 0), Fill = Brushes.Red
              , ToolTip = toolTip
            };
            if (_listPoints.Contains(ellipse)) return;
            _listPoints.Add(ellipse);
            _canvas!.Children.Add(ellipse);
        }
        public async Task DrawVectorNAsync(Point endPoint, Canvas canvas) {
            _canvas = canvas;
            endPoint = new Point((endPoint.X * 10) + 400, (-endPoint.Y * 10) + 300);
            var line = new Line {
                StrokeThickness = 4, Stroke = Brushes.Black, X1 = 400, Y1 = 300, X2 = endPoint.X, Y2 = endPoint.Y
            };
            _canvas!.Children.Add(line);
            _vectorN = line;
        }
        public async Task RemoveVectorNAsync() {
            _canvas!.Children.Remove(_vectorN);
            _vectorN = new Line();
        }
        public async Task UpdateAsync(ObservableCollection<Equation> listEquation) {
            await DrawLinesAsync(listEquation, _canvas!);
        }
        private static Point ConvertPoint(Point point) => new((point.X * 10) + 400, (-point.Y * 10) + 300);
        private static Line GetLineByPoints(Point p1, Point p2, Brush color) {
            p1 = ConvertPoint(p1);
            p2 = ConvertPoint(p2);
            var k1 = p1.X - p2.X;
            var k2 = p1.Y - p2.Y;
            p1.X += k1 * 20;
            p1.Y += k2 * 20;
            p2.X -= k1 * 20;
            p2.Y -= k2 * 20;
            return new Line { StrokeThickness = 2, Stroke = color, X1 = p1.X, Y1 = p1.Y, X2 = p2.X, Y2 = p2.Y };
        }
        private async Task ClearCanvasAsync() {
            if (_listLines.Count == 0) return;
            foreach (var element in _listLines) _canvas!.Children.Remove(element);
            foreach (var element in _listPolygon) _canvas!.Children.Remove(element);
            foreach (var element in _listPoints) _canvas!.Children.Remove(element);
        }
        public static SolidColorBrush GetRandomSolidColorBrush() {
            var random = new Random();
            return new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255)
                                                   , (byte)random.Next(0, 255)));
        }
    }
}