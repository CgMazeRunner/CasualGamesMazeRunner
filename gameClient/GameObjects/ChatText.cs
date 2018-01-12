using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Engines;
using Microsoft.Xna.Framework.Input;
using CommonDataItems;

namespace gameClient.GameObjects
{
    class ChatTextManager : DrawableGameComponent
    {
        Vector2 basePosition;

        public ChatTextManager(Game game) : base(game)
        {
            game.Components.Add(this);
            basePosition = new Vector2(game.GraphicsDevice.Viewport.Width - 600, 0);
        }
        protected override void LoadContent()
        {

            base.LoadContent();
        }

        

        public override void Update(GameTime gameTime)
        {
           
            var faders = Game.Components.Where(
                            t => t.GetType() == typeof(ChatText));
            if (faders.Count() > 0)
            {
                Vector2 b = basePosition;
                var font = Game.Services.GetService<SpriteFont>();
                Vector2 fontsize = font.MeasureString("Y");
                foreach (ChatText ft in faders)
                {
                    ft.Position = b;
                    b += new Vector2(0, fontsize.Y + 30);
                }
            }
            base.Update(gameTime);
        }


    }


    class ChatText : DrawableGameComponent
    {
        string text;
        Vector2 position;
        byte blend = 255;

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public ChatText(Game game, Vector2 Position, string Text) : base(game)
        {
            game.Components.Add(this);
            text = Text;
            this.Position = Position;
        }

        public override void Update(GameTime gameTime)
        {
            var faders = Game.Components.Where(
                           t => t.GetType() == typeof(ChatText));
            if (faders.Count() > 5)
            {
                //Caps the number of chat messages

                //if (blend > 0)
                //    blend--;
                //else { Game.Components.Remove(this); }

                Game.Components.Remove(this);
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var sp = Game.Services.GetService<SpriteBatch>();
            var font = Game.Services.GetService<SpriteFont>();
            Color myColor = new Color((byte)0, (byte)0, (byte)0, blend);
            sp.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            sp.DrawString(font, text, Position, new Color((byte)255, (byte)255, (byte)255, blend), 0, Vector2.Zero, 3, SpriteEffects.None, 1);
            sp.End();
            base.Draw(gameTime);
        }

    }
}