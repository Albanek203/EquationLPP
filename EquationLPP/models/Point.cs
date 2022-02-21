namespace EquationLPP.models {
    public class Point {
        public float X { get; set; }
        public float Y { get; set; }
        public Point(float x, float y) {
            this.X = x;
            this.Y = y;
        }
        public override string ToString() => $"({this.X} ; {this.Y})";
    }
}