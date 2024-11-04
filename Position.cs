namespace Tetris_2._0 {
    public class Position {// aceasta clasa tine minte un rand si o coloana si updateaza pozitia obiectului pe care il miscam

        public int Row { get; set; }
        public int Column { get; set; }


        public Position(int row, int column) {

            Row = row; Column = column;

        }
    }
}
