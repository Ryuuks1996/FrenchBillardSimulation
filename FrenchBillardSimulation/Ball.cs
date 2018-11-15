using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenchBillardSimulation
{
    public class Ball
    {
        public Texture2D ballTexture;
        public Vector2 position, initialPosition, velocity;
        public float radius, shootingAngle;

        public float mass, initialVelocity,uk, totalTime, lastTime;

        public bool isBallCollision; 
        public bool isStatic;
        public Color color;

        public Ball(Texture2D _texture, Vector2 _position, float _initialVelociy, Color _color)
        {
            ballTexture = _texture;
            position = _position;
            velocity = new Vector2(0f, 0f);
            mass = 0.0214f;
            initialPosition = position;
            uk = 0.25f;
            radius = ballTexture.Width / 2;

            initialVelocity = _initialVelociy;
            totalTime = 0f;
            lastTime = 0f;


            isBallCollision = false;
            isStatic = true;

            color = _color;
        }

        public float getAngle(Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            return angle;
        }

        public float pixelToMeter(float position)
        {
            float meters = position / 400f;
            return meters;

        }

        public void Update(GameTime gameTime, Vector2 cursor, bool _isLeftClick, float intensity, float windAngle)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            totalTime += elapsed;

            //position.X = pixelToMeter(position.X);
            //position.Y = pixelToMeter(position.Y);

            position.X += velocity.X * elapsed;
            position.Y += velocity.Y * elapsed;

            Console.WriteLine("x,y: "+position.X+", "+position.Y+"metros");

            if (Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) <= 2f)
            {
                isStatic = true;
            }
            else
            {
                isStatic = false;
            }


            //kinematic friction
            if (!isStatic)
            {
                velocity -= new Vector2((float)(uk * 9.81 * Math.Sin(Math.Atan2(velocity.X, velocity.Y))),
                        (float)(uk * 9.81 * Math.Cos(Math.Atan2(velocity.X, velocity.Y))));
            }
            else
            {
                velocity = new Vector2(0f, 0f);
            }

            if (position.Y <= 0f || position.Y >= 600 - ballTexture.Height)
            {
                velocity.Y = -velocity.Y;
            }
            if(position.X <= 0f || position.X >= 1200 - ballTexture.Width)
            {
                velocity.X = -velocity.X;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ballTexture, position, color);
        }

    }
}
