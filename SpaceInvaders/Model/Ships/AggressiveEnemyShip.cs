using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Ships
{

    /// <summary>Describes the behavior and attributes of AggressiveEnemyShip class objects</summary>
    public class AggressiveEnemyShip : EnemyShip
    {

        /// <summary>Initializes a new instance of the <see cref="AggressiveEnemyShip" /> class.</summary>
        /// <param name="level">The enemy ship level.</param>
        public AggressiveEnemyShip(int level)
        {
            if (level == 1)
            {
                Sprite = new Level3EnemyShipSprite();
                PointValue = 10;
                EnemyLevel = EnemyType.Level3;
            }
            else if (level == 2)
            {
                Sprite = new Level4EnemyShipSprite();
                PointValue = 20;
                EnemyLevel = EnemyType.Level4;
            }
            SetSpeed(5,5);
        }


        /// <summary>Fires the bullet.</summary>
        /// <returns>the bullet that was fired</returns>
        public EnemyBullet FireBullet()
        {
            return new EnemyBullet();
        }
    }
}
