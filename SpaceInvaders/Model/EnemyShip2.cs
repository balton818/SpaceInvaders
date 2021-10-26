using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>Defines the behavior and fields of EnemyShip2 objects</summary>
    public class EnemyShip2 : EnemyShip
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="EnemyShip2" /> class.</summary>
        public EnemyShip2()
        {
            Sprite = new Level2EnemySprite();
            SetSpeed(5, 5);
            PointValue = 5;
        }

        #endregion
    }
}