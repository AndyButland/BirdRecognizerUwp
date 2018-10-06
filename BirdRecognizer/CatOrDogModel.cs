using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// 304aa07b-6c8c-4641-93a6-f3152f8740a1_028da4e3-9c6e-480b-b53c-c1db13d24d70

namespace BirdRecognizer
{
    public sealed class CatOrDogModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class CatOrDogModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public CatOrDogModelOutput()
        {
            this.classLabel = new List<string>();
            this.loss = new Dictionary<string, float>()
            {
                { "cat", float.NaN },
                { "dog", float.NaN },
            };
        }
    }

    public sealed class CatOrDogModel
    {
        private LearningModelPreview learningModel;
        public static async Task<CatOrDogModel> CreateCatOrDogModel(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            CatOrDogModel model = new CatOrDogModel();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<CatOrDogModelOutput> EvaluateAsync(CatOrDogModelInput input) {
            CatOrDogModelOutput output = new CatOrDogModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
