using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD_33
{

    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Entity> entities;
        List<Tile> tiles;
        List<Button> buttons;
        Texture2D tileTexture;
        Texture2D failedTexture;
        Texture2D buttonTexture;
        Texture2D cursorTexture;
        Vector2 offset;
        Entity player;
        Random rand;
        SpriteFont font;
        Button restartButton;
        Button increaseFright;
        Button decreaseFright;
        int frightness;
        bool isSprinting;
        bool playerFailed;
        public Game()
        {
            player = new Entity(64, 64, 32, 32, 63);
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (800 / 16) * 16;
            graphics.PreferredBackBufferHeight = (600 / 16) * 16;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            rand = new Random();
            tiles = new List<Tile>();
            entities = new List<Entity>();
            buttons = new List<Button>();
            restartButton = new Button(450, 275, "Restart");
            increaseFright = new Button(100,400,"Increase");
            decreaseFright = new Button(100,450,"Decrease");
            buttons.Add(restartButton);
            buttons.Add(increaseFright);
            buttons.Add(decreaseFright);
            entities.Add(player);
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f);
            graphics.SynchronizeWithVerticalRetrace = false;
            playerFailed = false;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            tileTexture = Content.Load<Texture2D>("tiles");
            failedTexture = Content.Load<Texture2D>("failed");
            buttonTexture = Content.Load<Texture2D>("button");
            cursorTexture = Content.Load<Texture2D>("cursor");
            font = Content.Load<SpriteFont>("font");
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

            if (Keyboard.GetState().IsKeyDown(Keys.F)) playerFailed = true;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) { Move(player, Entity.Movement.Forward); y = false; }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { Move(player, Entity.Movement.Back); y = false; }
            if (Keyboard.GetState().IsKeyDown(Keys.A) && y) Move(player, Entity.Movement.Left);
            if (Keyboard.GetState().IsKeyDown(Keys.D) && y) Move(player, Entity.Movement.Right);
            isSprinting = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            base.Update(gameTime);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                foreach (Button button in buttons)
                {
                    Vector2 clickPos = Mouse.GetState().Position.ToVector2();
                    if (clickPos.X > button.x && clickPos.X < (button.width + button.x) && clickPos.Y > button.y && clickPos.Y < (button.height + button.y))
                    {
                        button.clicked = true;
                    }

                }
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                foreach (Button button in buttons)
                {
                    button.clicked = false;
                }
            }

            foreach (Entity entity in entities)
            {
                if (!entity.Equals(player))
                {
                    Move(entity, entity.direction);
                    if (rand.Next(1, 16) == 5)
                    {
                        int i = rand.Next(1, 5);
                        switch (i)
                        {
                            case 1:
                                entity.direction = Entity.Movement.Back;
                                break;
                            case 2:
                                entity.direction = Entity.Movement.Forward;
                                break;
                            case 3:
                                entity.direction = Entity.Movement.Left;
                                break;
                            case 4:
                                entity.direction = Entity.Movement.Right;
                                break;
                        }
                    }
                }
            }

            restartButton.visible = playerFailed;
            foreach (Button button in buttons)
            {
                if (button.clicked) button.trans = 0;
            }
            if (restartButton.clicked) LoadMap(1);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, Matrix.CreateTranslation(offset.X, offset.Y, 0));
            foreach (Tile tile in tiles)
            {
                spriteBatch.Draw(tileTexture, new Vector2(tile.x * 64, tile.y * 64), new Rectangle((tile.ID % (tileTexture.Height / 16)) * 16, tile.ID / (tileTexture.Height / 16) * 16, 16, 16), Color.White, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
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
                spriteBatch.Draw(tileTexture, new Vector2(entity.x + 16, entity.y + 16), new Rectangle((entity.ID % (tileTexture.Height / 16)) * 16, entity.ID / (tileTexture.Height / 16) * 16, 16, 16), Color.White, rotation, new Vector2(8, 8), 2f, SpriteEffects.None, 0);
            }

            foreach (Button button in buttons)
            {
                if (button.visible)
                {
                    if (button.trans < 255 - 10)
                    {
                        button.trans += 10;
                    }
                    Color color = Color.White;
                    if (button.clicked) color = Color.Red;
                    color.G = (byte)button.trans;
                    color.B = (byte)button.trans;
                    spriteBatch.Draw(buttonTexture, new Rectangle((button.x - 11) - (int)offset.X, (button.y - 10) - (int)offset.Y, button.width, button.height), new Rectangle(0, 0, 16, 16), color);
                    spriteBatch.DrawString(font, button.text, new Vector2(button.x - offset.X, button.y - offset.Y), color);
                }
            }

            if (playerFailed)
            {
                spriteBatch.DrawString(font, "Reason: " + "Target had a heart attack", new Vector2(175 - offset.X, 215 - offset.Y), Color.White);
                spriteBatch.Draw(failedTexture, new Vector2(175 - offset.X, 128 - offset.Y), null, Color.White, 0, new Vector2(0, 0), 6, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(cursorTexture, Mouse.GetState().Position.ToVector2() - offset, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Move(Entity entity, Entity.Movement movement)
        {
            int speed = 3;
            if (isSprinting && entity.Equals(player)) speed = speed * 2;
            switch (movement)
            {
                case Entity.Movement.Forward:
                    while (willCollide(new Rectangle(entity.x, entity.y - speed, entity.x + entity.width, (entity.y - speed) + entity.height)))
                    {
                        speed--;
                    }
                    entity.direction = Entity.Movement.Forward;
                    entity.y -= speed;
                    if (entity.Equals(player)) offset.Y += speed;
                    break;
                case Entity.Movement.Left:
                    while (willCollide(new Rectangle(entity.x - speed, entity.y, (entity.x - speed) + entity.width, entity.y + entity.height)))
                    {
                        speed--;
                    }
                    entity.direction = Entity.Movement.Left;
                    entity.x -= speed;
                    if (entity.Equals(player)) offset.X += speed;
                    break;
                case Entity.Movement.Back:
                    while (willCollide(new Rectangle(entity.x, entity.y + speed, entity.x + entity.width, (entity.y + speed) + entity.height)))
                    {
                        speed--;
                    }
                    entity.direction = Entity.Movement.Back;
                    entity.y += speed;
                    if (entity.Equals(player)) offset.Y -= speed;
                    break;
                case Entity.Movement.Right:
                    while (willCollide(new Rectangle(entity.x + speed, entity.y, (entity.x + speed) + entity.width, entity.y + entity.height)))
                    {
                        speed--;
                    }
                    entity.direction = Entity.Movement.Right;
                    entity.x += speed;
                    if (entity.Equals(player)) offset.X -= speed;
                    break;
            }
        }

        public bool willCollide(Rectangle A)
        {
            bool willCollide = false;
            foreach (Tile tile in tiles)
            {
                Rectangle B = new Rectangle(tile.x * 64, tile.y * 64, (tile.x * 64) + 64, (tile.y * 64) + 64);
                if (A.X < B.Width && A.Width > B.X && A.Y < B.Height && A.Height > B.Y && tile.hasPhysics)
                {
                    willCollide = true;
                }
            }
            return willCollide;
        }

        public void LoadMap(int i)
        {
            playerFailed = false;
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
            entities.Clear();
            player.x = 64;
            player.y = 64;
            offset = new Vector2((((800 / 16) * 16) / 2) - 32, (((600 / 16) * 16) / 2) - 32);
            entities.Add(player);
            for (int it = 0; it < 16; it++)
            {
                entities.Add(new Entity(rand.Next(2, 16) * 64, rand.Next(2, 16) * 64, 32, 32, rand.Next(56, 63)));
            }
        }
    }
}
