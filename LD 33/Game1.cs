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
        Random rand;
        bool isSprinting;
        public Game1()
        {
            player = new Entity(64,64, 32, 32, 63);
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (800 / 16) * 16;
            graphics.PreferredBackBufferHeight = (600 / 16) * 16;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            rand = new Random();
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
            bool y = true;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) { Move(player, Entity.Movement.Forward); y = false; }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { Move(player, Entity.Movement.Back); y = false; }
            if (Keyboard.GetState().IsKeyDown(Keys.A) && y) Move(player, Entity.Movement.Left);
            if (Keyboard.GetState().IsKeyDown(Keys.D) && y) Move(player, Entity.Movement.Right);
            isSprinting = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            base.Update(gameTime);

            foreach(Entity entity in entities)
            {
                if(entity != player)
                {
                    Move(entity, entity.direction);
                    if(rand.Next(1,10) == 5)
                    {
                        int i = rand(rand.Next(1, 5));
                        switch(i)
                        {
                            case:
                                break;
                            case:
                                break;
                            case:
                                break;
                            case:
                                break;
                        }
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp,null,null,null,Matrix.CreateTranslation(offset.X, offset.Y, 0));
            foreach (Tile tile in tiles)
            {
                spriteBatch.Draw(tileTexture, new Vector2(tile.x * 64, tile.y * 64), new Rectangle((tile.ID % (tileTexture.Height / 16)) * 16, tile.ID / (tileTexture.Height / 16) * 16, 16,16), Color.White, 0, new Vector2(0,0), 4, SpriteEffects.None, 0);
            }
            foreach (Entity entity in entities)
            {

                float rotation = 0.00f;
                switch (entity.direction)
                {
                    case Entity.Movement.Forward:
                        rotation = 1.57079633f * 4;
                        break;
                    case Entity.Movement.Left:
                        rotation = 1.57079633f * 3;
                        break;
                    case Entity.Movement.Back:
                        rotation = 1.57079633f * 2;
                        break;
                    case Entity.Movement.Right:
                        rotation = 1.57079633f * 1;
                        break;
                }
                spriteBatch.Draw(tileTexture, new Vector2(entity.x + 16, entity.y + 16), new Rectangle((entity.ID % (tileTexture.Height / 16)) * 16, entity.ID / (tileTexture.Height / 16) * 16, 16, 16), Color.White, rotation, new Vector2(8, 8), 2, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Move(Entity entity, Entity.Movement movement)
        {
            int speed = 3;
            if (isSprinting) speed = speed * 2;
            switch(movement)
            {
                case Entity.Movement.Forward:
                    while(willCollide(new Rectangle(entity.x, entity.y - speed, entity.x + entity.width, (entity.y - speed) +  entity.height)))
                    {
                        speed--;
                    }
                    entity.direction = Entity.Movement.Forward;
                    entity.y -= speed;
                    offset.Y += speed;
                    break;
                case Entity.Movement.Left:
                    while (willCollide(new Rectangle(entity.x - speed, entity.y, (entity.x - speed) + entity.width, entity.y + entity.height)))
                    {
                        speed--;
                    }
                    entity.direction = Entity.Movement.Left;
                    entity.x -= speed;
                    offset.X += speed;
                    break;
                case Entity.Movement.Back:
                    while (willCollide(new Rectangle(entity.x, entity.y + speed, entity.x + entity.width, (entity.y + speed)+ entity.height)))
                    {
                        speed--;
                    }
                    entity.direction = Entity.Movement.Back;
                    entity.y += speed;
                    offset.Y -= speed;
                    break;
                case Entity.Movement.Right:
                    while (willCollide(new Rectangle(entity.x + speed, entity.y, (entity.x + speed) + entity.width, entity.y + entity.height)))
                    {
                        speed--;
                    }
                    entity.direction = Entity.Movement.Right;
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
                if (A.X < B.Width && A.Width > B.X && A.Y < B.Height && A.Height > B.Y && tile.hasPhysics)
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
                    bool hasPhysics = ID > 15;
                    tiles.Add(new Tile(x, y, ID, hasPhysics));
                }
                catch
                {

                }
            }
        }
    }
}
