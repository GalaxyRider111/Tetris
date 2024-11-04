using System.Collections.Generic;

namespace Tetris_2._0 {
    public abstract class Block {

        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; } //offset inseamna cat de mult e distantata blocul de tetris de pozitia (0,0) a grid-ului
        public abstract int id { get; }// fiecare tip de block, va avea un alt id pentru a putea fi identificate mai usor

        private int rotationState; // fiecare block va avea 4 stari de rotire: 0 (start),90,180,270  grade
        private Position offset;//offset-ul actual al block-ului

        public Block() {

            offset = new Position(StartOffset.Row, StartOffset.Column); 

        }

        public IEnumerable<Position> TilePositions() { // functie tine cont de cat spatiu ocupa block-ul in grid si unde este acesta (offset-ul)

            foreach (Position p in Tiles[rotationState]) {

                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);

            }

        }

        public void RotateCw() {// functia  aceasta roteste block-ul 90 de grade in directia acelor de ceas, si trece prin starile de rotatie respective. Cand ajunge la ultima, se intoarce inapoi la 0 (prima)

            rotationState = (rotationState + 1) % Tiles.Length;

        }

        public void RotateCCw() {// invarte blocul in directia opusa acelor de ceas

            if (rotationState == 0) {

                rotationState = Tiles.Length - 1;


            } else {

                rotationState--;
            }

        }

        public void Move(int rows, int columns) {// muta blocul intr-o directie cu un numar specific de randuri si coloane

            offset.Row += rows;
            offset.Column += columns;


        }

        public void Reset() {// reseteaza la valorile initiale

            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;

        }

        public int returnRow() {

            return offset.Row;
        
        }

        public int returColumn() {

            return offset.Column;
        
        }



    }
}
