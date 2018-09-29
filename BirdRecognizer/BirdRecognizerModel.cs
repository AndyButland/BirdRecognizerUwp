using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// 5d2c2340-5bd8-4ce0-8fd4-0bb755f1f165_e73151f3-e7f4-46be-83d4-8893ae5c6e5d

namespace BirdRecognizer
{
    public sealed class BirdRecognizerModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class BirdRecognizerModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public BirdRecognizerModelOutput()
        {
            this.classLabel = new List<string>();
            this.loss = new Dictionary<string, float>()
            {
                { "bluetit", float.NaN },
                { "coaltit", float.NaN },
                { "goldfinch", float.NaN },
                { "jay", float.NaN },
                { "robin", float.NaN },
            };
        }
    }

    public sealed class BirdRecognizerModel
    {
        private LearningModelPreview learningModel;
        public static async Task<BirdRecognizerModel> CreateModel(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            BirdRecognizerModel model = new BirdRecognizerModel();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<BirdRecognizerModelOutput> EvaluateAsync(BirdRecognizerModelInput input) {
            BirdRecognizerModelOutput output = new BirdRecognizerModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
