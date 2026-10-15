using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Smoke_Label
{
    public class COCO
    {
        public const float W_goal = 1280;
        public const float H_goal = 720;

        public class class_images
        {
            public string file_name;
            public int id;
            public int width;
            public int height;
            public string[] prev_imgs;

            public class_images()
            {
            }

            public class_images(string file_name, int id, int width, int height, string[] prev_imgs)
            {
                this.file_name = file_name;
                this.id = id;
                this.width = width;
                this.height = height;
                this.prev_imgs = prev_imgs;
            }
        }
        public class class_categories
        {
            public int id;
            public string name;
            public string supercategory;

            public class_categories()
            {
                id = 1;
                name = "smoke";
                supercategory = "";
            }
        }
        public class class_annotations
        {
            public int id;
            public int category_id;
            public int image_id;
            public int[] bbox;
            public int iscrowd;
            public int area;

            public class_annotations()
            {

            }

            public class_annotations(int id, int image_id, int[] bbox)
            {
                this.id = id;
                this.category_id = 1;
                this.image_id = image_id;
                this.bbox = bbox;
                this.iscrowd = 0;
                this.area = bbox[2] * bbox[3];
            }
        }

        public List<class_images> images;
        public List<class_categories> categories;
        public List<class_annotations> annotations;

        public static Bitmap ResizeImage(Bitmap originalImage, int newWidth, int newHeight)
        {
            // Создаем новый Bitmap с новыми размерами
            Bitmap resizedImage = new Bitmap(newWidth, newHeight, originalImage.PixelFormat);

            // Настройка качества рендеринга
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                // Устанавливаем высокое качество интерполяции
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;

                // Рисуем исходное изображение на новом Bitmap с новыми размерами
                graphics.DrawImage(
                    originalImage,
                    new Rectangle(0, 0, newWidth, newHeight),  // Целевая область
                    new Rectangle(0, 0, originalImage.Width, originalImage.Height),  // Исходная область
                    GraphicsUnit.Pixel
                );
            }
            return resizedImage;
        }

        public COCO(List<ObjectManager> manager, List<ExtendBitmap> frames, int w, int h, string type_set, ProgressBar bar)
        {
            images = new List<class_images>();
            categories = new List<class_categories>
            {
                new class_categories()
            };
            annotations = new List<class_annotations>();

            // собираем аннотации со всех объектов и сортируем по кадрам
            ObjectManager common_manager = new ObjectManager();
            for (int i = 0; i < manager.Count; i++)
            {
                for (int j = 0; j < manager[i].compiled_labels.Count; j++)
                {
                    common_manager.compiled_labels.Add(manager[i].compiled_labels[j]);
                }
            }
            common_manager.compiled_labels = common_manager.compiled_labels.OrderBy(f => f.frame).ToList();

            List<string> prevs = new List<string>();
            bar.Value = 0;
            bar.Maximum = frames.Count;
            for (int i = 0; i < frames.Count; i++)
            {
                bool was_found = false;
                bool was_found_all = false;

                for (int j = 0; j < common_manager.compiled_labels.Count; j++)
                {
                    // если это первая аннотация или кадр в аннотациях точно сменился
                    if (j == 0 || common_manager.compiled_labels[j - 1].frame != common_manager.compiled_labels[j].frame)
                    {
                        was_found = false;
                    }

                    // если нашлась запись для текущего кадра и это произошло в первый раз
                    if (common_manager.compiled_labels[j].frame == i && was_found == false)
                    {
                        // картинки
                        var temp_frame = ResizeImage(frames[i].bitmap, (int)W_goal, (int)H_goal);
                        temp_frame.Save(@"dataset-" + type_set + "/" + type_set + "2017/" + frames[i].file_name, ImageFormat.Jpeg);

                        // images
                        string[] prevs_now = new string[prevs.Count];
                        prevs.CopyTo(prevs_now);
                        images.Add(new class_images(frames[i].file_name, images.Count, (int)W_goal, (int)H_goal, prevs_now));
                        prevs.Add(frames[i].file_name);
                        //MessageBox.Show(prevs.Count + " " + prevs.ToString());

                        was_found = true;
                        was_found_all = true;
                    }

                    // если это произошло в первый и последующие разы
                    if (common_manager.compiled_labels[j].frame == i && was_found == true)
                    {
                        // annotations
                        annotations.Add(new class_annotations(
                            annotations.Count,
                            images.Count - 1,
                            new int[4] {
                                    (int)(W_goal * common_manager.compiled_labels[j].x),
                                    (int)(H_goal * common_manager.compiled_labels[j].y),
                                    (int)(W_goal * common_manager.compiled_labels[j].w),
                                    (int)(H_goal * common_manager.compiled_labels[j].h)
                            }));
                    }


                }

                bar.Value = i;
                if (!was_found_all)
                {
                    prevs = new List<string>();
                }
            }
            bar.Value = 0;
        }
    }
}
