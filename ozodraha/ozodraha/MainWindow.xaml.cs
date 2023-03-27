using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;


namespace ozodraha
{

    public partial class MainWindow : Window
   {
        public enum TileType
        {
            Empty,
            Straight,
            Turn,
            TCross,
            Cross,
            Active
        }
        public static TileType draggedType;
        public List<Tile> Tiles;
        public double originalFieldWidth;
        public double originalFieldHeight;
        List<BluetoothDeviceInfo> bluetoothDeviceInfos = new List<BluetoothDeviceInfo>();
        BluetoothDeviceInfo selectedDevice;
        BluetoothClient client;
        bool conVis = false;
        bool connected = false;
        string data;
        System.Net.Sockets.NetworkStream BluetoothStream;
        public double fieldWidth = 10;
        public double fieldHeight = 6;
        public static Tile selectedTile;

        public MainWindow()
        {
            InitializeComponent();
            Tiles = new List<Tile>();
            originalFieldHeight = field.Height;
            originalFieldWidth = field.Width;
            widthText.Text = fieldWidth.ToString();
            heightText.Text = fieldHeight.ToString();
            selectedTile = new Tile(field, Active_options, Preview,Confirm);
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(connected == false)
            {
                connectBt.Content = "Disconnect";
                connect(selectedDevice.DeviceAddress, BluetoothService.SerialPort);
            }
            else if (connected == true)
            {
                connectBt.Content = "Connect";
                disconnect();
            }
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (BluetoothDeviceInfo info in bluetoothDeviceInfos)
            {
                if (info.DeviceName.ToString() == e.AddedItems[0].ToString())
                {
                    selectedDevice = info;
                }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            scan();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            send();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (conVis == false)
            {
                deviceList.Visibility = Visibility.Visible;
                connectBt.Visibility = Visibility.Visible;
                discoverBt.Visibility = Visibility.Visible;
                conVis = true;
            }
            else
            {
                deviceList.Visibility = Visibility.Collapsed;
                connectBt.Visibility = Visibility.Collapsed;
                discoverBt.Visibility = Visibility.Collapsed;
                conVis = false;
            }
        }

        private void scan()
        {
            deviceList.Items.Clear();
            client = new BluetoothClient();
            BluetoothDeviceInfo[] devices = client.DiscoverDevices();
            String deviceName;
            foreach (BluetoothDeviceInfo device in devices)
            {
                deviceName = device.DeviceName.ToString();
                bluetoothDeviceInfos.Add(device);
                deviceList.Items.Add(deviceName);
            }
        }
        private void connect(InTheHand.Net.BluetoothAddress address, Guid service)
        {
            client.Connect(address, service);
            if(client.Connected == true)
            {
                connected = true;
            }
            else connected = false;
        }

        private void disconnect()
        {
            client.Close();
            connected = false;
        }

        private void send()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            BluetoothStream = client.GetStream();
            BluetoothStream.Write(buffer, 0, buffer.Length);
            BluetoothStream.Close();
        }
        private void generateTiles(double fieldWidth, double fieldHeight)
        {

                for (int i = 0; i < fieldWidth; i++)
                {
                    field.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(154,GridUnitType.Pixel)});
                }
                for (int i = 0; i < fieldHeight; i++)
                {
                    field.RowDefinitions.Add(new RowDefinition {Height = new GridLength(154,GridUnitType.Pixel)});
                }
            
