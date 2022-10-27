using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TaskForBro
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int counter;
        public MainWindow()
        {
            InitializeComponent();
            counter = int.Parse(ConfigurationManager.AppSettings["Counter"]);
            UpdateTextBlock();
        }

        /// <summary>
        /// Печать QR-кода
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printer = new PrintDialog();
            if (printer.ShowDialog() == true)
            {
                printer.PrintVisual(new Image() { Source = GetQR() }, "Qr-code");
            }
            counter++;
            UpdateTextBlock();
        }

        /// <summary>
        /// Обновление отоброжаемоего значения счетчика (с биндингом беды, поэтому так)
        /// </summary>
        void UpdateTextBlock() => CounterTxb.Text = $"Всего нажатий на кнопку: {counter}";

        /// <summary>
        /// Создание QR-кода
        /// </summary>
        private WriteableBitmap GetQR()
        {
            QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.M);

            QrCode qrCode;

            encoder.TryEncode(counter.ToString(), out qrCode);

            WriteableBitmapRenderer wRenderer = new WriteableBitmapRenderer(new FixedModuleSize(2, QuietZoneModules.Two), Colors.Black, Colors.White);

            WriteableBitmap wBitmap = new WriteableBitmap(50, 50, 96, 96, PixelFormats.Gray8, null);

            wRenderer.Draw(wBitmap, qrCode.Matrix);

            return wBitmap;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var config:Configuration ? = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
    }
}
