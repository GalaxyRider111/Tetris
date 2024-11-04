namespace Tetris_2._0 {
    public class IBlock : Block { // aceasta clasa extinde clasa de baza Block


        private readonly Position[][] tiles = new Position[][] {// salvam pozitiile (fiecarui patratel ce alcatuieste blockul) din fiecare stare de rotatie a blockului I

            new Position[]{new(1,0),new(1,1),new(1,2),new(1,3) },
            new Position[]{new(0,2),new(1,2),new(2,2),new(3,2) },
            new Position[]{new(2,0),new(2,1),new(2,2),new(2,3) },
            new Position[]{new(0,1),new(1,1),new(2,1),new(3,1) }
        };

        public override int id => 1;

        protected override Position StartOffset => new Position(-1, 3);//blockul va fi creat pe mijlocul primului rand

        protected override Position[][] Tiles => tiles; //updatam matricea grid-ului

    }
}
