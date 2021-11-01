using System.ComponentModel.Design;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    public class PassiveEnemyShip : EnemyShip
    {
        public PassiveEnemyShip(int level)
        {
            if (level == 1)
            {
                Sprite = new Level1EnemySprite();
                SetPoints(1);
                EnemyLevel = EnemyType.Level1;
            }
            else if (level == 2)
            {
                Sprite = new Level2EnemySprite();
                SetPoints(5);
                EnemyLevel = EnemyType.Level2;
            }
            SetSpeed(5,5);
        }
    }
}
