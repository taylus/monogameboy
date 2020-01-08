using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameBoy
{
    public class GameBoyScreen
    {
        public const int WidthInPixels = 160;
        public const int HeightInPixels = 144;

        private RenderTarget2D renderTarget;

        public GameBoyScreen(GraphicsDevice graphicsDevice, int width = WidthInPixels, int height = HeightInPixels)
        {
            renderTarget = new RenderTarget2D(graphicsDevice, width, height);
        }

        public void PutPixel(Color color, int x, int y)
        {
            var pixels = new Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(pixels);
            pixels[renderTarget.Width * y + x] = color;
            renderTarget.SetData(pixels);
        }

        public void PutPixels(Color[] color)
        {
            renderTarget.SetData(color);
        }

        public void PutPixels(GameBoyColorPalette palette, int[] colors)
        {
            //this is the method I imagine the emulator proper will use
            //the emulator core's PPU should produce a 160x144 grid of color palette numbers
            //which MonoGame should be able to draw at 60 fps even in a naive/unoptimized way
            PutPixels(colors.Select(c => palette[c]).ToArray());
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
        {
            spriteBatch.Draw(renderTarget, destinationRectangle, Color.White);
        }
    }
}
