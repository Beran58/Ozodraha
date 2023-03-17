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

        public MainWindow()
        {
            InitializeComponent();
        }
        public class Tile
        {
            public Rectangle view;
            public TileType type;
            public double width;
            public double height;
            public (int, int) position;
            public Brush backgroundColor;
            public Tile()
            {
                type = TileType.Empty;
                view = new Rectangle();
                view.Width = width;
                view.Height = height;
                backgroundColor = new SolidColorBrush(Color.FromRgb(255,255,255));
                view.Fill = backgroundColor;
                view.StrokeThickness = 1;
                view.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                view.MouseLeftButtonDown += new MouseButtonEventHandler(onClick);
                view.AllowDrop = true;
                view.Drop += new DragEventHandler(onDrop);
                view.MouseMove += new MouseEventHandler(onDrag);
            }
            public void onClick(object sender, RoutedEventArgs e)
            {
                type = TileType.Straight;
                Console.WriteLine(type);
                backgroundColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                view.Fill = backgroundColor;
            }
            public void onDrop(object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string recievedType = e.Data.GetData(DataFormats.StringFormat).ToString();
                    switch(recievedType)
                    {
                        case "Empty": type = TileType.Empty; break;
                        case "Straight": type = TileType.Straight; break;
                        case "Turn": type = TileType.Turn; break;
                        case "TCross": type = TileType.TCross; break;
                        case "Cross": type = TileType.Cross; break;
                        case "Active": type = TileType.Active; break;
                    }
                }
            }
            public void onDrag(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                  DragDrop.DoDragDrop(view, type.ToString(), DragDropEffects.Move);
                }
            }

        }
        List<BluetoothDeviceInfo> bluetoothDeviceInfos = new List<BluetoothDeviceInfo>();
        BluetoothDeviceInfo selectedDevice;
        BluetoothClient client;
        bool conVis = false;
        bool connected = false;
        string data;
        System.Net.Sockets.NetworkStream BluetoothStream;
        public double fieldWidth = 12;
        public double fieldHeight = 8;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(connected == false)
            {
                connectBt.Content = "Disconnect";
                connect(selectedDevice.DeviceAddress, BluetoothService.SerialPort);
                connected = true;
            }
            else if (connected == true)
            {
                connectBt.Content = "Connect";
                disconnect();
                connected = false;
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
        }

        private void disconnect()
        {
            client.Close();
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
            for (int i = 0; i < fieldHeight; i++)
            {
                field.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0;i < fieldWidth; i++)
            {
                field.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < fieldHeight; i++)
            {
                for (int j = 0; j < fieldWidth; j++)
                {     
                    Tile tile = new Tile();
                    tile.view.Width = field.Width / fieldWidth;
                    tile.view.Height = field.Height / fieldHeight;
                    Grid.SetRow(tile.view,i);
                    Grid.SetColumn(tile.view, j);
                    field.Children.Add(tile.view);
                }
            }

        }

        private void Field_Loaded(object sender, RoutedEventArgs e)
        {
            generateTiles(fieldWidth,fieldHeight);
        }
    }
}
