using Raylib_cs;
using System.Numerics;

namespace RaylibProGame;

internal class Ball : Entity
{
    public  float   ballPosX;
    public  float   ballPosY;
    //the speed the ball moves set to the velocity vector
    private float   speed = 4f;
    private float   angle = 0.0f;

    public Vector2 velocity = new Vector2();

    public bool IsRicochet = false;
    //who scored p1 or p2 in that order, 0 is p1, 1 is p2
    public bool LastWin = false;


    //is used to set the size of the ball collision rec
    public Vector2 ballSize = new Vector2  (20, 20);

    //the actual collision rec instance for the ball itself
    public Rectangle col    = new Rectangle();

    public Ball(int x, int y) 
    {
        ballPosX = x;
        ballPosY = y;
        col.Size = ballSize;
    }

    public override void StartEntity()
    {
        Console.WriteLine("Start ran!", this);
        velocity.X = speed;
        velocity.Y = angle;
    }

    public override void EntityUpdate()
    {
        ballPosX += velocity.X;
        ballPosY += velocity.Y;
        col.X = ballPosX;
        col.Y = ballPosY;

        //edge controls
        if (ballPosY > 460 || ballPosY < 0) 
        {
            velocity.Y = velocity.Y * -1;
            IsRicochet = true;
        }
    }

    public override void EntityDraw()
    {
        //visualize the collision box
        //Raylib.DrawRectangle((int)col.X - 2, (int)col.Y - 2, 24, 24, Color.Red);
        //drawing the actual ball
        Raylib.DrawRectangle((int)ballPosX, (int)ballPosY, 20, 20, Color.Black);
        //draw origin
        //Raylib.DrawRectangle((int)ballPosX+ 9, (int)ballPosY + 9, 2, 2, Color.Green);
    }
}