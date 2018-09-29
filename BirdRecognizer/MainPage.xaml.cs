namespace BirdRecognizer
{
    using System;
    using System.Threading.Tasks;
    using Windows.Graphics.Imaging;
    using Windows.Media;
    using Windows.Storage;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class MainPage : Page
    {
        private BirdRecognizerModel _model;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await LoadModel();
        }

        private async Task LoadModel()
        {
            const string ModelPath = "ms-appx:///BirdRecognizerModel.onnx";
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModelPath));
            _model = await BirdRecognizerModel.CreateModel(file);
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            await GetPredictionFromImage();
        }

        private async Task GetPredictionFromImage()
        {
            // TODO: get from picker (see: https://blogs.msdn.microsoft.com/appconsult/2018/05/23/add-a-bit-of-machine-learning-to-your-windows-application-thanks-to-winml/)
            const string TestFile = "ms-appx:///Assets/blue-tit-test.jpg";
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(TestFile));
            var softwareBitmap = await GetSoftwareBitmap(storageFile);

            await ShowImage(softwareBitmap);

            var videoFrame = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);
            var input = new BirdRecognizerModelInput
                {
                    data = videoFrame
                };
            var output = await _model.EvaluateAsync(input);
            TextBlock.Text = output.classLabel[0];
        }

        private async Task ShowImage(SoftwareBitmap softwareBitmap)
        {
            var imageSource = new SoftwareBitmapSource();
            await imageSource.SetBitmapAsync(softwareBitmap);
            Image.Source = imageSource;
        }

        private static async Task<SoftwareBitmap> GetSoftwareBitmap(IStorageFile file)
        {
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                return SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }
        }
    }
}
