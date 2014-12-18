﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using DWrite = SharpDX.DirectWrite;
using D2D = SharpDX.Direct2D1;

namespace CleverDock.Graphics.Views
{
    public class FPSCounterView : View
    {
        // These are used for tracking an accurate frames per second
        private DateTime time;
        private int frameCount;
        private int fps;

        private DWrite.TextFormat textFormat;
        private DWrite.Factory writeFactory;
        private D2D.SolidColorBrush whiteBrush;

        public FPSCounterView(RectangleF bounds)
            : base(bounds)
        {
            this.writeFactory = new DWrite.Factory();
            this.textFormat = new DWrite.TextFormat(writeFactory, "Arial", 12);
        }

        protected override void OnCreateResources()
        {
            this.whiteBrush = new D2D.SolidColorBrush(RenderTarget, new Color(1f, 1f, 1f));

            // Start animation.
            base.OnCreateResources();
        }

        protected override void OnFreeResources()
        {
            // Stop animation to prevent rendering while the resources are freed.
            base.OnFreeResources();

            if (this.whiteBrush != null)
            {
                this.whiteBrush.Dispose();
                this.whiteBrush = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose();

            if (writeFactory != null)
            {
                writeFactory.Dispose();
                writeFactory = null;
            }
            if (textFormat != null)
            {
                textFormat.Dispose();
                textFormat = null;
            }
        }

        protected override void OnRender()
        {
            // Calculate our actual frame rate
            this.frameCount++;
            if (DateTime.UtcNow.Subtract(this.time).TotalSeconds >= 1)
            {
                this.fps = this.frameCount;
                this.frameCount = 0;
                this.time = DateTime.UtcNow;
            }

            // Draw a little FPS in the top left corner
            string text = string.Format("FPS {0}", this.fps);
            this.RenderTarget.DrawText(text, this.textFormat, Frame, this.whiteBrush);
        }
    }
}
