using System.ComponentModel;
using System.Formats.Tar;
using Tetris_C_;

namespace Tetris_2._0 {
    public class GameState { //aceasta clasa leaga aproape toate clasele de gameplay intre ele

        private Block currentBlock;

        public Block CurrentBlock { // fucntie care actualizeaza block-ul curent
        
            get => currentBlock;
            private set {

                currentBlock = value;
                currentBlock.Reset();

                for (int i = 0; i < 2; i++) {

                    currentBlock.Move(1, 0);

                    if (!BlockFits()) { 
                    
                        CurrentBlock.Move(-1, 0);
                    
                    }
                
                }

            }
                


        }

        public Game_Grid GameGrid { get; }
        public BlockQueue BlockQueue { get; }

        public bool GameOver { get; private set; } // tine cont de momentul cand jucatorul pierde

        public int Score { get; private set; }

        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState() {

            GameGrid = new Game_Grid(22, 10); // initializam tabla de joc cu 22 de randuri si 10 coloeana (primele 2 randuri vor si invizibile pentru jucator si in acestea vor fi generate noile blockuri)
            BlockQueue=new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        
        }

        private bool BlockFits() { // aceasta functie verifica daca block-ul generat se afla pe o pozitie valida (in interiorul tablei de joc si intr-un spatiu gol)


            foreach (Position p in CurrentBlock.TilePositions()) {

                if (!GameGrid.IsEmpty(p.Row, p.Column)) { 
                
                    return false;
                
                }
            
            }

            return true;

        }

        public void RotateBlockCW() { // functia roteste blockul in directia acelor de ceasornic

            CurrentBlock.RotateCw();

            if (!BlockFits()) { // daca dupa ce blockul este rotit, nu se afla intr-o pozitie valia, va fi rotit inapoi ajungand in pozitia initiala
            
                CurrentBlock.RotateCCw();
            
            }
        
        }

        public void RotateBlockCCW() {

            CurrentBlock.RotateCCw();

            if (!BlockFits()) {

                CurrentBlock.RotateCw();
            
            }
        
        
        }

        public void MoveBlockLeft() { // mutam blockul la stanga si verificam daca se afla intr-o pozitie valida

            CurrentBlock.Move(0, -1);

            if (!BlockFits()) {

                CurrentBlock.Move(0, 1);
            
            }
        
        }

        public int RowReturn() {

            return CurrentBlock.returnRow();
        
        }

        public int ColumnReturn() { 
        
            return CurrentBlock.returColumn();
        
        }

        public void MoveBlockRight() {

            CurrentBlock.Move(0, 1);

            if (!BlockFits()) {

                CurrentBlock.Move(0, -1);
            
            }
        
        
        }

        public void MoveBlockDown() {

            CurrentBlock.Move(1,0);

            if (!BlockFits()) {

               CurrentBlock.Move(-1, 0);
               PlaceBlock(); // daca blockul nu poate poate fi mutat in jos, il plaseaza automat pe tabla
            
            }
        
        }


        private int TiileDropDistance(Position p) { // cu aceasta metoda, vedem numarul maxim de blocuri cu care blockul poate fi mutat mai jos

            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop+1, p.Column)) {

                drop++;
            
            }

            return drop;
        
        
        
        }


        public int BlockDropDistance() {


            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions()) { 
            
                drop= Math.Min(drop, TiileDropDistance(p));
            
                
            
            }
            return drop; 

        
        }


        public void DropBlock() {

            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        
        
        
        }

        public void HoldBlock() { 
        
            if(!CanHold){

                return;

            }

            if (HeldBlock == null) {// daca nu tinem nimic in mana, blocul de acoo (care este null, zero) devine blocul curent, iar blocul curent devine urmatorul block

                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();

            } else {                        // daca tinem ceva in mana, blockurile o sa faca switch

                Block temo = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = temo;
                
            
            }

            CanHold = false; //poti sa schimbi blocul doar odata, per block

        }


        private bool IsGameOver() {

            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        
        }


        private void PlaceBlock() { // functia asta aseaza block-ul din "mana" jucatorului pe tabla. apoi sterge toate randurile pline (daca sunt) si verifica conditia de game over

            foreach (Position p in CurrentBlock.TilePositions()) {

                GameGrid[p.Row, p.Column] = CurrentBlock.id;
            
            }

            int cleared = GameGrid.ClearFullRows();

            Score += cleared*10;

            if (cleared > 1) {

                Score +=(int) Math.Pow(3,cleared);
            
            }

            

            if (IsGameOver()) {

                GameOver = true;

            } else {

                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true; // de fiecare data dupa ce pui un block jos, vei putea sa faci inca un switch cu blockul din hold
            
            }

        }

    }
}
