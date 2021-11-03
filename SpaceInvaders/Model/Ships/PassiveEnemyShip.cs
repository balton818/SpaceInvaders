using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Ships
{
    /// <summary>Defines the behavior and attributes of PassiveEnemyShip class objects</summary>
    public class PassiveEnemyShip : EnemyShip
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="PassiveEnemyShip" /> class.</summary>
        /// <param name="level">The level.</param>
        public PassiveEnemyShip(int level)
        {
            switch (level)
            {
                case 1:
                    Sprite = new Level1EnemySprite();
                    SetPoints(1);
                    EnemyLevel = EnemyType.Level1;
                    break;
                case 2:
                    Sprite = new Level2EnemySprite();
                    SetPoints(5);
                    EnemyLevel = EnemyType.Level2;
                    break;
            }

            SetSpeed(5, 5);
        }

        #endregion
    }
}