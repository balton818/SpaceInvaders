using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Ships
{
    /// <summary>Describes the behavior and attributes of AggressiveEnemyShip class objects</summary>
    public class AggressiveEnemyShip : EnemyShip
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="AggressiveEnemyShip" /> class.</summary>
        /// <param name="level">The enemy ship level.</param>
        public AggressiveEnemyShip(int level)
        {
            switch (level)
            {
                case 1:
                    Sprite = new Level3EnemyShipSprite();
                    PointValue = 10;
                    EnemyLevel = EnemyType.Level3;
                    break;
                case 2:
                    Sprite = new Level4EnemyShipSprite();
                    PointValue = 20;
                    EnemyLevel = EnemyType.Level4;
                    break;
            }

            SetSpeed(5, 5);
        }

        #endregion

        #region Methods

        /// <summary>Fires the bullet.</summary>
        /// <returns>the bullet that was fired</returns>
        public EnemyBullet FireBullet()
        {
            return new EnemyBullet();
        }

        #endregion
    }
}