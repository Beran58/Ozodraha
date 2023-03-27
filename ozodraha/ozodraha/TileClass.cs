using System;
using System.Windows;
using static ozodraha.MainWindow;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ozodraha
{
    public class Tile
    {
        public Rectangle view;
        public Grid viewCont;
        public TileType type;
        public double width;
        public double height;
        public int positionX;
        public int positionY;
        public Brush backgroundColor;
        public int angle;
        public bool rotatable;
        public Grid usedField;
        public Grid menu;
        public Rectangle preview;
        public Button confirm;
        public string address;
        public Tile(Grid usedField, Grid menu, Rectangle preview, Button confirm)
        {
            type = TileType.Empty;
            view = new Rectangle();
            viewCont = new Grid();
            view.Width = width;
            view.Height = height;
            backgroundColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            view.Fill = backgroundColor;
            view.StrokeThickness = 1;
            view.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            view.MouseRightButtonDown += new MouseButtonEventHandler(onClick);
            view.AllowDrop = true;
            view.Drop += new DragEventHandler(onDrop);
            view.MouseMove += new MouseEventHandler(onDrag);
            viewCont.Children.Add(view);
            viewCont.Background = backgroundColor;
            angle = 0;
            rotatable = true;
            this.usedField = usedField;
            this.menu = menu;
            this.preview = preview;
            this.confirm = confirm;
            render();
            rotate();
        }
        public void onClick(object sender, RoutedEventArgs e)
        {
            if (rotatable)
            {
                if (type == TileType.Active)
                {
                    showActiveMenu();
                    selectedTile = this;
                }
                else
                {
                    angle = angle + 90;
                    Console.WriteLine(type);
                }
                render();
                rotate();
            }
        }
        public void onDrop(object sender, DragEventArgs e)
        {
            if (rotatable)
            {
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string recieved = e.Data.GetData(DataFormats.StringFormat).ToString();
                    string recievedType = recieved.Split(' ')[0];
                    int recievedAngle = Int32.Parse(recieved.Split(' ')[1]);
                    switch (recievedType)
                    {
                        case "Empty":
                            if (type != TileType.Empty)
                            {
                                type = TileType.Empty;
                            }
                            break;
                        case "Straight":
                            if (type != TileType.Straight)
                            {
                                type = TileType.Straight;
                            }
                            break;
                        case "Turn":
                            if (type != TileType.Turn)
                            {
                                type = TileType.Turn;
                            }
                            break;
                        case "TCross":
                            if (type != TileType.TCross)
                            {
                                type = TileType.TCross;
                            }
                            break;
                        case "Cross":
                            if (type != TileType.Cross)
                            {
                                type = TileType.Cross;
                            }
                            break;
                        case "Active":
                            if (type != TileType.Active)
                            {
                                type = TileType.Active;
                            }
                            break;
                    }
                    angle = recievedAngle;
                }
            }
            render();
            rotate();
        }

        public void showActiveMenu()
        {
            menu.Visibility = Visibility.Visible;
            preview.Fill = view.Fill;
        }

        public void onDrag(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                string buffer = type.ToString() + " " + angle.ToString();
                DragDrop.DoDragDrop(view, buffer, DragDropEffects.Move);
                if (rotatable)
                {
                    type = TileType.Empty;
                }
                render();
                rotate();
            }
        }
        public void render()
        {
            switch (type)
            {
                case TileType.Empty: view.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255)); break;
                case TileType.Straight: view.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tile_straight.png", UriKind.Relative)) }; break;
                case TileType.Turn: view.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tile_turn.png", UriKind.Relative)) }; break;
                case TileType.TCross: view.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tile_t_cross.png", UriKind.Relative)) }; break;
                case TileType.Cross: view.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tile_cross.png", UriKind.Relative)) }; break;
                case TileType.Active: view.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/tile_active.png", UriKind.Relative)) }; break;
            }
        }
        public void rotate()
        {
            view.LayoutTransform = new RotateTransform(angle);
        }

    }
}
