using System;
using CommonDataItems;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Engine.Engines;
using Sprites;
using System.Collections.Generic;
using GameComponentNS;
using gameClient.GameObjects;
using MonoTileMapEx;
using System.Timers;

namespace gameClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D _character;
        PlayerData Player;
        string connectionMessage = string.Empty;
        Vector2 ViewportCentre
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2);
            }
        }
        List<Texture2D> tileTextures = new List<Texture2D>();
        int[,] tileMap = new int[,]
    {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0},
        {0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0},
        {0,1,1,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,1,1,1,0,1,1,0,1,1,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0},
        {0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0},
        {0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0},
        {0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0},
        {0,0,0,0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0},
        {0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0},
        {0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0},
        {0,1,1,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,0,1,1,1,1,1,1,0,1,1,0,1,1,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,0,1,1,1,1,1,1,0,1,1,0,1,1,0},
        {0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,1,1,0,0,0,1,1,0,1,1,1,0,0,0,0,1,1,0,1,1,0},
        {0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,0,1,1,1,1,1,1,0,1,1,1,1,1,0},
        {0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,0,1,1,1,1,1,1,0,1,1,1,1,1,0},
        {0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0},
        {0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0},
        {0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0},
        {0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,1,1,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0},
        {0,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0},
        {0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,0},
        {0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0},
        {0,0,0,0,1,1,0,1,1,0,1,1,0,1,1,0,1,1,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,0,1,1,0,0,0},
        {0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0},
        {0,1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0},
        {0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,0,1,1,1,1,1},
        {0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,0,1,1,1,1,1},
        {0,1,1,0,1,1,0,0,0,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0,1,1,0,1,1,0,1,1,0,0,0,0,0,1,1,0,1,1,0,0,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,0,1,1,1,1,0},
        {0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,0,1,1,1,1,0},
        {0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,0},
        {0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0},
        {0,0,0,0,0,0,0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0},
        {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };
        int[,] hiddenMap = new int[,]
        {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };

        // SignalR Client object delarations
        int tileWidth = 64;
        int tileHeight = 64;
        Vector2 cameraPosition = new Vector2(0, 0);
        float cameraSpeed = 2;
        private Rectangle _characterRect;
        Vector2 WorldBounds;
        KeyboardState oldState;
        int impassible = 1;
        public enum TileType { Dirt, Grass, Ground, Mud, Road, Rock, Wood };
       // SpriteFont debug;
        public static TileManager _tManager;
     //   private byte pulseColor;
        Camera cam;
        public static List<PlayerData> totalPlayers = new List<PlayerData>();


        HubConnection serverConnection;
        IHubProxy proxy;

        public bool Connected { get; private set; }
        public string ID { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = tileWidth * 46;
            graphics.PreferredBackBufferHeight = tileHeight * 47;
            WorldBounds = new Vector2(tileMap.GetLength(1) * tileWidth, tileMap.GetLength(0) * tileHeight);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // create input engine
            new InputEngine(this);
            new FadeTextManager(this);
            new ChatTextManager(this);
            new Leaderboard(this);
            _tManager = new TileManager();
            cam = new Camera(Vector2.Zero,
                new Vector2(tileMap.GetLength(1) * tileWidth, tileMap.GetLength(0) * tileHeight));


            // TODO: Add your initialization logic here change local host to newly created local host

            serverConnection = new HubConnection("http://cgmazerunner.azurewebsites.net");
            serverConnection.StateChanged += ServerConnection_StateChanged;
            proxy = serverConnection.CreateHubProxy("GameHub");
            serverConnection.Start();


            Action<PlayerData> joined = clientJoined;
            proxy.On<PlayerData>("Joined", joined);

            Action<List<PlayerData>> currentPlayers = clientPlayers;
            proxy.On<List<PlayerData>>("CurrentPlayers", currentPlayers);

            Action<string> _Chat = writeChat;
            proxy.On<string>("chat", _Chat);

            Action<string> LeaderB = writeLeaderboard;
            proxy.On<string>("leader", LeaderB);

            Action<string, Position> otherMove = clientOtherMoved;
            proxy.On<string, Position>("OtherMove", otherMove);

            // Add the proxy client as a Game service o components can send messages 
            Services.AddService<IHubProxy>(proxy);
           
            base.Initialize();
        }

        private void clientOtherMoved(string playerID, Position newPos)
        {
            // iterate over all the other player components 
            // and check to see the type and the right id
            foreach (var player in Components)
            {
                if (player.GetType() == typeof(OtherPlayerSprite)
                    && ((OtherPlayerSprite)player).pData.playerID == playerID)
                {
                    OtherPlayerSprite p = ((OtherPlayerSprite)player);
                    p.pData.playerPosition = newPos;
                    p.Target = new Point(p.pData.playerPosition.X, p.pData.playerPosition.Y);
                    break; // break out of loop as only one player position is being updated
                           // and we have found it
                }
            }
        }

        // Only called when the client joins a game
        private void clientPlayers(List<PlayerData> otherPlayers)
        {
            foreach (PlayerData player in otherPlayers)
            {
                // Create an other player sprites in this client after
                new OtherPlayerSprite(this, player, Content.Load<Texture2D>(player.imageName),
                                        new Point(player.playerPosition.X, player.playerPosition.Y));
                connectionMessage = player.GamerTag + " delivered ";
                totalPlayers.Add(player);
            }
        }

        private void clientJoined(PlayerData otherPlayerData)
        {
            // Create an other player sprite
            new OtherPlayerSprite(this, otherPlayerData, Content.Load<Texture2D>(otherPlayerData.imageName),
                                    new Point(otherPlayerData.playerPosition.X, otherPlayerData.playerPosition.Y));
            new FadeText(this, Vector2.Zero, otherPlayerData.GamerTag + " has joined the game ");
            totalPlayers.Add(otherPlayerData);
        }

        private void ServerConnection_StateChanged(StateChange State)
        {
            switch (State.NewState)
            {
                case ConnectionState.Connected:
                    connectionMessage = "Connected......";
                    Connected = true;
                    startGame();
                    break;
                case ConnectionState.Disconnected:
                    connectionMessage = "Disconnected.....";
                    if (State.OldState == ConnectionState.Connected)
                        connectionMessage = "Lost Connection....";
                    Connected = false;
                    break;
                case ConnectionState.Connecting:
                    connectionMessage = "Connecting.....";
                    Connected = false;
                    break;
            }
        }

        private void startGame()
        {
            // Continue on and subscribe to the incoming messages joined, currentPlayers, otherMove messages

            // Immediate Pattern
            proxy.Invoke<PlayerData>("Join")
                .ContinueWith( // This is an inline delegate pattern that processes the message 
                               // returned from the async Invoke Call
                        (p) => { // Wtih p do 
                            if (p.Result == null)
                                connectionMessage = "No player Data returned";
                            else
                            {
                                CreatePlayer(p.Result);
                                // Here we'll want to create our game player using the image name in the PlayerData 
                                // Player Data packet to choose the image for the player
                                // We'll use a simple sprite player for the purposes of demonstration 
                            }

                        });

        }

        // When we get new player Data Create 
        private void CreatePlayer(PlayerData player)
        {
            ID = player.GamerTag;
            new SimplePlayerSprite(this, player, Content.Load<Texture2D>(player.imageName),
                                    new Point(player.playerPosition.X, player.playerPosition.Y));

             _character = Content.Load<Texture2D>(player.imageName);
            new FadeText(this, Vector2.Zero, " Welcome " + player.GamerTag + " you are playing as " + player.imageName);
            totalPlayers.Add(player);
            //cam.follow(new Vector2((int)player.playerPosition.X, (int)player.playerPosition.Y), GraphicsDevice.Viewport);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService<SpriteBatch>(spriteBatch);

            font = Content.Load<SpriteFont>("Message");
            Services.AddService<SpriteFont>(font);

            Texture2D dirt = Content.Load<Texture2D>("se_free_dirt_texture");
            tileTextures.Add(dirt);

            Texture2D grass = Content.Load<Texture2D>("se_free_grass_texture");
            tileTextures.Add(grass);

            Texture2D ground = Content.Load<Texture2D>("se_free_ground_texture");
            tileTextures.Add(ground);

            Texture2D mud = Content.Load<Texture2D>("se_free_mud_texture");
            tileTextures.Add(mud);

            Texture2D road = Content.Load<Texture2D>("se_free_road_texture");
            tileTextures.Add(road);

            Texture2D rock = Content.Load<Texture2D>("se_free_rock_texture");
            tileTextures.Add(rock);

            Texture2D wood = Content.Load<Texture2D>("se_free_wood_texture");
            tileTextures.Add(wood);

            _character = Content.Load<Texture2D>("Player 1");

            //debug = Content.Load<SpriteFont>("debug");
            string[] backTileNames = { "free","grass","ground", "mud", "road", "rock", "wood" };
            string[] impassableTiles = { "free", "ground", "mud", "rock" };
            string[] hiddenTileNames = { "NONE", "chest", "key" };

            _tManager.addLayer("hidden", hiddenTileNames, hiddenMap);
            int mapWidth = _tManager.Layers[0].MapWidth;
            _tManager.addLayer("background", backTileNames, tileMap);

            _tManager.ActiveLayer = _tManager.getLayer("background");
            _tManager.ActiveLayer.makeImpassable(impassableTiles); 
            _tManager.CurrentTile = new Tile();
            _tManager.CurrentTile.TileName = "Character";
            _tManager.CurrentTile.X = 1;
            _tManager.CurrentTile.Y = 1;
            _characterRect = new Rectangle(tileWidth * _tManager.CurrentTile.X, tileHeight * _tManager.CurrentTile.Y, tileWidth, tileHeight);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //
                Exit(); }

            KeyboardState keyState = Keyboard.GetState();

            if (InputEngine.IsKeyPressed(Keys.T))
            {
                //posts dummy text for chat
                string chatMessage = "";

                chatMessage = Chatting(chatMessage);
               // new GameObjects.ChatText(this, Vector2.Zero, chatMessage);

                proxy.Invoke<string>("Chat", new object[] { ID +": " + chatMessage }).ContinueWith(
                    (q) =>
                    {
                        if (q.Result == null)
                        {
                            new GameObjects.ChatText(this, Vector2.Zero, "Not working");
                        }
                        else
                        {
                            writeChat(q.Result);
                        }
                    });

                //proxy.Invoke("Chat").ContinueWith((Task) => writeChat(Task.res));
            }

            Tile previousTile = _tManager.CurrentTile;

            //setting bounding box
            Rectangle r = new Rectangle(_tManager.CurrentTile.X * tileWidth,
                                           _tManager.CurrentTile.Y * tileHeight, tileWidth, tileHeight);
            bool inView = GraphicsDevice.Viewport.Bounds.Contains(r);
            bool passable = _tManager.ActiveLayer.Tiles[_tManager.CurrentTile.Y, _tManager.CurrentTile.X].Passable;
            //Vector2 PossibleCameraMove = new Vector2(_characterRect.X - GraphicsDevice.Viewport.Bounds.Width / 2,
            //                                   _characterRect.Y - GraphicsDevice.Viewport.Bounds.Height / 2);
            if (passable)
            {
                _characterRect = r;
            }
            else
            {
                _tManager.CurrentTile = previousTile;
            }

            cam.follow(new Vector2((int)_characterRect.X, (int)_characterRect.Y), GraphicsDevice.Viewport);
            oldState = keyState;

            //takes finish time and sends to draw. prevent repeating
            if(SimplePlayerSprite.FinshTime != 0f)
            {
                //new GameObjects.LeaderboardText(this, Vector2.Zero, SimplePlayerSprite.gamerTime);
                SimplePlayerSprite.FinshTime = 0f;
                proxy.Invoke<string>("LeaderBoardINvoke", new object[] { SimplePlayerSprite.gamerTime }).ContinueWith(
                    (q) =>
                    {
                        if (q.Result == null)
                        {
                            new GameObjects.LeaderboardText(this, Vector2.Zero, "Not working");
                        }
                        else
                        {
                            writeLeaderboard(q.Result);
                        }
                    });
            }

            base.Update(gameTime);
        }

        Vector2 TileDifference(Tile t1, Tile t2)
        {
            Vector2 v1 = new Vector2(t1.X, t1.Y) * tileWidth;
            Vector2 v2 = new Vector2(t2.X, t2.Y) * tileWidth;
            Vector2 result = v1 - v2;
            return result;
        }
        public void drawUsingPlayerCamera()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cam.CurrentCameraTranslation);
            TileLayer background = _tManager.getLayer("background");
            List<Tile> surroundingTiles = background.adjacentTo(_tManager.CurrentTile);
            for (int x = 0; x < background.MapWidth; x++)
                for (int y = 0; y < background.MapHeight; y++)
                {
                    int textureIndex = background.Tiles[y, x].Id;
                    Texture2D texture = tileTextures[textureIndex];
                    // Draw surrounding tiles
                    if (surroundingTiles.Contains(background.Tiles[y, x]))
                    {
                        spriteBatch.Draw(texture,
                            new Rectangle(x * tileWidth,
                          y * tileHeight,
                          tileWidth,
                          tileHeight),
                            new Color(255, 255, 255));
                    }
                    else
                    {
                        spriteBatch.Draw(texture,
                            new Rectangle(x * tileWidth,
                          y * tileHeight,
                          tileWidth,
                          tileHeight),
                            Color.White);
                    }

                }
            // draw the character
            spriteBatch.DrawString(font, Math.Round( SimplePlayerSprite.currentTime, 2).ToString(), new Vector2(GraphicsDevice.Viewport.Width/2, 10), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 1);

            spriteBatch.Draw(_character, new Rectangle(_tManager.CurrentTile.X * tileWidth,
                          _tManager.CurrentTile.Y * tileHeight,
                          tileWidth,
                          tileHeight),
                            Color.White);

            //spriteBatch.Draw(_character, _characterRect, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            //spriteBatch.DrawString(debug, cameraPosition.ToString(), new Vector2(10, 10), Color.White);
            //spriteBatch.DrawString(debug, new Vector2(_characterRect.X, _characterRect.Y).ToString(), new Vector2(10, 30), Color.White);
            spriteBatch.End();


        }
        public void drawUsingTileCamera()
        {
            int tileMapWidth = tileMap.GetLength(1); // number of columns
            int tileMapHeight = tileMap.GetLength(0); // number of rows
            spriteBatch.Begin();

            // Draw the background texture
            TileLayer background = _tManager.getLayer("background");
            List<Tile> surroundingTiles = background.adjacentTo(_tManager.CurrentTile);
            for (int x = 0; x < background.MapWidth; x++)
                for (int y = 0; y < background.MapHeight; y++)
                {
                    int textureIndex = background.Tiles[y, x].Id;
                    Texture2D texture = tileTextures[textureIndex];
                    // Draw surrounding tiles
                    if (surroundingTiles.Contains(background.Tiles[y, x]))
                    {
                        spriteBatch.Draw(texture,
                            new Rectangle(x * tileWidth - (int)cameraPosition.X,
                          y * tileHeight - (int)cameraPosition.Y,
                          tileWidth,
                          tileHeight),
                            new Color(255, 255, 255));
                    }
                    else
                    {
                        spriteBatch.Draw(texture,
                            new Rectangle(x * tileWidth - (int)cameraPosition.X,
                          y * tileHeight - (int)cameraPosition.Y,
                          tileWidth,
                          tileHeight),
                            Color.White);
                    }

                }
            // draw the character
            spriteBatch.Draw(_character, new Rectangle(_tManager.CurrentTile.X * tileWidth - (int)cameraPosition.X,
                          _tManager.CurrentTile.Y * tileHeight - (int)cameraPosition.Y,
                          tileWidth,
                          tileHeight),
                            Color.White);

            spriteBatch.End();

        }

        private void writeChat(string result)
        {
            //makes chat text an object for display
            new GameObjects.ChatText(this, Vector2.Zero, result.ToString());
        }
        private void writeLeaderboard(string result)
        {
            //makes leaderboard an object for display
            new GameObjects.LeaderboardText(this, Vector2.Zero, result);
        }
        protected string Chatting(string chatmsg)
        {
            //testing
            chatmsg = "hello";
            return chatmsg;
                       
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            drawUsingPlayerCamera();
            
           
            spriteBatch.Begin();
            spriteBatch.DrawString(font, connectionMessage, new Vector2(10, 10), Color.White,0, Vector2.Zero ,3,SpriteEffects.None,1);
            
            // TODO: Add your drawing code here
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
