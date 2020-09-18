using Newtonsoft.Json;
using Poker.Bizlogic.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FromPokerClient.Bizlogic
{
    public class Common
    {
        public const int DISTANCE = 25, SMALL_DISTANCE = 20;

        public static string GetCardImageUrl(Card card)
        {
            return @"..\..\Image\Poker\" + card.color + "_" + card.point + ".png";
        }

        public static string GetCardImageUrl(string id)
        {
            return @"..\..\Image\Poker\" + id + ".png";
        }

        public static string GetPhotoImageUrl(string id)
        {
            return @"..\..\Image\Photo\" + id + ".png";
        }

        public static string GetBackgorundImageUrl(string id)
        {
            return @"..\..\Image\Material\" + id;
        }

        public static Card GetCardByPictureBox(PictureBox picBox)
        {
            string imageLocation = picBox.ImageLocation;
            int indexLeft = imageLocation.LastIndexOf('\\'), indexRight = imageLocation.LastIndexOf('.');
            string strCard = imageLocation.Substring(indexLeft + 1, indexRight - indexLeft - 1);
            string[] cards = strCard.Split('_');
            return new Card(Convert.ToInt16(cards[0]), Convert.ToInt16(cards[1]));
        }

        public static Point GetPokerPoint(Point benchmark, int cardNum, int index, bool small)
        {
            int nowDistance = small ? SMALL_DISTANCE : DISTANCE;
            int moveNum = index - ((cardNum / 2));
            int pointX = benchmark.X + moveNum * nowDistance;
            int pointY = benchmark.Y;
            return new Point(pointX, pointY);
        }

        public static PictureBox CreatePictureBox(Control.ControlCollection controlCol, Point point, Card card, int index, string type)
        {
            PictureBox picBox = new PictureBox();
            picBox.Location = point;
            switch (type)
            {
                case "myCard":
                    picBox.Width = 75;
                    picBox.Height = 98;
                    break;
                case "sendCard":
                    picBox.Width = 56;
                    picBox.Height = 74;
                    break;
                default:
                    break;
            }
            
            picBox.Load(GetCardImageUrl(card));
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.BackColor = Color.Transparent;
            picBox.Visible = false;
            picBox.Name = "createPicBox_" + type + "_" + index;
            controlCol.Add(picBox);

            return picBox;
        }

        public static void DeletePictureBox(Control.ControlCollection controlCol, string type)
        {
            foreach (Control control in controlCol)
            {
                if(control.Name.Contains("createPicBox_" + type))
                {
                    controlCol.Remove(control);
                }
            }
        }

        public static void SetContorlStyle(Control.ControlCollection controlCol)
        {
            foreach (Control control in controlCol)
            {
                //按钮添加样式
                if (control.Name.Contains("btn"))
                {
                    Button button = control as Button;
                    button.BackgroundImageLayout = ImageLayout.Stretch;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = Color.FromArgb(246, 185, 58);
                    button.Width = 80;
                    button.Height = 25;
                    button.ForeColor = Color.White;
                    button.Font = new Font("微软雅黑", 9);
                    button.FlatStyle = FlatStyle.Flat;
                    GraphicsPath path = GetRoundedRectPath(button.ClientRectangle, 18);
                    Region reg = new Region(path);
                    button.Region = reg;
                }
                else if(control.Name.Contains("lbl"))
                {
                    Label label = control as Label;
                    label.BackgroundImageLayout = ImageLayout.Stretch;
                    label.Width = 200;
                    label.Height = 20;
                    label.TextAlign = ContentAlignment.BottomCenter;
                    label.Visible = false;
                    label.Font = new Font("微软雅黑", 8);
                }
            }
        }

        private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();
            path.AddArc(arcRect, 180, 90);
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
