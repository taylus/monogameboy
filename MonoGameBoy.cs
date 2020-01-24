using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameBoy
{
    public class MonoGameBoy : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameBoyScreen screen;
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;
        private GameBoyColorPalette palette = GameBoyColorPalette.Dmg;

        public MonoGameBoy()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            currentKeyboardState = previousKeyboardState = Keyboard.GetState();
            ShowBackgroundMap();
        }

        private void SetWindowSize(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape)) Exit();

            if (WasJustPressed(Keys.T))
            {
                ShowTileSet();
            }
            else if (WasJustPressed(Keys.B))
            {
                ShowBackgroundMap();
            }
            else if (WasJustPressed(Keys.S))
            {
                ShowSpriteLayer();
            }
            else if (WasJustPressed(Keys.P))
            {
                ShowPalettes();
            }

            previousKeyboardState = currentKeyboardState;
            base.Update(gameTime);
        }

        private bool WasJustPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }

        private void ShowTileSet()
        {
            screen = new GameBoyScreen(GraphicsDevice, width: 128, height: 192);
            SetWindowSize(screen.Width * 4, screen.Height * 4);

            string inputFile = Path.Combine("input", "tetris_tileset_pixels.bin");
            screen.PutPixelsFromFile(palette, inputFile);

            Window.Title = $"MonoGameBoy - Tiles [{inputFile}]";
        }

        private void ShowBackgroundMap()
        {
            screen = new GameBoyScreen(GraphicsDevice, width: 256, height: 256);
            SetWindowSize(screen.Width * 2, screen.Height * 2);

            string inputFile = Path.Combine("input", "tetris_bgmap_pixels.bin");
            screen.PutPixelsFromFile(palette, inputFile);

            Window.Title = $"MonoGameBoy - Background Map [{inputFile}]";
        }

        private void ShowSpriteLayer()
        {
            screen = new GameBoyScreen(GraphicsDevice, width: 160, height: 144);
            SetWindowSize(512, 512);

            string inputFile = Path.Combine("input", "tetris_sprite_pixels.bin");
            screen.PutPixelsFromFile(palette, inputFile);

            Window.Title = $"MonoGameBoy - Sprite Layer [{inputFile}]";
        }

        private void ShowPalettes()
        {
            //TODO: implement
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            var sourceRect = Window.Title.Contains("Background Map") ? new Rectangle(0, 0, 160, 144) : (Rectangle?)null;    //HACK: implement scroll registers later
            screen.Draw(spriteBatch, GraphicsDevice.Viewport.Bounds, sourceRect);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}