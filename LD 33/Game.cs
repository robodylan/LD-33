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
        List<Slider> sliders;
        List<Notification> notifications;
        Texture2D tileTexture;
        Texture2D failedTexture;
        Texture2D buttonTexture;
        Texture2D cursorTexture;
        Texture2D sliderTexture;
        Texture2D overlayTexture;
        Random rand;
        Vector2 offset;
        Entity player;
        SpriteFont font;
        SpriteFont fontSmall;
        Button restartButton;
        Button increaseFright;
        Button decreaseFright;
        Button instructionsButton;
        Slider frightness;
        Target target;
        int points;
        int bestScore;
        bool showInstructions;
        bool isSprinting;
        bool playerFailed;
        bool firstLoad;
        string reasonForFailure;
        string instructions;
        public Game()
        {
            player = new Entity(64, 64, 32, 32, 63);
            target = new Target(64, 64);
            target.ID = 46;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (800 / 16) * 16;
            graphics.PreferredBackBufferHeight = (600 / 16) * 16;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            instructions = "    You are a contract scarer, similiar to a contract killer, you scare people\n" +
                           "for money. You must adjust your scariness for each target or else you\n" +
                           "may scare them too much or not enough, adjust your scariness\n" +
                           "levels at the bottom left. The amount needed to scare them depends on \n" +
                           "thier age and gender: women, children, and the eldery scare more easily. \n" +
                           "By the way you are wanted by the police so make sure to watch out for\n" +
                           "them. You are given information about each target which appears\n" +
                           "in the upper right hand corner. The controls are very simple: W,A,S,D\n" +
                           "for movement and E to scare once you are near you're target.\n" +
                           "Good Luck :)";

        }

        protected override void Initialize()
        {
            rand = new Random();
            tiles = new List<Tile>();
            entities = new List<Entity>();
            buttons = new List<Button>();
            sliders = new List<Slider>();

            frightness = new Slider(20, 515, 50);
            restartButton = new Button(450, 275, "Restart");
            increaseFright = new Button(160, 550, "Increase");
            decreaseFright = new Button(31, 550, "Decrease");
            instructionsButton = new Button(180, 355, "Play ");
            buttons.Add(restartButton);
            buttons.Add(increaseFright);
            buttons.Add(decreaseFright);
            buttons.Add(instructionsButton);
            sliders.Add(frightness);
            entities.Add(target);
            entities.Add(player);
            offset = new Vector2((((800 / 16) * 16) / 2) - 32, (((600 / 16) * 16) / 2) - 32);
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f);
            graphics.SynchronizeWithVerticalRetrace = false;
            playerFailed = false;
            showInstructions = true;
            firstLoad = true;
            reasonForFailure = "NULL: ERROR CODE 34";
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            tileTexture = Content.Load<Texture2D>("tiles");
            failedTexture = Content.Load<Texture2D>("failed");
            buttonTexture = Content.Load<Texture2D>("button");
            cursorTexture = Content.Load<Texture2D>("cursor");
            sliderTexture = Content.Load<Texture2D>("slider");
            overlayTexture = Content.Load<Texture2D>("overlay");
            font = Content.Load<SpriteFont>("font");
            fontSmall = Content.Load<SpriteFont>("fontSm");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            foreach(Notification notification in notifications)
            if(!showInstructions && firstLoad)
            {
                firstLoad = false;
                LoadMap(1);
                instructionsButton.visible = false;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            bool y = true;

            if (Keyboard.GetState().IsKeyDown(Keys.F)) playerFailed = true;
            if (!playerFailed)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E)) Scare();
                if (Keyboard.GetState().IsKeyDown(Keys.W)) { Move(player, Entity.Movement.Forward); y = false; }
                if (Keyboard.GetState().IsKeyDown(Keys.S)) { Move(player, Entity.Movement.Back); y = false; }
                if (Keyboard.GetState().IsKeyDown(Keys.A) && y) Move(player, Entity.Movement.Left);
                if (Keyboard.GetState().IsKeyDown(Keys.D) && y) Move(player, Entity.Movement.Right);
            }
            isSprinting = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            base.Update(gameTime);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                foreach (Button button in buttons)
                {
                    Vector2 clickPos = Mouse.GetState().Position.ToVector2();
                    if (clickPos.X > button.x && clickPos.X < (button.width + button.x) && clickPos.Y > button.y && clickPos.Y < (button.height + button.y) && button.visible)
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
                    if(entity.ID == 56)
                    {
                        if(Math.Sqrt(Math.Pow((player.x + 16) - (entity.x + 16), 2) + Math.Pow((player.y + 16) - (entity.y + 16), 2)) < 64 && !playerFailed)
                        {
                            playerFailed = true;
                            reasonForFailure = "Caught by the police";
                        }
                    }
                    Move(entity, entity.direction);
                    if (rand.Next(1, 64) == 5)
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
            if (decreaseFright.clicked && frightness.percentFull > 10) frightness.percentFull -= 5;
            if (increaseFright.clicked && frightness.percentFull < 100) frightness.percentFull += 5;
            if (restartButton.clicked) LoadMap(1);         
            if (instructionsButton.clicked)
            {
                showInstructions = false;
            }   
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Vector2 shake = new Vector2(0,0);
            if (playerFailed)
            {
                shake.X += rand.Next(-2, 3);
                shake.Y += rand.Next(-2, 3);
            }
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, Matrix.CreateTranslation(offset.X + shake.X, offset.Y + shake.Y, 0));
            foreach (Tile tile in tiles)
            {
                if (!firstLoad) spriteBatch.Draw(tileTexture, new Vector2(tile.x * 64, tile.y * 64), new Rectangle((tile.ID % (tileTexture.Height / 16)) * 16, tile.ID / (tileTexture.Height / 16) * 16, 16, 16), Color.White, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
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
                if(!firstLoad) spriteBatch.Draw(tileTexture, new Vector2(entity.x + 16, entity.y + 16), new Rectangle((entity.ID % (tileTexture.Height / 16)) * 16, entity.ID / (tileTexture.Height / 16) * 16, 16, 16), Color.White, rotation, new Vector2(8, 8), 2f, SpriteEffects.None, 0);
            }

            if (!firstLoad) spriteBatch.Draw(overlayTexture, new Vector2(0 - offset.X, 0 - offset.Y), new Color(Color.White, 0.5f));

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
                    color.A = 128;
                    spriteBatch.Draw(buttonTexture, new Rectangle((button.x - 11) - (int)offset.X, (button.y) - (int)offset.Y, button.width, button.height), new Rectangle(0, 0, 16, 16), color);
                    spriteBatch.DrawString(font, button.text, new Vector2(button.x - offset.X, button.y - offset.Y), color);
                }
            }

            if (playerFailed)
            {
                if (!firstLoad) spriteBatch.DrawString(font, "Reason: " + reasonForFailure, new Vector2(175 - offset.X, 215 - offset.Y), Color.White);
                if (!firstLoad) spriteBatch.Draw(failedTexture, new Vector2(175 - offset.X, 128 - offset.Y), null, Color.White, 0, new Vector2(0, 0), 6, SpriteEffects.None, 0);
            }

            foreach (Slider slider in sliders)
            {
                spriteBatch.Draw(buttonTexture, new Rectangle(slider.x - (int)offset.X,slider.y - (int)offset.Y, 200, 25), new Rectangle(0, 0,16,16), Color.White, 0, new Vector2(0,0), SpriteEffects.None, 0);
                spriteBatch.Draw(sliderTexture, new Rectangle((slider.x + 10) - (int)offset.X, (slider.y + 2) - (int)offset.Y, (int)(181f * (slider.percentFull / 100f)), 21), new Rectangle(0, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                spriteBatch.DrawString(font,slider.percentFull + "%", new Vector2((slider.x + 203) - offset.X, (slider.y) - offset.Y), Color.White);
            }
            spriteBatch.DrawString(font, "Scariness", new Vector2(18 - offset.X, 475 - offset.Y), Color.White);
            if (!firstLoad) spriteBatch.DrawString(fontSmall, "Name: " + target.name + "\nAge: " + target.age + "\nDescription: " + target.desc + "\nPayment: $" + target.worth, new Vector2(15 - offset.X, 15 - offset.Y), Color.LightGreen);
            if (!firstLoad) spriteBatch.DrawString(font, "        Cash: $" + points + "\nBest Score: $" + bestScore, new Vector2(550 - offset.X, 32 - offset.Y), Color.DarkGreen);
            if (showInstructions) spriteBatch.DrawString(font, instructions, new Vector2(15 - offset.X, 100 - offset.Y), Color.Red);
            spriteBatch.Draw(cursorTexture, Mouse.GetState().Position.ToVector2() - offset, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Move(Entity entity, Entity.Movement movement)
        {
            int speed = 3;
            if(entity.ID == 56) speed = 5;
            if (isSprinting && entity.Equals(player)) speed = speed * 2;
            bool collidided = false;
            switch (movement)
            {
                case Entity.Movement.Forward:
                    while (willCollide(new Rectangle(entity.x, entity.y - speed, entity.x + entity.width, (entity.y - speed) + entity.height)))
                    {
                        collidided = true;
                        speed--;
                    }
                    entity.direction = Entity.Movement.Forward;
                    entity.y -= speed;
                    if (entity.Equals(player)) offset.Y += speed;
                    break;
                case Entity.Movement.Left:
                    while (willCollide(new Rectangle(entity.x - speed, entity.y, (entity.x - speed) + entity.width, entity.y + entity.height)))
                    {
                        collidided = true;
                        speed--;
                    }
                    entity.direction = Entity.Movement.Left;
                    entity.x -= speed;
                    if (entity.Equals(player)) offset.X += speed;
                    break;
                case Entity.Movement.Back:
                    while (willCollide(new Rectangle(entity.x, entity.y + speed, entity.x + entity.width, (entity.y + speed) + entity.height)))
                    {
                        collidided = true;
                        speed--;
                    }
                    entity.direction = Entity.Movement.Back;
                    entity.y += speed;
                    if (entity.Equals(player)) offset.Y -= speed;
                    break;
                case Entity.Movement.Right:
                    while (willCollide(new Rectangle(entity.x + speed, entity.y, (entity.x + speed) + entity.width, entity.y + entity.height)))
                    {
                        collidided = true;
                        speed--;
                    }
                    entity.direction = Entity.Movement.Right;
                    entity.x += speed;
                    if (entity.Equals(player)) offset.X -= speed;
                    break;
            }
            if(collidided && !entity.Equals(player))
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
            if (points > bestScore) bestScore = points;
            points = 0;
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
            target = new Target(rand.Next(2, 15) * 64, rand.Next(2, 14) * 64);
            entities.Add(player);
            player.x = 64;
            player.y = 64;
            offset = new Vector2((((800 / 16) * 16) / 2) - 64, (((600 / 16) * 16) / 2) - 64);
            entities.Add(target);
            for (int it = 0; it < 32; it++)
            {
                entities.Add(new Entity(rand.Next(2, 15) * 64, rand.Next(2, 14) * 64, 32, 32, rand.Next(56, 63)));
            }
        }

        public void Scare()
        {
            if (Math.Sqrt(Math.Pow(player.x - target.x, 2) + Math.Pow(player.y - target.y, 2)) < 64)
            {
                if (Math.Abs((frightness.percentFull / 10) - target.fear) > 2)
                {
                    playerFailed = true;
                    if ((frightness.percentFull / 10) < target.fear)
                    {
                        reasonForFailure = "Not Scary Enough! Target was unnerved";
                    }
                    else
                    {
                        reasonForFailure = "Too Scary! Target had a heart attack";
                    }
                }

                if (!playerFailed) points += target.worth;
                entities.Remove(target);
                target = new Target(rand.Next(2, 15) * 64, rand.Next(2, 14) * 64);
                entities.Add(target);
            }
        }
    }
}
