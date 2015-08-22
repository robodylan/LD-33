using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD_33
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Entity> entities;
        List<Tile> tiles;
        Texture2D tileTexture;
        Vector2 offset;
        Entity player;

        public Game1()
        {
            player = new Entity(0, 0, 64, 64, 1);
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (800 / 16) * 16;
            graphics.PreferredBackBufferHeight = (600 / 16) * 16;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            offset = new Vector2((((800 / 16) * 16) / 2) - 32, (((600 / 16) * 16) / 2) - 32);
            tiles = new List<Tile>();
            entities = new List<Entity>();
            entities.Add(player);
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f);
            //this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            tileTexture = Content.Load<Texture2D>("tiles");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadMap(1);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.W)) Move(player, Entity.Movement.Forward);
            if (Keyboard.GetState().IsKeyDown(Keys.A)) Move(player, Entity.Movement.Left);
            if (Keyboard.GetState().IsKeyDown(Keys.S)) Move(player, Entity.Movement.Back);
            if (Keyboard.GetState().IsKeyDown(Keys.D)) Move(player, Entity.Movement.Right);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp,null,null,null,Matrix.CreateTranslation(offset.X, offset.Y, 0));
            foreach (Tile tile in tiles)
            {
                spriteBatch.Draw(tileTexture, new Vector2(tile.x * 64, tile.y * 64), new Rectangle((tile.ID % tileTexture.Width) * 16,(tile.ID / tileTexture.Width) * 16,16,16), Color.White, 0, new Vector2(0,0), 4, SpriteEffects.None, 0);
            }
            foreach (Entity entity in entities)
            {
                spriteBatch.Draw(tileTexture, new Vector2(entity.x, entity.y), new Rectangle((entity.ID % tileTexture.Width) * 16, (entity.ID / tileTexture.Width) * 16, 16, 16), Color.White, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Move(Entity entity, Entity.Movement movement)
        {
            int speed = 5;
            switch(movement)
            {
                case Entity.Movement.Forward:
                    while(willCollide(new Rectangle(entity.x, entity.y - speed, entity.x + entity.width, (entity.y - speed) +  entity.height)))
                    {
                        speed--;
                    }
                    entity.y -= speed;
                    offset.Y += speed;
                    break;
                case Entity.Movement.Left:
                    while (willCollide(new Rectangle(entity.x - speed, entity.y, (entity.x - speed) + entity.width, entity.y + entity.height)))
                    {
                        speed--;
                    }
                    entity.x -= speed;
                    offset.X += speed;
                    break;
                case Entity.Movement.Back:
                    while (willCollide(new Rectangle(entity.x, entity.y + speed, entity.x + entity.width, (entity.y + speed)+ entity.height)))
                    {
                        speed--;
                    }
                    entity.y += speed;
                    offset.Y -= speed;
                    break;
                case Entity.Movement.Right:
                    while (willCollide(new Rectangle(entity.x + speed, entity.y, (entity.x + speed) + entity.width, entity.y + entity.height)))
                    {
                        speed--;
                    }
                    entity.x += speed;
                    offset.X -= speed;
                    break;
            }
        }

        public bool willCollide(Rectangle A)
        {
            bool willCollide = false;
            foreach (Tile tile in tiles)
            {
                Rectangle B = new Rectangle(tile.x * 64, tile.y * 64,(tile.x * 64) + 64, (tile.y * 64) + 64);
                if (A.X < B.Width && A.Width > B.X && A.Y < B.Height && A.Height > B.Y)
                {
                    willCollide = true;
                }
            }
            return willCollide;
        }

        public void LoadMap(int i)
        {
            string[] mapData = File.ReadAllLines("Content/Maps/Map" + i + ".map");

            foreach (string blockData in mapData)
            {
                try
                {
                    string[] properties = blockData.Split(',');
                    int x = Convert.ToInt32(properties[0]);
                    int y = Convert.ToInt32(properties[1]);
                    int ID = Convert.ToInt32(properties[2]);
                    bool hasPhysics = true;
                    tiles.Add(new Tile(x, y, ID, hasPhysics));
                }
                catch
                {

                }
            }
        }
    }
}
