using System.Collections.Generic;

namespace Smoke_Label
{
    public class ObjectManager
    {
        public int index = -1;
        public List<Labels> labels = new List<Labels>();
        public List<Labels> compiled_labels = new List<Labels>();

        public ObjectManager()
        {

        }

        public ObjectManager(int index)
        {
            this.index = index;
        }

        public override string ToString()
        {
            return "Объект #" + index;
        }
    }
}
