using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dal.Properties;
using Newtonsoft.Json;
using Project.Kernel.Dal;
using Project.Kernel.Extensions;

namespace Dal
{
    public interface IExtendedFunctionDalContext
    {
        void Scan();
        void Scan(string pathToLocalDataBase);
        Bitmap FindPictureByFileName(string pictureFileName);
        Bitmap GetPicture(Guid id);
        Dictionary<double, List<double>> GetSobelDataMatrix();
        Dictionary<double, List<double>> GetHighFrequencySpatialDataMatrix();
        Dictionary<double, List<double>> GetLowFrequencySpatialDataMatrix();
    }

    public interface IDalContext : IDbContext, IExtendedFunctionDalContext, IDisposable
    {
        IDbSet<PictureModel> Pictures { get; set; }
    }

    public class LocalDalContext: IDalContext
    {
        public LocalDalContext()
        {
            Pictures = Creator<LocalDbSet<PictureModel>>.Create();
        }

        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task<int>.Factory.StartNew(() => 0);
        }

        public void ExecuteTransaction(Action action)
        {
            action();
        }

        public void Scan()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var pathToLocalStorage = Path.Combine(basePath, "LocalStorage");
            Scan(pathToLocalStorage);
        }

        public void Scan(string pathToLocalDataBase)
        {
            var files = Directory.GetFiles(pathToLocalDataBase, "*.bmp");
            foreach (var file in files)
            {
                var model = Creator<PictureModel>.Create(file.ToLowerInvariant());
                Pictures.Add(model);
            }
        }

        public Bitmap FindPictureByFileName(string pictureFileName)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var pathToLocalStorage = Path.Combine(basePath, "LocalStorage");
            var key = Path.Combine(pathToLocalStorage, pictureFileName).ToLowerInvariant();
            var findValue=Pictures.First(el => string.Compare(el.PathToFileName, key, StringComparison.OrdinalIgnoreCase) == 0);
            var findId = findValue?.Id ?? Guid.Empty;
            return GetPicture(findId);
        }

        public Bitmap GetPicture(Guid id)
        {
            if (Guid.Empty == id) return new Bitmap(0, 0);
            var findValue = Pictures.First(el => el.Id == id);
            var findFileName = findValue?.PathToFileName ?? string.Empty;
            return string.IsNullOrWhiteSpace(findFileName) ? new Bitmap(0,0) : new Bitmap(findFileName);
        }

        public Dictionary<double, List<double>> GetDataMatrixFromResource(byte[] resource)
        {
            using (var ms = new MemoryStream(resource))
            using (var sr = new StreamReader(ms))
            {
                var json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<double, List<double>>>(json);
            }
        }

        public Dictionary<double, List<double>> GetSobelDataMatrix()
        {
            return GetDataMatrixFromResource(Resources.Sobel);
        }

        public Dictionary<double, List<double>> GetHighFrequencySpatialDataMatrix()
        {
            return GetDataMatrixFromResource(Resources.HighFrequencySpatial);
        }

        public Dictionary<double, List<double>> GetLowFrequencySpatialDataMatrix()
        {
            return GetDataMatrixFromResource(Resources.LowFrequencySpatial);
        }

        public void Dispose()
        {
        }

        public IDbSet<PictureModel> Pictures { get; set; }
    }
}