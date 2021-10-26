using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpaceInvaders.View
{
    /// <summary>
    ///     The main page for the game.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const double ApplicationHeight = 480;

        /// <summary>
        ///     The application width
        /// </summary>
        public const double ApplicationWidth = 640;

        private readonly DispatcherTimer timer;

        private readonly GameManager gameManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;

            this.gameManager = new GameManager(ApplicationHeight, ApplicationWidth);
            this.gameManager.InitializeGame(this.theCanvas);

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            this.timer.Start();
        }

        #endregion

        #region Methods

        private void timerTick(object sender, object e)
        {
            this.moveEnemies();
            this.moveBullets();

            if (this.gameManager.PlayerBullet != null)
            {
                var bullet = this.gameManager.PlayerBullet;
                this.removeDeadEnemyAndBulletFromScreen(bullet);
                this.updateScore();
            }

            this.gameManager.EnemiesShoot(this.theCanvas);
            this.moveEnemyBullets();

            this.checkIfGameOver();
        }

        private void moveEnemyBullets()
        {
            this.gameManager.MoveAllEnemyBullets();
        }

        private void checkIfGameOver()
        {

            
            if (this.gameManager.DetermineIfPlayerWasHit())
            {
                this.timer.Stop();
                this.showGameOverAndClearSprites();
            }
        }
        
        private void showGameOverAndClearSprites()
        {
            this.theCanvas.Children.Clear();
            var gameOverTextBlock = new TextBlock();
            var output = "You were killed by the alien invaders. Game Over."
                         + Environment.NewLine + "Score: " + this.gameManager.Score;
            gameOverTextBlock.Text = output;
            this.theCanvas.Children.Add(gameOverTextBlock);
        }

        private void removeDeadEnemyAndBulletFromScreen(PlayerBullet bullet)
        {
            var enemyHit = this.gameManager.DetectPlayerHitsAndIncreaseScore();
            if (enemyHit != null)
            {
                this.theCanvas.Children.Remove(enemyHit.Sprite);
                this.removeBulletFromScreen(bullet);
            }
        }

        private void removeBulletFromScreen(PlayerBullet bullet)
        {
            this.gameManager.PlayerBullet = null;
            this.theCanvas.Children.Remove(bullet.Sprite);
        }

        private void moveBullets()
        {
            this.gameManager.MoveAllPlayerBullets();
        }

        private void moveEnemies()
        {
            this.gameManager.MoveAllEnemies();
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.gameManager.MovePlayerShipLeft();
                    break;
                case VirtualKey.Right:
                    this.gameManager.MovePlayerShipRight();
                    break;
                case VirtualKey.Space:
                    this.gameManager.FireBullet(this.theCanvas);
                    break;
            }
        }

        private void updateScore()
        {
            if (this.gameManager.Enemies.Count > 0)
            {
                this.scoreBoard.Text = "Score: " + this.gameManager.Score;
            }
            else
            {
                this.timer.Stop();
                this.showPlayerWon();
            }
        }

        private void showPlayerWon()
        {
            var output = "Congratulations! You saved the galaxy!";
            output += Environment.NewLine + "Score: " + this.gameManager.Score;
            this.scoreBoard.Text = output;
        }

        #endregion
    }
}