            for (int i = 0; i < fieldHeight; i++)
            {
                for (int j = 0; j < fieldWidth; j++)
                {     
                    Tile tile = new Tile(field,Active_options,Preview,Confirm);
                    tile.positionX = j;
                    tile.positionY = i;
                        tile.view.Width = field.Width / fieldWidth;
                        tile.view.Height = field.Height / fieldHeight;
                    Tiles.Add(tile);
                    Grid.SetRow(tile.viewCont,tile.positionY);
                    Grid.SetColumn(tile.viewCont, tile.positionX);
                    field.Children.Add(tile.viewCont);
                }
            }

        }

        public void reGenerateTiles(double fieldWidth, double fieldHeight)
        {
            field.Children.Clear();
            field.RowDefinitions.Clear();
            field.ColumnDefinitions.Clear();
            Tiles.Clear();
            void fieldGen()
            {
                    for (int i = 0; i < fieldWidth; i++)
                    {
                        field.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(154, GridUnitType.Pixel) });           
                    }
                    for (int i = 0; i < fieldHeight; i++)
                    {
                        field.RowDefinitions.Add(new RowDefinition { Height = new GridLength(154, GridUnitType.Pixel) });
                    }
            }
                     fieldGen();
                for (int i = 0; i < fieldHeight; i++)
                    {
                for (int j = 0; j < fieldWidth; j++)
                    {
                    Tile tile = new Tile(field,Active_options,Preview,Confirm);
                    tile.positionX = j;
                    tile.positionY = i;
                    tile.view.Width = field.Width / fieldWidth;
                    tile.view.Height = field.Height / fieldHeight;
                    Tiles.Add(tile);
                    Grid.SetRow(tile.viewCont, tile.positionY);
                    Grid.SetColumn(tile.viewCont, tile.positionX);
                }
                }
            foreach (Tile tile in Tiles)
                    {
                        Grid.SetRow(tile.viewCont, tile.positionY);
                        Grid.SetColumn(tile.viewCont, tile.positionX);
                        field.Children.Add(tile.viewCont);
                    }             
        }
        private void generateMenu()
        {
            for(int i = 0;i < 5;i++)
            {
                    tileMenu.RowDefinitions.Add(new RowDefinition { Height = new GridLength(tileMenu.ActualHeight / 5) });
                    Tile tile = new Tile(field,Active_options,Preview,Confirm);
                    tile.view.Width = tileMenu.Width / 6;
                    tile.view.Height = tileMenu.Width / 6;
                    tile.type = (TileType)i + 1;
                    tile.rotatable = false;
                    tile.render();
                    Grid.SetRow(tile.viewCont, i);
                    tileMenu.Children.Add(tile.viewCont);
            }
        }

        private void Field_Loaded(object sender, RoutedEventArgs e)
        {
            generateTiles(fieldWidth,fieldHeight);
        }
        private void tileMenuLoaded(object sender, RoutedEventArgs e)
        {
            generateMenu();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
           
        }
        bool settingsExpand = false;
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if(settingsExpand == true)
            {
                settings.Visibility = Visibility.Hidden;
                widthlabel.Visibility = Visibility.Hidden;
                heightLabel.Visibility = Visibility.Hidden;
                widthText.Visibility = Visibility.Hidden;
                heightText.Visibility = Visibility.Hidden;
                sizeConfirm.Visibility = Visibility.Hidden;
                settingsExpand = false;
            }
            else if (settingsExpand == false)
            {
                settings.Visibility= Visibility.Visible;
                widthlabel.Visibility = Visibility.Visible;
                heightLabel.Visibility= Visibility.Visible;
                widthText.Visibility= Visibility.Visible;
                heightText.Visibility = Visibility.Visible;
                sizeConfirm.Visibility = Visibility.Visible;
                settingsExpand = true;
            }
        }

        private void sizeConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(Int32.Parse(widthText.Text) > 0 && Int32.Parse(heightText.Text) > 0)
            {
                reGenerateTiles(Int32.Parse(widthText.Text), Int32.Parse(heightText.Text));
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            data = "";
            data = Address.Text;
            switch (Color_codes.SelectedIndex)
            {
                case 0: data += "48<@";break;
                case 1: data += "16;@"; break;
                case 2: data += "189@"; break;
                case 3: data += "28:@"; break;
                case 4: data += "38;@"; break;
                case 5: data += "36;@"; break;
                case 6: data += "369@"; break;
                case 7: data += "289@"; break;
                case 8: data += "389@"; break;
                case 9: data += "35:@"; break;
                case 10: data += "25:@"; break;
                case 11: data += "27:@"; break;
                case 12: data += "169@"; break;
                case 13: data += "35;@"; break;
                case 14: data += "48;="; break;
                case 15: data += "169>"; break;
                case 16: data += "38:="; break;
                case 17: data += "25:="; break;
                case 18: data += "16<?"; break;
                case 19: data += "179@"; break;
                case 20: data += "18;>"; break;
                case 21: data += "27<="; break;
                case 22: data += "48:?"; break;
                case 23: data += "48:="; break;
                case 24: data += "25;>"; break;
                case 25: data += "17:?"; break;
                case 26: data += "16;="; break;
                case 27: data += "179>"; break;
                case 28: data += "17:@"; break;
                case 29: data += "279@"; break;
            }
            if(connected)send(); 
            Active_options.Visibility = Visibility.Hidden;
        }

        private void Color_codes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(selectedTile == null)
            {
                selectedTile = new Tile(field, Active_options, Preview, Confirm);
            }
            if(Preview == null)
            {
                Preview = new Rectangle();
            }
            Brush newView; 
            switch (Color_codes.SelectedIndex)
            {
                case 0: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/tile_active.png", UriKind.Relative)) }; break;
                case 1: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/super_slow.png", UriKind.Relative)) }; break;
                case 2: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/slow.png", UriKind.Relative)) }; break;
                case 3: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/cruise.png", UriKind.Relative)) }; break;
                case 4: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/fast.png", UriKind.Relative)) }; break;
                case 5: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/turbo.png", UriKind.Relative)) }; break;
                case 6: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/nitro_boost.png", UriKind.Relative)) }; break;
                case 7: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/left.png", UriKind.Relative)) }; break;
                case 8: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/straight.png", UriKind.Relative)) }; break;
                case 9: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/right.png", UriKind.Relative)) }; break;
                case 10: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/switch_left.png", UriKind.Relative)) }; break;
                case 11: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/switch_straight.png", UriKind.Relative)) }; break;
                case 12: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/switch_right.png", UriKind.Relative)) }; break;
                case 13: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/U_turn.png", UriKind.Relative)) }; break;
                case 14: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/U_turn_end.png", UriKind.Relative)) }; break;
                case 15: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/tornado.png", UriKind.Relative)) }; break;
                case 16: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/zigzag.png", UriKind.Relative)) }; break;
                case 17: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/spin.png", UriKind.Relative)) }; break;
                case 18: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/backwalk.png", UriKind.Relative)) }; break;
                case 19: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/pause.png", UriKind.Relative)) }; break;
                case 20: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/timer_on.png", UriKind.Relative)) }; break;
                case 21: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/timer_off.png", UriKind.Relative)) }; break;
                case 22: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/win_play.png", UriKind.Relative)) }; break;
                case 23: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/win_over.png", UriKind.Relative)) }; break;
                case 24: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/x-ing_counter.png", UriKind.Relative)) }; break;
                case 25: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/turn_counter.png", UriKind.Relative)) }; break;
                case 26: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/path_counter.png", UriKind.Relative)) }; break;
                case 27: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/point_counter.png", UriKind.Relative)) }; break;
                case 28: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/point_plus.png", UriKind.Relative)) }; break;
                case 29: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/point_minus.png", UriKind.Relative)) }; break;
                default: newView = new ImageBrush { ImageSource = new BitmapImage(new Uri("../../tileImages/tiles_active/tile_active.png", UriKind.Relative)) }; break;
            }
            selectedTile.view.Fill = newView;
            Preview.Fill = newView;
        }

        private void RotateCounter_Click(object sender, RoutedEventArgs e)
        {
            selectedTile.angle -= 90;
            selectedTile.render();
            Preview.LayoutTransform = new RotateTransform(selectedTile.angle);
        }

        private void RotateClock_Click(object sender, RoutedEventArgs e)
        {
            selectedTile.angle += 90;
            selectedTile.rotate();
            Preview.LayoutTransform = new RotateTransform(selectedTile.angle);
        }
    }
}
