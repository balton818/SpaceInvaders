using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public class BulletManager
    {
        public IList<EnemyBullet> EnemyBullets { get; set; }

        public IList<PlayerBullet> PlayerBullets { get; set; }

        public BulletManager()
        {
            this.EnemyBullets = new List<EnemyBullet>();
            this.PlayerBullets = new List<PlayerBullet>();
        }


    }
}
