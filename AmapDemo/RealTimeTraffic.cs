using System;
using System.Windows.Forms;

namespace AmapDemo
{
    public partial class RealTimeTraffic : UserControl
    {
        private Uri uri;

        public RealTimeTraffic()
        {
            InitializeComponent();
        }

        public Uri Uri
        {
            get => uri;
            set
            {
                uri = value;
                webBrowser1.Url = uri;
            }
        }

        public string DocumentText
        {
            get => documentText;
            set
            {
                documentText = value;
                webBrowser1.DocumentText = documentText;
            }
        }

        private string documentText;
    }
}