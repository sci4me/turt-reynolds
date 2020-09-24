using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Turt;
using Turt.Lexer;
using Turt.Parser;
using Turt.Runtime;
using Turt.Util;

namespace Turt_Reynolds {
    public partial class TurtReynolds : Form, ExecutionEnvironment {
        public TurtReynolds() {
            InitializeComponent();
        }

        private Bitmap buffer;
        private Graphics graphics;
        private bool penDown;
        private Pen pen;
        private float x;
        private float y;
        private int angle;
        private Scope globalScope;
        private Frame currentFrame;

        private void OnLoad(object sender, EventArgs e) {
            buffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(buffer);

            penDown = true;
            pen = new Pen(Color.Black, 2);
            x = Width / 2;
            y = Height / 2;

            globalScope = new Scope();
            currentFrame = new Frame(globalScope);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            e.Graphics.DrawImageUnscaled(buffer, 0, 0);
        }

        private void OnDispose() {
            pen.Dispose();
        }

        public void PenUp() {
            penDown = false;
        }

        public void PenDown() {
            penDown = true;
        }

        public void TurnLeft(TurtInteger angle) {
            this.angle = (this.angle - angle.Value) % 360;
        }

        public void TurnRight(TurtInteger angle) {
            this.angle = (this.angle + angle.Value) % 360;
        }

        public void Go(TurtInteger distance) {
            var angleInRadians = Math.PI * angle / 180;
            var directionX = Math.Cos(angleInRadians);
            var directionY = Math.Sin(angleInRadians);

            var d = distance.Value;
            var newX = (float)(x + directionX * d).Clamp(0, buffer.Width);
            var newY = (float)(y + directionY * d).Clamp(0, buffer.Height);
            
            if(penDown) {
                graphics.DrawLine(pen, x, y, newX, newY);
                Invalidate();
            }

            x = newX;
            y = newY;
        }

        public void SetColor(TurtInteger red, TurtInteger green, TurtInteger blue) {
            pen.Color = Color.FromArgb(red.Value.Clamp(0, 255), green.Value.Clamp(0, 255), blue.Value.Clamp(0, 255));
        }

        public void SetWidth(TurtInteger width) {
            pen.Width = width.Value.Clamp(1, 10);
        }

        public void Dot(TurtInteger size) {
            var half_size = size.Value / 2;
            graphics.FillEllipse(pen.Brush, x - half_size, y - half_size, size.Value, size.Value);
            Invalidate();
        }

        public void PushFrame() {
            currentFrame = currentFrame.CreateChild();
        }

        public void PopFrame() {
            currentFrame = currentFrame.Parent;
            if (currentFrame == null) throw new InvalidOperationException();
        }

        public Frame Frame => currentFrame;

        private void LoadCode(string source) {
            var lexer = new Lexer(source);
            var parser = new Parser(lexer.Tokenize());
            var ast = parser.Parse();
            ast.Eval(this);
        }

        private void FileLoad_Click(object sender, EventArgs e) {
            try {
                var dialog = new OpenFileDialog() {
                    Filter = "Turt Files|*.turt",
                    Title = "Select a Turt File",
                    InitialDirectory = Environment.CurrentDirectory
                };

                if (dialog.ShowDialog() == DialogResult.OK) {
                    var reader = new StreamReader(dialog.OpenFile());
                    var source = reader.ReadToEnd();
                    reader.Close();

                    LoadCode(source);
                }
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }
    }
}