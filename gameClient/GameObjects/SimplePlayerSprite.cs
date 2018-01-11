using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CommonDataItems;
using Engine.Engines;
using Microsoft.Xna.Framework.Input;
using Microsoft.AspNet.SignalR.Client;
using MonoTileMapEx;
using System.Timers;

namespace Sprites
{
    public class SimplePlayerSprite :DrawableGameComponent
    {
        public Texture2D Image;
        public Point Position;
        public Rectangle BoundingRect;
        public bool Visible = true;
        public Color tint = Color.White;
		public PlayerData pData;
        public Point previousPosition;		
        public int speed = 5;
        public TimeSpan delay = new TimeSpan(0,0,1);
        TileManager _tManager;
        Camera cam;
        KeyboardState oldState;
        private Rectangle _characterRect;
        int tileWidth = 64;
        int tileHeight = 64;
        bool canMove = false;
        public static float currentTime = 0f;
        public static float FinshTime = 0f;
        public static bool firstTime = true;
        public static  Timer time = new Timer();
        // Constructor epects to see a loaded Texture
        // and a start position


        public SimplePlayerSprite(Game game, PlayerData data, Texture2D spriteImage,
                            Point startPosition) :base(game)
        {
            _tManager = gameClient.Game1._tManager;
            pData = data;
            DrawOrder = 1;
            game.Components.Add(this);
            // Take a copy of the texture passed down
            Image = spriteImage;
            // Take a copy of the start position
            previousPosition = Position = startPosition;
            // Calculate the bounding rectangle
            BoundingRect = new Rectangle((int)Position.X, Position.Y, Image.Width, Image.Height);



        }

        public override void Update(GameTime gameTime)
        {
            if (gameClient.Game1.totalPlayers.Count >= 1)
            {
                canMove = true;
            }

            KeyboardState keyState = Keyboard.GetState();
            
           
            if (canMove == true)
            {
                //time.Interval = (0.001);

                //time.Start();
                //time.BeginInit();
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                Tile previousTile = _tManager.CurrentTile;

                previousPosition = Position;
                if (InputEngine.IsKeyPressed(Keys.Up))
                    if (_tManager.ActiveLayer.valid("above", _tManager.CurrentTile))
                    {
                        _tManager.CurrentTile =
                              _tManager.ActiveLayer.getadjacentTile("above", _tManager.CurrentTile);
                        // Position = new Point(_tManager.CurrentTile.X * tileWidth, _tManager.CurrentTile.Y * tileHeight);
                    }

                if (InputEngine.IsKeyPressed(Keys.Down))
                    if (_tManager.ActiveLayer.valid("below", _tManager.CurrentTile))
                    {
                        _tManager.CurrentTile =
                              _tManager.ActiveLayer.getadjacentTile("below", _tManager.CurrentTile);
                        //Position = new Point(_tManager.CurrentTile.X * tileWidth, _tManager.CurrentTile.Y * tileHeight);
                    }

                if (InputEngine.IsKeyPressed(Keys.Left))
                    if (_tManager.ActiveLayer.valid("left", _tManager.CurrentTile))
                    {
                        _tManager.CurrentTile =
                              _tManager.ActiveLayer.getadjacentTile("left", _tManager.CurrentTile);
                        //Position = new Point(_tManager.CurrentTile.X * tileWidth, _tManager.CurrentTile.Y * tileHeight);
                    }

                if (InputEngine.IsKeyPressed(Keys.Right))
                    if (_tManager.ActiveLayer.valid("right", _tManager.CurrentTile))
                    {
                        _tManager.CurrentTile =
                              _tManager.ActiveLayer.getadjacentTile("right", _tManager.CurrentTile);
                        // Position = new Point(_tManager.CurrentTile.X * tileWidth, _tManager.CurrentTile.Y * tileHeight);
                    }

                Rectangle r = new Rectangle(_tManager.CurrentTile.X * tileWidth,
                                            _tManager.CurrentTile.Y * tileHeight, tileWidth, tileHeight);
                Position = new Point(_tManager.CurrentTile.X * tileWidth, _tManager.CurrentTile.Y * tileHeight);
                bool inView = GraphicsDevice.Viewport.Bounds.Contains(r);
                bool passable = _tManager.ActiveLayer.Tiles[_tManager.CurrentTile.Y, _tManager.CurrentTile.X].Passable;
                //Vector2 PossibleCameraMove = new Vector2(_characterRect.X - GraphicsDevice.Viewport.Bounds.Width / 2,
                //                                    _characterRect.Y - GraphicsDevice.Viewport.Bounds.Height / 2);
                if (passable)
                {
                    _characterRect = r;
                }
                else
                {
                    _tManager.CurrentTile = previousTile;
                }
            }
               // cam.follow(new Vector2((int)_characterRect.X, (int)_characterRect.Y), GraphicsDevice.Viewport);
           
            oldState = keyState;

            //Position += new Point(speed,0) ;

            delay -= gameTime.ElapsedGameTime; 
            // if we have moved pull back the proxy reference and send a message to the hub
            if(Position != previousPosition && delay.Milliseconds <= 0)
            {
                delay = new TimeSpan(0, 0, 1);
                pData.playerPosition = new Position { X = Position.X, Y = Position.Y };
                IHubProxy proxy = Game.Services.GetService<IHubProxy>();
                proxy.Invoke("Moved", new Object[] 
                {
                    pData.playerID,
                    pData.playerPosition});
            }

            BoundingRect = new Rectangle(Position.X, Position.Y, Image.Width, Image.Height);

            if (_tManager.CurrentTile.X >= 45 && firstTime == true)
            {
                
                
                FinshTime = currentTime;
                //gameClient.Game1.LeaderboardChat(gameTime,FinshTime.ToString());
                //string LeaderboardMessage = "";
                //LeaderboardMessage = FinshTime.ToString();
                //gameClient.Game1.LeaderboardChat(LeaderboardMessage);


                //LeaderboardMessage = gameClient.Game1.LeaderboardChat(LeaderboardMessage);
                    firstTime = false;

                //new GameObject.LeaderboardText(this, Vector2.Zero, FinshTime.ToString());

            
                //proxy.Invoke("Leaderboard", new Object[]
                //    {LeaderboardMessage});
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            if (sp == null) return;
            if (Image != null && Visible)
            {
                sp.Begin();
                sp.Draw(Image, BoundingRect, tint);
               
                sp.End();
            }

            base.Draw(gameTime);
        }



        
    }
}
