﻿using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Phoenix.Client.Classes.Extensions
{
    public class VisualBar : ProgressBar
    {
        public enum TextPosition
        {
            Left,
            Right,
            Center,
            Sliding,
            None
        }

        private Color channelColor = Color.LightSteelBlue;
        private Color sliderColor = Color.RoyalBlue;
        private Color foreBackColor = Color.RoyalBlue;
        private int channelHeight = 6;
        private int sliderHeight = 6;
        private TextPosition showValue = TextPosition.Right;
        private string symbolBefore = "";
        private string symbolAfter = "";
        private bool showMaximun = false;

        public VisualBar()
        {

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.ForeColor = Color.White;

        }

        public Color ChannelColor
        {
            get { return channelColor; }
            set
            {
                channelColor = value;
                this.Invalidate();
            }
        }

        public Color SliderColor
        {
            get { return sliderColor; }
            set
            {
                sliderColor = value;
                this.Invalidate();
            }
        }

        public Color ForeBackColor
        {
            get { return foreBackColor; }
            set
            {
                foreBackColor = value;
                this.Invalidate();
            }
        }

        public int ChannelHeight
        {
            get { return channelHeight; }
            set
            {
                channelHeight = value;
                this.Invalidate();
            }
        }

        public int SliderHeight
        {
            get { return sliderHeight; }
            set
            {
                sliderHeight = value;
                this.Invalidate();
            }
        }

        public TextPosition ShowValue
        {
            get { return showValue; }
            set
            {
                showValue = value;
                this.Invalidate();
            }
        }

        public string SymbolBefore
        {
            get { return symbolBefore; }
            set
            {
                symbolBefore = value;
                this.Invalidate();
            }
        }

        public string SymbolAfter
        {
            get { return symbolAfter; }
            set
            {
                symbolAfter = value;
                this.Invalidate();
            }
        }

        public bool ShowMaximun
        {
            get { return showMaximun; }
            set
            {
                showMaximun = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
            }
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            Graphics graph = pevent.Graphics;
            Rectangle rectChannel = new(0, 0, this.Width, ChannelHeight);
            using var brushChannel = new SolidBrush(channelColor);
            if (channelHeight >= sliderHeight)
                rectChannel.Y = this.Height - channelHeight;
            else rectChannel.Y = this.Height - ((channelHeight + sliderHeight) / 2);
            graph.Clear(this.Parent.BackColor);
            graph.FillRectangle(brushChannel, rectChannel);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graph = e.Graphics;
            double scaleFactor = (((double)this.Value - this.Minimum) / ((double)this.Maximum - this.Minimum));
            int sliderWidth = (int)(this.Width * scaleFactor);
            Rectangle rectSlider = new(0, 0, sliderWidth, sliderHeight);
            using var brushSlider = new SolidBrush(sliderColor);
            if (sliderHeight >= channelHeight)
                rectSlider.Y = this.Height - sliderHeight;
            else rectSlider.Y = this.Height - ((sliderHeight + channelHeight) / 2);
            if (sliderWidth > 1)
                graph.FillRectangle(brushSlider, rectSlider);
            if (showValue != TextPosition.None)
                DrawValueText(graph, sliderWidth, rectSlider);
        }

        private void DrawValueText(Graphics graph, int sliderWidth, Rectangle rectSlider)
        {
            // Fields
            string text = this.Value.ToString();
            if (showMaximun) text = text + "/" + this.Maximum.ToString();
            var textSize = TextRenderer.MeasureText(text, this.Font);
            var rectText = new Rectangle(0, 0, textSize.Width + 5, textSize.Height + 2);
            using var brushText = new SolidBrush(this.ForeColor);
            using var brushTextBack = new SolidBrush(foreBackColor);
            using var textFormat = new StringFormat();
            switch (showValue)
            {
                case TextPosition.Left:
                    rectText.X = 0;
                    textFormat.Alignment = StringAlignment.Near;
                    break;
                case TextPosition.Right:
                    rectText.X = this.Width - textSize.Width;
                    textFormat.Alignment = StringAlignment.Far;
                    break;
                case TextPosition.Center:
                    rectText.X = (this.Width - textSize.Width) / 2;
                    rectText.Y = (this.Height - textSize.Height) / 2;
                    textFormat.Alignment = StringAlignment.Center;
                    break;
                case TextPosition.Sliding:
                    rectText.X = sliderWidth - textSize.Width;
                    textFormat.Alignment = StringAlignment.Center;
                    using (var brushClear = new SolidBrush(this.Parent.BackColor))
                    {
                        var rect = rectSlider;
                        rect.Y = rectText.Width;
                        rect.Height = rectText.Height;
                        graph.FillRectangle(brushClear, rect);
                    }
                    break;
            }
            graph.FillRectangle(brushTextBack, rectText);
            graph.DrawString(text, this.Font, brushText, rectText, textFormat);
        }

    }
}
