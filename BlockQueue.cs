namespace Tetris_2._0 {
    public class BlockQueue { // aceasta clasa are rolul sa aleaga ce block sa fie introdus in joc

        private readonly Block[] blocks = new Block[] { // face un vector de tip Block care contine intante penru toate tipurile de blockuri pe care le vom folos in joc

            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()


        };

        private readonly Random random= new Random();// oridnea blocurilor in care vor fi introduse in joc va fi aleatorie (la intamplare)


        public Block NextBlock { get; private set; } // aici se va salva blockul ce urmeaza, ca jucatorul sa poate vizualiza ce il asteapta, ca in orice joc de tetris normal


      

        private Block RandomBlock() {

            return blocks[random.Next(blocks.Length)]; // alegerea la intamplarea a urmatorului block
        
        }

        public BlockQueue() {

            NextBlock = RandomBlock(); // functie in care se salveaza blockul urmator care ii va permite jcatorului sa il vizualizeze

        }

        public Block GetAndUpdate() { // aceasta functie se asigura ca acelasi block nu va fi ales de 2 sau mai multe ori consecutiv

            Block block = NextBlock;

            do {

                NextBlock = RandomBlock();

            } while (block.id==NextBlock.id);

            return block;
        
        
        }




    }
}
