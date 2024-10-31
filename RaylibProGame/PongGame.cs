using Raylib_cs;

namespace RaylibProGame;

internal class PongGame
{
    //Used for a basic state machine for game state (look at Update)
    private enum GameStates : ushort
    {
        MainMenu = 0,
        Start    = 1,
        Reset    = 2,
        GameOver = 3
    }

    private GameStates gameState;
    private string gameResult = "placeholder";
    private Sound scoreSFX;
    private Sound gameOverSFX;
    private Music music;
    private int p1Score;
    private int p2Score;
    //initialize the game entities
    //I could make a scene system and automate this but that would be overengineered
    //especially for a pong game
    public Ball   gameBall  = new Ball  (800 / 2, 480/2);
    public Paddle paddle1   = new Paddle(20, 200);
    public Paddle paddle2   = new Paddle(760, 200);

    //making an entity list so I can iterate and update every entity in the scene easily
    public List<Entity> entities = new List<Entity>();

    public PongGame() 
    {
        gameState = GameStates.MainMenu;
        entities.Add(gameBall);
        entities.Add(paddle1);
        entities.Add(paddle2);
    }

    public void GameInitialize()
    {
        //setting collision targets
        paddle1.targetCol = gameBall;
        paddle2.targetCol = gameBall;

        //assigning input
        paddle1.up   = KeyboardKey.W;
        paddle1.down = KeyboardKey.S;
        paddle2.up   = KeyboardKey.Up;
        paddle2.down = KeyboardKey.Down;

        scoreSFX = Raylib.LoadSound("Pickup_Coin2.wav");
        gameOverSFX = Raylib.LoadSound("Explosion4.wav");
        music = Raylib.LoadMusicStream("PongBack.wav");
        Raylib.PlayMusicStream(music);

        //calling the start function of each entity (look at entity.cs, paddle.cs, ball.cs)
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].StartEntity();
        }
    }

    public void GameRun()
    {
        Raylib.InitWindow(800, 480, "Pong!");
        Raylib.SetTargetFPS(60);
        Raylib.InitAudioDevice();
        Raylib.SetMasterVolume(1f);

        GameInitialize();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            Update();

            Raylib.EndDrawing();
        }

        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
        Console.WriteLine("End Console!");
    }
    public void Update()
    {
        Raylib.UpdateMusicStream(music);
        DrawUI();
        //checks for the current state of the game
        switch (gameState)
        {
            case GameStates.MainMenu:
                GameMenu();
                break;
            case GameStates.Start:
                GameUpdate();
                break;
            case GameStates.Reset:
                GameReset();
                break;
            case GameStates.GameOver:
                GameOver();
                break;
        }
    }

    private void DrawUI() 
    {
        Raylib.DrawText(p1Score.ToString(), 800/2 - 34, 12, 40, Color.Black);
        Raylib.DrawText(p2Score.ToString(), 800/2 + 16, 12, 40, Color.Black);
        int recPosY = 2;

        //drawing the net
        for (int i = 0; i < 34; i++) 
        {
            Raylib.DrawRectangle(800/2 - 1, recPosY, 4, 10, Color.Black);
            recPosY = recPosY + 15;
        }
    }

    private void GameMenu()
    {
        Raylib.DrawText("Press Enter", 800 / 2 - 160, 480 / 2 - 24, 50, Color.Black);
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            gameState = GameStates.Start;
        }
    }

    private void GameUpdate()
    {
        //looping over the entity list and calling their updates
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].EntityUpdate();
            entities[i].EntityDraw();
        }

        if (gameBall.ballPosX > 800) 
        {
            gameState = GameStates.Reset;
            Raylib.PlaySound(scoreSFX);
            p1Score += 1;
            gameBall.LastWin = false;
        }
        if (gameBall.ballPosX < 0) 
        {
            gameState = GameStates.Reset;
            Raylib.PlaySound(scoreSFX);
            p2Score += 1;
            gameBall.LastWin = true;
        }
        if (p1Score >= 5)
        {
            Raylib.PlaySound(gameOverSFX);
            gameState = GameStates.GameOver;
            gameResult = "Player 1 Wins!";
        }
        if (p2Score >= 5)
        {
            Raylib.PlaySound(gameOverSFX);
            gameState = GameStates.GameOver;
            gameResult = "Player 2 Wins!";
        }
    }

    private void GameReset() 
    {
        gameBall.ballPosX = 800 / 2;
        gameBall.ballPosY = 480 / 2;
        if (gameBall.LastWin)
        {
            gameBall.velocity.X = 4f;
        }
        else
        {
            gameBall.velocity.X = -4f;
        }
        gameBall.velocity.Y = 0f;
        paddle1.paddlePosX = 20;
        paddle1.paddlePosY = 200;
        paddle2.paddlePosX = 760;
        paddle2.paddlePosY = 200;
        gameState = GameStates.Start;
    }

    private void GameOver() 
    {
        if (p1Score >= 5)
        {
            gameState = GameStates.GameOver;
            Raylib.DrawText(gameResult, 800 / 2 - 160, 480 / 2, 50, Color.Black);
        }
        if (p2Score >= 5)
        {
            gameState = GameStates.GameOver;
            Raylib.DrawText(gameResult, 800 / 2 - 160, 480 / 2, 50, Color.Black);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            gameState = GameStates.Reset;
            p1Score = 0;
            p2Score = 0;
        }
    }
}
