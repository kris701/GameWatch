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

namespace GameWatch.UserControls.Overview
{
    /// <summary>
    /// Interaction logic for PieChartControl.xaml
    /// </summary>
    public partial class PieChartControl : UserControl
    {
        private Color _goodColor = Color.FromArgb(255, 0, 255, 0);
        private Color _badColor = Color.FromArgb(255, 255, 0, 0);

        public PieChartControl()
        {
            InitializeComponent();
        }

        public void UpdateChart(double value, double max)
        {
            double percent = 1;
            if (value < max)
                percent = value / max;
            double newWidth = this.Width * percent;
            double newHeight = this.Height * percent;

            FillEllipse.Width = newWidth;
            FillEllipse.Height = newHeight;

            FillLabel.Content = $"{Math.Round(percent * 100,0)}%";
            FillEllipse.Fill = SetBackgroundColor(percent);
        }

        private SolidColorBrush SetBackgroundColor(double percent)
        {
            var color1 = Color.FromArgb(255, (byte)(_goodColor.R * (1 - percent)), (byte)(_goodColor.G * (1 - percent)), (byte)(_goodColor.B * (1 - percent)));
            var color2 = Color.FromArgb(255, (byte)(_badColor.R * percent), (byte)(_badColor.G * percent), (byte)(_badColor.B * percent));
            return new SolidColorBrush(Color.Add(color1, color2));
        }
    }
}
