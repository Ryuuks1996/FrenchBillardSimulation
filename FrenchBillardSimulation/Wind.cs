using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenchBillardSimulation
{
    public class Wind
    {
        public Texture2D texture;
        public Vector2 position, origin;
        public float angle, intensity, totalTime;
        public bool isRotationTime, isIntensityTime;
        public Wind(Texture2D _texture, Vector2 _position, float _angle)
        {
            texture = _texture;
            position = _position;
            angle = _angle;
            intensity = 2f;
            totalTime = 0f;
            isRotationTime = false;
            isIntensityTime = false;
        }

        public void Update(GameTime gameTime)
        {
            float elapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);
            totalTime += elapse;

            //Wind Rotation flag
            if(totalTime >= 5f)
            {
                isRotationTime = true;
                isIntensityTime = true;
            }
            else
            {
                isRotationTime = false;
                isIntensityTime = false;
            }

            if(isRotationTime && isIntensityTime)
            {
                angle = MathHelper.ToRadians(randomAngle());
                intensity = randomIntensity();
                totalTime = 0f;
            }

            Console.WriteLine(intensity);

        }

        public float randomIntensity()
        {
            Random random = new Random();
            float randomIntensity = (float)(random.NextDouble() + 1.5f);
            return randomIntensity;
            
        }

        public float randomAngle()
        {
            Random random = new Random();
            int randomAngle = random.Next(0, 360);
            return randomAngle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, angle, origin, intensity, SpriteEffects.None, 0f);
        }
    }
}
