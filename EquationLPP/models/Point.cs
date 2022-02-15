namespace EquationLPP.models {
    public class Point {
        public float X { get; set; }
        public float Y { get; set; }
        public string StringPoint { get; set; }
        public Point(float x, float y) {
            this.X = x;
            this.Y = y;
            StringPoint = $"({this.X};{this.Y})";
        }
        public override string ToString() => $"({this.X};{this.Y})";
    }
}