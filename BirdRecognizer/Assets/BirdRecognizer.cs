using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// 5d2c2340-5bd8-4ce0-8fd4-0bb755f1f165_ff289e5c-4894-4097-8294-91eaddc3ed36

namespace BirdRecognizer
{
    public sealed class _x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36ModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class _x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36ModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public _x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36ModelOutput()
        {
            this.classLabel = new List<string>();
            this.loss = new Dictionary<string, float>()
            {
                { "Blue tit", float.NaN },
                { "Coal tit", float.NaN },
                { "Goldfinch", float.NaN },
                { "Jay", float.NaN },
                { "Robin", float.NaN },
            };
        }
    }

    public sealed class _x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36Model
    {
        private LearningModelPreview learningModel;
        public static async Task<_x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36Model> Create_x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36Model(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            _x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36Model model = new _x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36Model();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<_x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36ModelOutput> EvaluateAsync(_x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36ModelInput input) {
            _x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36ModelOutput output = new _x0035_d2c2340_x002D_5bd8_x002D_4ce0_x002D_8fd4_x002D_0bb755f1f165_ff289e5c_x002D_4894_x002D_4097_x002D_8294_x002D_91eaddc3ed36ModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
