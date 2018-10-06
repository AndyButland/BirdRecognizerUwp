namespace BirdRecognizer
{
    using System;
    using System.Threading.Tasks;
    using Windows.Graphics.Imaging;
    using Windows.Media;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class MainPage : Page
    {
        private CatOrDogModel _model;

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
            const string ModelPath = "ms-appx:///CatOrDogModel.onnx";
            var modelUri = new Uri(ModelPath);
            var file = await StorageFile.GetFileFromApplicationUriAsync(modelUri);
            _model = await CatOrDogModel.CreateCatOrDogModel(file);
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            await GetPredictionFromImage();
        }

        private async Task GetPredictionFromImage()
        {
            var storageFile = await GetFileForClassification();

            var softwareBitmap = await ConvertToSoftwareBitmap(storageFile);

            await ShowImage(softwareBitmap);

            var videoFrame = ConvertToVideoFrame(softwareBitmap);
            var input = new CatOrDogModelInput
                {
                    data = videoFrame
                };
            var output = await _model.EvaluateAsync(input);

            ShowPredictionResult(output);
        }

        private static async Task<StorageFile> GetFileForClassification()
        {
            var fileOpenPicker = new FileOpenPicker
                {
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };
            fileOpenPicker.FileTypeFilter.Add(".bmp");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            return await fileOpenPicker.PickSingleFileAsync();
        }

        private static async Task<SoftwareBitmap> ConvertToSoftwareBitmap(IStorageFile file)
        {
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                return SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }
        }

        private async Task ShowImage(SoftwareBitmap softwareBitmap)
        {
            var imageSource = new SoftwareBitmapSource();
            await imageSource.SetBitmapAsync(softwareBitmap);
            Image.Source = imageSource;
        }

        private static VideoFrame ConvertToVideoFrame(SoftwareBitmap softwareBitmap)
        {
            return VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);
        }

        private void ShowPredictionResult(CatOrDogModelOutput output)
        {
            TextBlock.Text = "Image contains: " + output.classLabel[0];
        }
    }
}
