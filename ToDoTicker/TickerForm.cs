// <copyright file="TickerForm.cs" company="PublicDomain">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>
namespace PublicDomain
{
    // Directives
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Ticker form.
    /// </summary>
    public partial class TickerForm : Form
    {
        /// <summary>
        /// The form graphics.
        /// </summary>
        private Graphics formGraphics;

        /// <summary>
        /// The ticker text font.
        /// </summary>
        private Font tickerTextFont;

        /// <summary>
        /// The ticker text.
        /// </summary>
        private string tickerText;

        /// <summary>
        /// The position in the x axis.
        /// </summary>
        private int xPos;

        /// <summary>
        /// The color of the foreground.
        /// </summary>
        private Color foregroundColor;

        /// <summary>
        /// The size of the text.
        /// </summary>
        private Size textSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PublicDomain.TickerForm"/> class.
        /// </summary>
        /// <param name="tickerText">Ticker text.</param>
        /// <param name="tickerTextFont">Ticker text font.</param>
        /// <param name="textSpeed">Text speed.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        /// <param name="backgroundColor">Background color.</param>
        public TickerForm(string tickerText, Font tickerTextFont, int textSpeed, Color foregroundColor, Color backgroundColor)
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            this.InitializeComponent();

            // Set form control graphics
            this.formGraphics = this.CreateGraphics();

            // Set ticker text
            this.tickerText = tickerText;

            // Set ticker font
            this.tickerTextFont = tickerTextFont;

            // Measure string.
            this.textSize = TextRenderer.MeasureText(tickerText, tickerTextFont);

            // Set ticker text speed
            this.tickerTimer.Interval = textSpeed;

            // Set form's background color
            this.BackColor = backgroundColor;

            // Set ticker font color
            this.foregroundColor = foregroundColor;

            // Set xPos to the rightmost point
            this.xPos = this.DisplayRectangle.Width;
        }

        /// <summary>
        /// Handles the form's paint event.
        /// </summary>
        /// <param name="e">Paint event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Set current buffered graphics context
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;

            // Set buffered graphics
            BufferedGraphics bufferedGraphics = currentContext.Allocate(e.Graphics, this.DisplayRectangle);

            // Clear graphics
            bufferedGraphics.Graphics.Clear(this.BackColor);

            // Draw ticker text. 5 = half padding
            bufferedGraphics.Graphics.DrawString(this.tickerText, this.tickerTextFont, new SolidBrush(this.foregroundColor), this.xPos, 5);

            // Make it happen
            bufferedGraphics.Render();

            // Free resources
            bufferedGraphics.Dispose();
        }

        /// <summary>
        /// Tickers the timer tick.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void TickerTimerTick(object sender, EventArgs e)
        {
            // Check if text finished displaying
            if (this.xPos < (this.textSize.Width * -1))
            {
                // Reset text position to the rightmost point
                this.xPos = this.DisplayRectangle.Width;
            }
            else
            {
                // Make text move to the left smoothly i.e. one pixel at a time
                this.xPos -= 1;
            }

            // Force redraw
            this.Invalidate();
        }

        /// <summary>
        /// Handles the ticker form shown.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnTickerFormShown(object sender, EventArgs e)
        {
            // Enable news ticker timer
            this.tickerTimer.Enabled = true;
        }
    }
}