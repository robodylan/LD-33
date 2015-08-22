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
            player = new Entity(0, 0, 16, 16, 1);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            tiles = new List<Tile>();
            entities = new List<Entity>();
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
            if (Keyboard.GetState().IsKeyDown(Keys.A)) Move(player, Entity.Movement.Back);
            if (Keyboard.GetState().IsKeyDown(Keys.S)) Move(player, Entity.Movement.Left);
            if (Keyboard.GetState().IsKeyDown(Keys.D)) Move(player, Entity.Movement.Right);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp,null,null,null,null);
            foreach (Tile tile in tiles)
            {
                spriteBatch.Draw(tileTexture, new Vector2(tile.x * 64, tile.y * 64), new Rectangle((tile.ID % tileTexture.Width) * 16,(tile.ID / tileTexture.Width) * 16,16,16),new Color(255,255,255,128), 0, new Vector2(0,0), 4, SpriteEffects.None, 0);
            }
            foreach (Entity entity in entities)
            {
                spriteBatch.Draw(tileTexture, new Vector2(entity.x * 64, entity.y * 64), new Rectangle((entity.ID % tileTexture.Width) * 16, (entity.ID / tileTexture.Width) * 16, 16, 16), new Color(255, 255, 255, 128), 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Move(Entity entity, Entity.Movement movement)
        {
            switch(movement)
            {
                case Entity.Movement.Forward:
                    break;
                case Entity.Movement.Left:
                    break;
                case Entity.Movement.Back:
                    break;
                case Entity.Movement.Right:
                    break;
            }
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
