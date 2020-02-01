using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameBoy
{
    //http://community.monogame.net/t/how-to-make-screenshot/1742/2
    public class ScreenShotTakingGame : Game
    {
        protected void SaveScreenshot(bool openAfterSaving = false)
        {
            string path = Guid.NewGuid() + ".png";
            using (var screenshot = TakeScreenshot())
            {
                Save(screenshot, path);
            }
            if (openAfterSaving) OpenFile(path);
        }

        protected RenderTarget2D TakeScreenshot()
        {
            int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = GraphicsDevice.PresentationParameters.BackBufferHeight;
            var screenshot = new RenderTarget2D(GraphicsDevice, w, h);
            GraphicsDevice.SetRenderTarget(screenshot);
            Draw(new GameTime());
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Present();
            return screenshot;
        }

        protected static void Save(Texture2D image, string path)
        {
            using (var filestream = new FileStream(path, FileMode.Create))
            {
                image.SaveAsPng(filestream, image.Width, image.Height);
            }
        }

        protected static void OpenFile(string path)
        {
            Process.Start(new ProcessStartInfo() { FileName = path, UseShellExecute = true });
        }
    }
}