using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris_C_;

namespace Tetris_2._0 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        private readonly ImageSource[] tileImages = new ImageSource[] { // un vector ce contine toate culorile pe care le vor avea blocurile cand vor fi afisate

            new BitmapImage(new Uri("Assets/TileEmpty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png",UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[] { // imaginea bloocului intreg care va fi afisata atunci cand tinem un block si cand vedem blocul care urmeaza

            new BitmapImage(new Uri("Assets/Block-Empty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png",UriKind.Relative)),

        };


        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private GameState gameState = new GameState();

        

        public MainWindow() {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(Game_Grid grid) {

            Image[,] ImageControls = new Image[grid.Rows, grid.Columns]; // functia se va ocupe de alegerea imaginii care va fi afisata intr-un anumiit loc
            int cellSize = 25; // toate patratelele sa aibe 25 de pixeli

            for (int r = 0; r < grid.Rows; r++) {

                for (int c = 0; c < grid.Columns; c++) { // trecem prin fiecare patratel de pe fiecare linie si coloana. Fiecare patratel va avea un obiect care va alege ce imagine sa afiseze in acel patratel

                    Image imageControl = new Image {

                        Width = cellSize,
                        Height = cellSize

                    };

                    Canvas.SetTop(imageControl, (r -2) * cellSize+10); // ascundem primele 2 randuri ca sa nu le vada jucatorul (aici vor fi generate noile blocuri)
                    Canvas.SetLeft(imageControl, c * cellSize);

                    GameCanvas.Children.Add(imageControl)
                ;   ImageControls[r, c] = imageControl;
                }

            }

            return ImageControls;

        }

        private void DrawGrid(Game_Grid grid) {

            for (int r = 0; r < grid.Rows; r++) {

                for (int c = 0; c < grid.Columns; c++) {

                   /*
                    int id=grid[r,c];

                    if (r == 1 && id == 0) {

                        grid[r, c] = 6;

                    }

                    */

                        int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                        imageControls[r, c].Source = tileImages[id]; // trecem prin fiecare bloc din tabla de joc si ii alegeam imaginea in functie de id-ul lui

                    
                }
            
            
            }
        
        }


        private void DrawBlock(Block block) { // le fiecare block, ne uitam pe toate poziitle pe care le ocupa acetsa, si desenam patratele de culoarea corepunzatoare

            foreach (Position p in block.TilePositions()) {


                imageControls[p.Row, p.Column].Opacity = 1;

                imageControls[p.Row, p.Column].Source = tileImages[block.id];
            
            }
        
        }


        private void DrawNextBlock(BlockQueue blockQueue) {

            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.id];
        
        }

        private void DrawHeldBlock(Block heldBlock) {

            if (heldBlock == null) {

                HoldImage.Source = blockImages[0]; // daca e gol afiseaza o poza  goala
            
            }else{

                HoldImage.Source = blockImages[heldBlock.id];// daca e ceva acolo, afiseaza poza blocului respectiv
            
            }
        
        }

        private void DrawGhostBlock(Block block) {

            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePositions()) {

                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.id];

            }
        
        
        }

        private void Draw(GameState gameState) {// o singura functie care deseneazaintreaga tabla de joc

            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Scorul: {gameState.Score}";
        
        }

        private async Task GammeLoop() {

            Draw(gameState);

            while (!gameState.GameOver) {

                int delay = Math.Max(minDelay, maxDelay - (gameState.Score / 10 * delayDecrease));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            
            }
        
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Felicitări, scorul tău final este de: {gameState.Score} puncte";
        }


        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e) {

            await GammeLoop();

        }

        private void Window_KeyDown(object sender, KeyEventArgs e) { // in aceasta functie luam input de la tastatura

            if (gameState.GameOver) { // daca jocul s-a terminat, nu se accepta nicio comanda

                return;
            
            }

            switch (e.Key) {

                case Key.Left:
                    gameState.MoveBlockLeft();
                    Debug.WriteLine("LEFT");
                    break;

                case Key.Right:
                    gameState.MoveBlockRight();
                    Debug.WriteLine("RIGHT");

                    break;
                case Key.Down: 
                    gameState.MoveBlockDown();
                    Debug.WriteLine("DOWN");

                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    Debug.WriteLine("ROTATE CCW");

                    break;
                case Key.X:
                    gameState.RotateBlockCW();
                    Debug.WriteLine("ROTAETE CW");

                    break;

                case Key.C:
                    gameState.HoldBlock();
                    Debug.WriteLine("Hold Block");
                    break;

                case Key.Space:
                    gameState.DropBlock();
                    Debug.WriteLine("Drop Block");

                    break;


                default:
                    return;
            }

            Debug.WriteLine("Pozitie block Row:", gameState.RowReturn());
            Debug.WriteLine("Pozitie block Column: ", gameState.ColumnReturn());

            Draw(gameState); // dupa ce luam comanda, mai desenam odata tabla de joc cu noua pozitia a blocului

        }

        private async void Button_Click(object sender, RoutedEventArgs e) {

            gameState=new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GammeLoop();


        }

       
    }
}