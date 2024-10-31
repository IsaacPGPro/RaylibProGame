using Raylib_cs;
using System.Numerics;

namespace RaylibProGame;

internal class Paddle : Entity
{
    private const float speed = 4.0f;
    //the collision rec for the paddle itself
    private Rectangle col = new Rectangle();

    public float paddlePosX;
    public float paddlePosY;
    //public Vector2 origin = new Vector2();
    public float paddleOrigin;
    //used to set the size of the collison rec
    public Vector2 paddleSize = new Vector2(20, 100);

    //contains a reference to the ball object
    //used to check the collision in the IsColliding() method
    public Ball? targetCol;

    //input
    public KeyboardKey up;
    public KeyboardKey down;
    //sound file
    public Sound hit;

    public Vector2 velocity = new Vector2();

    public Paddle(int PosX, int PosY)
    {
        paddlePosX = PosX;
        paddlePosY = PosY;
        col.Size   = paddleSize;
    }

    //is called once when the game is starting
    public override void StartEntity()
    {
        Console.WriteLine("Start ran!", this);
        paddleOrigin = paddlePosY - 50;
        hit = Raylib.LoadSound("Hit_Hurt14.wav");
    }

    //is ran every game loop
    public override void EntityUpdate()
    {
        paddleOrigin = paddlePosY + 50;
        //updates the position of the collision rec
        col.X = paddlePosX;
        col.Y = paddlePosY;

        //checks for the collision(look at method for more)
        if (IsColliding())
        {
            float currentVelocity = targetCol.velocity.X;

            float ballOrigin = targetCol.ballPosY + 10;
            //this multiplies the current horizontal velocity of the ball
            //by 20% every time it is hit
            currentVelocity = currentVelocity * 1.2f;

            //resets the velocity so a new one can be given
            //depending on how the ball is hit
            

            Raylib.PlaySound(hit);

            //clamps the horizontal velocity to prevent tunneling
            //problem with the current collision detection
            //not an issue for pong though 50px is enough speed
            //avg game never exceeds 20-30
            if (currentVelocity > 50f) 
            {
                currentVelocity = 50f;
            }
            else if (currentVelocity < -50f) 
            {
                currentVelocity = -50f;
            }


            Console.WriteLine(currentVelocity);

            
            //this inverts the velocity of the ball each time it is hit
            //send the ball flying the other direction
            //aka a hit
            if (targetCol.ballPosX > paddlePosX)
            {
                targetCol.velocity.X = currentVelocity * -1;
            }
            else if(targetCol.ballPosX < paddlePosX)
            {
                targetCol.velocity.X = currentVelocity * -1;
            }


            //changes the hit direction of the ball based on
            //position of the hit on the paddle
            //top of paddle hits up
            //bottom hits down
            //also is clamped to 10px
            if (targetCol.IsRicochet) 
            {
                Console.WriteLine(targetCol.velocity.Y);
                Console.WriteLine("The ball collided");
                targetCol.IsRicochet = false;
                return;
            }

            targetCol.velocity.Y = 0f;
            if (ballOrigin < paddleOrigin)
            {
                targetCol.velocity.Y = (ballOrigin / paddleOrigin) * -1f;
            }
            else if (ballOrigin > paddleOrigin)
            {
                targetCol.velocity.Y = (ballOrigin / paddleOrigin) * 1f;
            }
            if (targetCol.velocity.Y > 5) 
            {
                targetCol.velocity.Y = 5;
            }
            if (targetCol.velocity.Y < -5)
            {
                targetCol.velocity.Y = 5;
            }
            Console.WriteLine(targetCol.velocity.Y);
            Console.WriteLine("The ball collided");
        }


        //paddle controls
        if (paddlePosY < 0)
        {
            paddlePosY = 0;
            return;
        }
        if (paddlePosY + 100 > 480)
        {
            paddlePosY = 480 - 100;
            return;
        }
        if (Raylib.IsKeyDown(up))
        {
            velocity.Y = -speed;
        }
        else if (Raylib.IsKeyDown(down))
        {
            velocity.Y = speed;
        }
        else
        {
            velocity.Y = 0;
        }
        paddlePosY += velocity.Y;
    }

    //is ran every game loop
    public override void EntityDraw()
    {
        //draws the collision
        //Raylib.DrawRectangle((int)col.X-2, (int)col.Y-2, 24, 104, Color.Blue);
        //draws the actual paddle
        Raylib.DrawRectangle((int)paddlePosX, (int)paddlePosY, 20, 100, Color.Black);
        //draws the origin used to calculate ball angle
        //Raylib.DrawRectangle((int)paddlePosX + 9, (int)paddleOrigin, 2, 2, Color.Green);
        //Raylib.DrawRectangle((int)paddlePosX + 9, (int)paddlePosY + 100, 2, 2, Color.Green);

    }

    //checks to see if there is a ball to begin with
    //it checks the collision of the paddle against the ball and returns the result
    public bool IsColliding()
    {
        if (targetCol == null) return false;

        if(Raylib.CheckCollisionRecs(col, targetCol.col)) return true;
        
        return false;
    }
}
