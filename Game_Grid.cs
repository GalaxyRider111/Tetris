using System;

namespace Tetris_C_ {
    public class Game_Grid {

        private readonly int[,] grid; // spatiul de joc, care va avea 20 de randuri (+2 invizibile in care se vor genera piesele) si 10 coloane

        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c] { //functie prin care accesam un patrat din grid

            get => grid[r, c];
            set => grid[r, c] = value;

        }

        public Game_Grid(int rows, int columns) { // fuctnie care construieste tabla de joc. Se poate creea orice fel de tabla, cu un numar neconventional de linii si coloane

            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];

        }

        public bool IsInside(int r, int c) { //functie care verifica daca un patrat este sau nu parte din tabla de joc

            return r >= 0 && r < Rows && c >= 0 && c < Columns;

        }

        public bool IsEmpty(int r, int c) {  // functie care verifica daca patratelul este gol sau nu

            return IsInside(r, c) && grid[r, c] == 0;

        }

        public bool IsRowFull(int r) { // verifica daca un aumit rand este plin sau nu

            for (int c = 0; c < Columns; c++) {

                if (grid[r, c] == 0)
                    return false;

            }

            return true;

        }

        public bool IsRowEmpty(int r) { // verifica daca randul este gol

            for (int c = 0; c < Columns; c++) {

                if (grid[r, c] != 0)
                    return false;
            }
            return true;

        }

        public void ClearRow(int r) { // golim o linie intreaga

            for (int c = 0; c < Columns; c++) {

                grid[r, c] = 0;

            }

        }

        public void MoveRowDown(int r, int numRows) { //dupa ce liniile pline sunt golite, mutem toate linnile celelalte mai jos cu un numar de spatii proportional cu numarul de linii de sub acestea golite

            for (int c = 0; c < Columns; c++) {

                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;

            }

        }

        public int ClearFullRows() {// functie care goleste randurile pline si le muta mai jos pe cele care nu sunt pline

            int cleared = 0;//numarul de randuri sterse

            for (int r = Rows - 1; r >= 0; r--) {

                if (IsRowFull(r)) { // verificam daca randurl e plin, si daca da, il stergem

                    cleared++;
                    ClearRow(r);

                } else if (cleared > 0) {

                    MoveRowDown(r, cleared);// mutam toate randurile care nu sunt pline mai jos, luand in considerare numarul de randuri sterse pana atunci

                }

            }

            return cleared;

        }





    }
}
