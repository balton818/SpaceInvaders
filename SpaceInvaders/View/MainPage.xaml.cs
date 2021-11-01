using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
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
            this.gameManager.ScoreboardUpdated += this.handleScoreUpdate;
            this.gameManager.PlayerKilled += this.handlePlayerLost;
            this.gameManager.PlayerWon += this.handlePlayerWon;
        }

        #endregion

        #region Methods

        private void handleScoreUpdate(int score)
        {
            this.scoreBoard.Text = "Score: " + score;
        }

        private void handlePlayerLost()
        {
            var output = "You were killed by the alien invaders. Game Over.";
            output += Environment.NewLine + "Score: " + this.gameManager.Score;
            this.scoreBoard.Text = output;
        }

        private void handlePlayerWon()
        {
            var output = "Congratulations! You saved the galaxy from the alien invaders!";
            output += Environment.NewLine + "Score: " + this.gameManager.Score;
            this.scoreBoard.Text = output;
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
                    this.gameManager.FireBullet();
                    break;
            }
        }

        #endregion
    }
}