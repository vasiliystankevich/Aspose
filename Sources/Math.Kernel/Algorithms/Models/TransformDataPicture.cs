using Math.Kernel.Algorithms.Transformations;

namespace Math.Kernel.Algorithms.Models
{
    public class TransformDataPicture
    {
        public TransformDataPicture(int width, int height, ITransformImageAttributes transformImageAttributes)
        {
            Width = width;
            Height = height;
            TransformImageAttributes = transformImageAttributes;
        }

        public int Height { get; set; } 
        public int Width { get; set; }
        public ITransformImageAttributes TransformImageAttributes { get; set; }
    }
}
