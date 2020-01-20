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
        private readonly GameBoyColorPalette palette = GameBoyColorPalette.Dmg;

        public MonoGameBoy()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            screen = new GameBoyScreen(GraphicsDevice, width: 128, height: 192);
            SetWindowSize(screen.Width * 4, screen.Height * 4);
            screen.PutPixelsFromFile(palette, @"input\tetris_tileset_pixels.bin");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(new Color(70, 70, 70));

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            screen.Draw(spriteBatch, GraphicsDevice.Viewport.Bounds);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}