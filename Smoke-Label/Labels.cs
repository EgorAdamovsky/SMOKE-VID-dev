namespace Smoke_Label
{
    public class Labels
    {
        public int frame;
        public float x;
        public float y;
        public float w;
        public float h;
        public bool seq;

        public Labels(int frame, float x, float y, float w, float h, bool seq)
        {
            this.frame = frame;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.seq = seq;
        }

        public override string ToString()
        {
            return (seq ? "↓ " : "↑ ") + "Кадр " + frame + ": [" + x + ", " + y + ", " + w + ", " + h + "]";
        }
    }
}
