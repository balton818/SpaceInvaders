using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>Defines the behavior and fields of EnemyShip1 objects</summary>
    public class EnemyShip1 : EnemyShip
    {
        #region Data members

        private const int pointValue = 1;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="EnemyShip1" /> class.</summary>
        public EnemyShip1()
        {
            Sprite = new Level1EnemySprite();
            SetSpeed(5, 5);
            SetPoints(pointValue);
        }

        #endregion
    }
}