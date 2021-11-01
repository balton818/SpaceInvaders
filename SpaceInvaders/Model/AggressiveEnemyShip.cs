using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    public class AggressiveEnemyShip : EnemyShip
    {
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

        public EnemyBullet FireBullet()
        {
            return new EnemyBullet();
        }
    }
}
