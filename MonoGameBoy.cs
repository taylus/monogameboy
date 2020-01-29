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
            else if (WasJustPressed(Keys.Space))
            {
                ShowScreen();
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

            string inputFile = Path.Combine("input", "pokemon_reds_room_tileset.bin");
            screen.PutPixelsFromFile(palette, inputFile);

            Window.Title = $"MonoGameBoy - Tiles [{inputFile}]";
        }

        private void ShowBackgroundMap()
        {
            screen = new GameBoyScreen(GraphicsDevice, width: 256, height: 256);
            SetWindowSize(screen.Width * 2, screen.Height * 2);

            string inputFile = Path.Combine("input", "pokemon_reds_room_bgmap.bin");
            screen.PutPixelsFromFile(palette, inputFile);

            Window.Title = $"MonoGameBoy - Background Map [{inputFile}]";
        }

        private void ShowSpriteLayer()
        {
            screen = new GameBoyScreen(GraphicsDevice, width: 160, height: 144);
            SetWindowSize(screen.Width * 3, screen.Height * 3);

            string inputFile = Path.Combine("input", "pokemon_reds_room_sprites.bin");
            screen.PutPixelsFromFile(palette, inputFile);

            Window.Title = $"MonoGameBoy - Sprite Layer [{inputFile}]";
        }

        private void ShowPalettes()
        {
            //TODO: implement
        }

        private void ShowScreen()
        {
            screen = new GameBoyScreen(GraphicsDevice, width: 160, height: 144);
            SetWindowSize(screen.Width * 3, screen.Height * 3);

            string inputFile = Path.Combine("input", "pokemon_reds_room_screen.bin");
            screen.PutPixelsFromFile(palette, inputFile);

            Window.Title = $"MonoGameBoy [{inputFile}]";
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            screen.Draw(spriteBatch, GraphicsDevice.Viewport.Bounds);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}