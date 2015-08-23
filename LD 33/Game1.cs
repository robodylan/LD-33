// Decompiled with JetBrains decompiler
// Type: LD_33.Game1
// Assembly: LD 33, Version=3.4.0.456, Culture=neutral, PublicKeyToken=null
// MVID: C1E521C5-F2F5-4DB5-969D-DC3DBAC3CC81
// Assembly location: C:\Users\Dylan\Documents\GitHub\LD-33\LD 33\bin\Windows\Debug\LD 33.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace LD_33
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private List<Entity> entities;
    private List<Tile> tiles;
    private List<Button> buttons;
    private Texture2D tileTexture;
    private Texture2D failedTexture;
    private Texture2D buttonTexture;
    private Texture2D cursorTexture;
    private Vector2 offset;
    private Entity player;
    private Random rand;
    private SpriteFont font;
    private Button restartButton;
    private bool isSprinting;
    private bool playerFailed;

    public Game1()
    {
      this.player = new Entity(64, 64, 32, 32, 63);
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.graphics.PreferredBackBufferWidth = 800;
      this.graphics.PreferredBackBufferHeight = 592;
      this.graphics.ApplyChanges();
      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      this.rand = new Random();
      this.tiles = new List<Tile>();
      this.entities = new List<Entity>();
      this.buttons = new List<Button>();
      this.restartButton = new Button(500, 275, "Restart");
      this.buttons.Add(this.restartButton);
      this.entities.Add(this.player);
      this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 30.0);
      this.graphics.SynchronizeWithVerticalRetrace = false;
      this.playerFailed = false;
      this.graphics.ApplyChanges();
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.tileTexture = this.Content.Load<Texture2D>("tiles");
      this.failedTexture = this.Content.Load<Texture2D>("failed");
      this.buttonTexture = this.Content.Load<Texture2D>("button");
      this.cursorTexture = this.Content.Load<Texture2D>("cursor");
      this.font = this.Content.Load<SpriteFont>("font");
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.LoadMap(1);
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        this.Exit();
      bool flag = true;
      if (Keyboard.GetState().IsKeyDown(Keys.W))
      {
        this.Move(this.player, Entity.Movement.Forward);
        flag = false;
      }
      if (Keyboard.GetState().IsKeyDown(Keys.S))
      {
        this.Move(this.player, Entity.Movement.Back);
        flag = false;
      }
      if (Keyboard.GetState().IsKeyDown(Keys.A) & flag)
        this.Move(this.player, Entity.Movement.Left);
      if (Keyboard.GetState().IsKeyDown(Keys.D) & flag)
        this.Move(this.player, Entity.Movement.Right);
      if (Keyboard.GetState().IsKeyDown(Keys.F))
        this.playerFailed = true;
      this.isSprinting = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
      base.Update(gameTime);
      MouseState state = Mouse.GetState();
      if (state.LeftButton == ButtonState.Pressed)
      {
        foreach (Button button in this.buttons)
        {
          state = Mouse.GetState();
          Vector2 vector2 = state.Position.ToVector2();
          if ((double) vector2.X > (double) button.x && (double) vector2.X < (double) (button.width + button.x) && (double) vector2.Y > (double) button.y && (double) vector2.Y < (double) (button.height + button.y))
            button.clicked = true;
        }
      }
      foreach (Entity entity in this.entities)
      {
        if (!entity.Equals((object) this.player))
        {
          this.Move(entity, entity.direction);
          if (this.rand.Next(1, 16) == 5)
          {
            switch (this.rand.Next(1, 5))
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
      if (this.restartButton.clicked)
        this.LoadMap(1);
      this.restartButton.visible = this.playerFailed;
      if (!this.restartButton.clicked || !this.playerFailed)
        return;
      this.restartButton.clicked = false;
      this.restartButton.trans = 0;
    }

    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(Color.Black);
      this.spriteBatch.Begin(SpriteSortMode.Immediate, (BlendState) null, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null, (Effect) null, new Matrix?(Matrix.CreateTranslation(this.offset.X, this.offset.Y, 0.0f)));
      foreach (Tile tile in this.tiles)
        this.spriteBatch.Draw(this.tileTexture, new Vector2((float) (tile.x * 64), (float) (tile.y * 64)), new Rectangle?(new Rectangle(tile.ID % (this.tileTexture.Height / 16) * 16, tile.ID / (this.tileTexture.Height / 16) * 16, 16, 16)), Color.White, 0.0f, new Vector2(0.0f, 0.0f), 4f, SpriteEffects.None, 0.0f);
      foreach (Entity entity in this.entities)
      {
        float rotation = 0.0f;
        switch (entity.direction)
        {
          case Entity.Movement.Back:
            rotation = 3.141593f;
            break;
          case Entity.Movement.Forward:
            rotation = 6.283185f;
            break;
          case Entity.Movement.Right:
            rotation = 1.570796f;
            break;
          case Entity.Movement.Left:
            rotation = 4.712389f;
            break;
        }
        this.spriteBatch.Draw(this.tileTexture, new Vector2((float) (entity.x + 16), (float) (entity.y + 16)), new Rectangle?(new Rectangle(entity.ID % (this.tileTexture.Height / 16) * 16, entity.ID / (this.tileTexture.Height / 16) * 16, 16, 16)), Color.White, rotation, new Vector2(8f, 8f), 2f, SpriteEffects.None, 0.0f);
      }
      foreach (Button button1 in this.buttons)
      {
        if (button1.visible)
        {
          if (button1.trans < 245)
          {
            Button button2 = button1;
            button2.trans = button2.trans + 10;
          }
          Color color = Color.White;
          if (button1.clicked)
            color = Color.Red;
          color.G = (byte) button1.trans;
          color.B = (byte) button1.trans;
          this.spriteBatch.Draw(this.buttonTexture, new Rectangle(button1.x - 11 - (int) this.offset.X, button1.y - 10 - (int) this.offset.Y, button1.width, button1.height), new Rectangle?(new Rectangle(0, 0, 16, 16)), color);
          this.spriteBatch.DrawString(this.font, button1.text, new Vector2((float) button1.x - this.offset.X, (float) button1.y - this.offset.Y), color);
        }
      }
      if (this.playerFailed)
      {
        this.spriteBatch.DrawString(this.font, "Reason: Target had a heart attack", new Vector2(175f - this.offset.X, 215f - this.offset.Y), Color.White);
        this.spriteBatch.Draw(this.failedTexture, new Vector2(175f - this.offset.X, 128f - this.offset.Y), new Rectangle?(), Color.White, 0.0f, new Vector2(0.0f, 0.0f), 6f, SpriteEffects.None, 0.0f);
      }
      this.spriteBatch.Draw(this.cursorTexture, Mouse.GetState().Position.ToVector2() - this.offset, Color.White);
      this.spriteBatch.End();
      base.Draw(gameTime);
    }

    public void Move(Entity entity, Entity.Movement movement)
    {
      int speed = 3;
      if (this.isSprinting && entity.Equals((object) this.player))
        speed *= 2;
      switch (movement)
      {
        case Entity.Movement.Back:
          while (this.willCollide(new Rectangle(entity.x, entity.y + speed, entity.x + entity.width, entity.y + speed + entity.height)))
            --speed;
          entity.direction = Entity.Movement.Back;
          Entity entity1 = entity;
          entity1.y = entity1.y + speed;
          if (!entity.Equals((object) this.player))
            break;
          offset.Y -= speed * 2;
          player.y += speed;
          break;
        case Entity.Movement.Forward:
          while (this.willCollide(new Rectangle(entity.x, entity.y - speed, entity.x + entity.width, entity.y - speed + entity.height)))
            --speed;
          entity.direction = Entity.Movement.Forward;
          Entity entity2 = entity;
          entity2.y = entity2.y - speed;
          if (!entity.Equals((object) this.player))
            break;

          offset.Y += speed * 2;
          player.y -= speed;
          break;
        case Entity.Movement.Right:
          while (this.willCollide(new Rectangle(entity.x + speed, entity.y, entity.x + speed + entity.width, entity.y + entity.height)))
            --speed;
          entity.direction = Entity.Movement.Right;
          Entity entity3 = entity;
          entity3.x = entity3.x + speed;
          if (!entity.Equals((object) this.player))
            break;
          offset.X -= speed;
          player.x += speed;
          break;
        case Entity.Movement.Left:
          while (this.willCollide(new Rectangle(entity.x - speed, entity.y, entity.x - speed + entity.width, entity.y + entity.height)))
            --speed;
          entity.direction = Entity.Movement.Left;
          Entity entity4 = entity;
          entity4.x = entity4.x - speed;
          if (!entity.Equals((object) this.player))
            break;
          offset.X += speed;
          player.x -= speed;
          break;
      }
    }

    public bool willCollide(Rectangle A)
    {
      bool flag = false;
      foreach (Tile tile in this.tiles)
      {
        Rectangle rectangle = new Rectangle(tile.x * 64, tile.y * 64, tile.x * 64 + 64, tile.y * 64 + 64);
        if (A.X < rectangle.Width && A.Width > rectangle.X && (A.Y < rectangle.Height && A.Height > rectangle.Y) && tile.hasPhysics)
          flag = true;
      }
      return flag;
    }

    public void LoadMap(int i)
    {
      foreach (string str in File.ReadAllLines("Content/Maps/Map" + (object) i + ".map"))
      {
        try
        {
          string[] strArray = str.Split(',');
          int x = Convert.ToInt32(strArray[0]);
          int y = Convert.ToInt32(strArray[1]);
          int ID = Convert.ToInt32(strArray[2]);
          bool hasPhysics = ID > 15;
          this.tiles.Add(new Tile(x, y, ID, hasPhysics));
        }
        catch
        {
        }
      }
      this.entities.Clear();
      this.player.x = 64;
      this.player.y = 64;
      this.offset = new Vector2(368f, 264f);
      this.entities.Add(this.player);
      for (int index = 0; index < 16; ++index)
        this.entities.Add(new Entity(this.rand.Next(2, 16) * 64, this.rand.Next(2, 16) * 64, 32, 32, this.rand.Next(56, 63)));
      this.playerFailed = false;
    }
  }
}
