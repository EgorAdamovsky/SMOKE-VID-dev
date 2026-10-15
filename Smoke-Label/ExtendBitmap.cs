using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke_Label
{
    public class ExtendBitmap
    {
        public Bitmap bitmap;
        public string file_name;

        public ExtendBitmap(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            this.file_name = LongHashGenerator.GetLongImageHash(bitmap) + ".jpg"; ;
        }
    }
}
