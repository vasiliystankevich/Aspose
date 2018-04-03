using Project.Kernel.Dal;

namespace Dal
{
    public class PictureModel:BaseModel
    {
        public PictureModel()
        {
        }

        public PictureModel(string pathToFileName)
        {
            PathToFileName = pathToFileName;
        }

        public string PathToFileName { get; set; }
    }
}
