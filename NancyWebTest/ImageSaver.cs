using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ninject;

namespace NancyWebTest
{
    public class ImageSaver 
    {
        [Inject]
        public ImageSaver(IImageStore store)
        {
            this.store = store;
        }

        private IImageStore store;

        public void Save(string filename, Stream filestream)
        {
            this.store.Save(filename, filestream);
        }

    }
}
