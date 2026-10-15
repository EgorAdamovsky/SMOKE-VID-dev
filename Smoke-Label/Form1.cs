using Accord.Video.FFMPEG;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Smoke_Label
{
    public partial class MyForm : Form
    {
        public List<Color> colors = new List<Color>() {
            Color.FromArgb(128, 255, 0, 0),
            Color.FromArgb(128, 0, 255, 0),
            Color.FromArgb(128, 0, 0, 255),
            Color.FromArgb(128, 255, 128, 0),
            Color.FromArgb(128, 128, 255, 0),
            Color.FromArgb(128, 255, 0, 128),
            Color.FromArgb(128, 128, 0, 255),
            Color.FromArgb(128, 0, 128, 255),
            Color.FromArgb(128, 0, 255, 128)
        };
        public List<Color> colors_border = new List<Color>() {
            Color.FromArgb(255, 255, 0, 0),
            Color.FromArgb(255, 0, 255, 0),
            Color.FromArgb(255, 0, 0, 255),
            Color.FromArgb(255, 255, 128, 0),
            Color.FromArgb(255, 128, 255, 0),
            Color.FromArgb(255, 255, 0, 128),
            Color.FromArgb(255, 128, 0, 255),
            Color.FromArgb(255, 0, 128, 255),
            Color.FromArgb(255, 0, 255, 128)
        };

        public const float W = 480;
        public const float H = 320;
        public int W_real = -1;
        public int H_real = -1;

        public List<ExtendBitmap> frames = new List<ExtendBitmap>();
        public List<ObjectManager> manager = new List<ObjectManager>();
        public int manager_index = -1;

        private bool _isDrawing = false;
        private Point _startPoint;
        private MyRect _rect;

        public MyForm()
        {
            InitializeComponent();
        }

        // ВЫБРАТЬ ФАЙЛ И ЗАГРУЗИТЬ КАДРЫ
        private void _choosefile_Click(object sender, EventArgs e)
        {
            if (_openfile.ShowDialog() == DialogResult.OK) // если файл выбран
            {
                _pathfile.Text = _openfile.FileName; // обозначить путь на экране
                _loadfile_Click(sender, e); // начать загрузку
            }
        }
        private void _loadfile_Click(object sender, EventArgs e)
        {
            // сброс программы
            frames = new List<ExtendBitmap>(); // сброс кадров
            manager = new List<ObjectManager>(); // сброс меток
            _rect = new MyRect(); // сброс прямоугольника
            manager_index = -1; // сброс итератора меток
            _slider.Value = 0; // обнуление позиции слайдера
            _isDrawing = false; // отрисовка выкл
            W_real = -1; H_real = -1; // сброс размеров
            _list.Items.Clear(); // лчистка меток на экране
            _compiledlist.Items.Clear(); // очистка скомпилированных меток на экране
            _counter.Text = "0"; // сброс отображения кадра текущего
            _len.Text = "0"; // сброс отображения кадров всего

            try
            {
                using (var videoReader = new VideoFileReader()) // начинаем читать файл
                {
                    videoReader.Open(_pathfile.Text); // открываем путь

                    _progress.Maximum = (int)(videoReader.FrameCount - 1); // установка максимума прогресса
                    W_real = videoReader.Width; H_real = videoReader.Height; // получаем реальный размер
                    _ww.Text = "Исходная ширина: " + W_real + " пк."; // записываем его на экран
                    _hh.Text = "Исходная высота: " + H_real + " пк."; // записываем его на экран

                    bool resize = false; // флаг необходимости ресайза для того, чтобы влезло в программу
                    if (W_real > COCO.W_goal || H_real > COCO.H_goal) // если размер кадра слишком велик
                    {
                        resize = true; // пометить, что надо будет ресайзать
                        W_real = (int)COCO.W_goal; H_real = (int)COCO.H_goal; // переопределение размера потом
                    }

                    frames = new List<ExtendBitmap>((int)videoReader.FrameCount); // оптимизация аллокации
                    for (int i = 0; i < videoReader.FrameCount; i++) // проход по всем кадрам
                    {
                        Bitmap frame = videoReader.ReadVideoFrame(); // чтение очередного кадра
                        if (frame != null) // если прочиталось
                        {
                            if (resize) // если надо ресайзать
                            {
                                frame = COCO.ResizeImage(frame, (int)COCO.W_goal, (int)COCO.H_goal); // ресайзаем
                            }
                            frames.Add(new ExtendBitmap(frame)); // добавляем в массив кадров
                        }
                        _progress.Value = i;
                    }

                    _progress.Value = 0; // сброс прогресса
                    _screen.Image = frames[0].bitmap; // показать на экране первый кадр
                    _slider.Value = 0; // слайдер сбросить
                    _slider.Minimum = 0; // слайдер минимум установить
                    _slider.Maximum = frames.Count - 1; // слайдер максимум установить
                    _len.Text = frames.Count - 1 + ""; // максимум установить числовой
                    manager.Add(new ObjectManager(manager.Count)); // один объект у нас есть 100%
                    manager_index = 0; // индекс этого объекта 0
                    UpdateObjects(); // обновить объекты
                    _manager.SelectedIndex = 0; // установить выбранный объект
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                Application.Restart();
            }
        }

        // ПОЛЗУНОК: скролл и ручное изменение
        private void _slider_Scroll(object sender, EventArgs e)
        {
            if (frames.Count > 0)
            {
                // сброс на дефолтные установки и обновление кадра
                _counter.Text = _slider.Value + "";
                _screen.Image = frames[_slider.Value].bitmap;
                _isDrawing = false;
                _rect = new MyRect();

                _x.Text = "0"; _y.Text = "0"; _w.Text = "0"; _h.Text = "0";
                _seq.Checked = true;

                // цикл по исходным меткам
                for (int i = 0; i < manager[manager_index].labels.Count; i++)
                {
                    if (manager[manager_index].labels[i].frame == int.Parse(_counter.Text))
                    {
                        _rect = new MyRect(
                            manager[manager_index].labels[i].x,
                            manager[manager_index].labels[i].y,
                            manager[manager_index].labels[i].w,
                            manager[manager_index].labels[i].h);
                        _x.Text = _rect.x + ""; _y.Text = _rect.y + ""; _w.Text = _rect.w + ""; _h.Text = _rect.h + "";
                        _seq.Checked = manager[manager_index].labels[i].seq;
                        _isDrawing = true;
                        break;
                    }
                }

                // цикл по скомпилированным меткам
                for (int i = 0; i < manager[manager_index].compiled_labels.Count; i++)
                {
                    if (manager[manager_index].compiled_labels[i].frame == int.Parse(_counter.Text))
                    {
                        _rect = new MyRect(
                            manager[manager_index].compiled_labels[i].x,
                            manager[manager_index].compiled_labels[i].y,
                            manager[manager_index].compiled_labels[i].w,
                            manager[manager_index].compiled_labels[i].h);
                        _x.Text = _rect.x + ""; _y.Text = _rect.y + ""; _w.Text = _rect.w + ""; _h.Text = _rect.h + "";
                        _seq.Checked = manager[manager_index].compiled_labels[i].seq;
                        _isDrawing = true;
                        break;
                    }
                }
            }
        }
        private void _counter_TextChanged(object sender, EventArgs e)
        {
            if (frames.Count > 0)
            {
                if (int.TryParse(_counter.Text, out int temp))
                {
                    if (temp >= 0)
                    {
                        _slider.Value = temp >= frames.Count ? frames.Count - 1 : temp;
                        _counter.Text = _slider.Value + "";

                    }
                }
            }
            _slider_Scroll(sender, e);
        }

        // РИСОВАНИЕ
        private void _screen_Paint(object sender, PaintEventArgs e)
        {
            if (_isDrawing)
            {
                using (Brush brush = new SolidBrush(colors[manager_index % colors.Count]))
                {
                    e.Graphics.FillRectangle(brush, W * _rect.x, H * _rect.y, W * _rect.w, H * _rect.h);
                }
                using (Pen pen = new Pen(colors_border[manager_index % colors_border.Count]))
                {
                    e.Graphics.DrawRectangle(pen, W * _rect.x, H * _rect.y, W * _rect.w, H * _rect.h);
                }
            }
        }
        private void _screen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDrawing = true;
                _startPoint = e.Location;
                _rect = new MyRect(_startPoint.X / W, _startPoint.Y / H, 0, 0);
                _x.Text = _startPoint.X / W + "";
                _y.Text = _startPoint.Y / H + "";
            }
        }
        private void _screen_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                int x = Math.Min(_startPoint.X, e.X);
                int y = Math.Min(_startPoint.Y, e.Y);
                int width = Math.Abs(e.X - _startPoint.X);
                int height = Math.Abs(e.Y - _startPoint.Y);
                _rect = new MyRect(
                    x / W,
                    y / H,
                    width / W,
                    height / H);
                _w.Text = width / W + "";
                _h.Text = height / H + "";
                _screen.Invalidate();
            }
        }
        private void _screen_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _isDrawing)
            {
                CustomMouseUp(e.X, e.Y);
            }
        }
        void CustomMouseUp(int _x, int _y)
        {
            if (_isDrawing)
            {
                _isDrawing = false;
                int x = Math.Min(_startPoint.X, _x);
                int y = Math.Min(_startPoint.Y, _y);
                int width = Math.Abs(_x - _startPoint.X);
                int height = Math.Abs(_y - _startPoint.Y);
                _rect = new MyRect(
                    x / W,
                    y / H,
                    width / W,
                    height / H);
            }
        }

        // МЕНЕДЖМЕНТ
        void UpdateList()
        {
            manager[manager_index].labels = manager[manager_index].labels.OrderBy(f => f.frame).ToList();
            manager[manager_index].compiled_labels = manager[manager_index].compiled_labels.OrderBy(f => f.frame).ToList();

            var seen = new HashSet<int>();
            var result = new List<Labels>();
            foreach (var item in manager[manager_index].compiled_labels)
            {
                if (seen.Add(item.frame))
                {
                    result.Add(item);
                }
            }
            manager[manager_index].compiled_labels = result;

            _list.Items.Clear();
            _compiledlist.Items.Clear();
            for (int i = 0; i < manager[manager_index].labels.Count; i++)
            {
                _list.Items.Add(manager[manager_index].labels[i]);
            }
            for (int i = 0; i < manager[manager_index].compiled_labels.Count; i++)
            {
                _compiledlist.Items.Add(manager[manager_index].compiled_labels[i]);
            }
        }
        float Clamp01(float value)
        {
            if (value > 1f)
            {
                return 1f;
            }
            if (value < 0)
            {
                return 0;
            }
            return value;
        }
        private void _add_Click(object sender, EventArgs e)
        {
            if (manager.Count > 0)
            {
                // удалить, если есть дубликат
                for (int i = 0; i < manager[manager_index].labels.Count; i++)
                {
                    if (manager[manager_index].labels[i].frame == int.Parse(_counter.Text))
                    {
                        manager[manager_index].labels.RemoveAt(i);
                        break;
                    }
                }

                // прочитать и обезопасить по границам координаты
                float _x_ = Clamp01(float.Parse(_x.Text));
                float _y_ = Clamp01(float.Parse(_y.Text));
                float _w_ = Clamp01(float.Parse(_w.Text));
                float _h_ = Clamp01(float.Parse(_h.Text));
                float xw = _x_ + _w_;
                float yh = _y_ + _h_;
                if (xw > 1f)
                {
                    _w_ -= xw - 1f;
                }
                if (yh > 1f)
                {
                    _h_ -= yh - 1f;
                }

                manager[manager_index].labels.Add(new Labels(_slider.Value, _x_, _y_, _w_, _h_, _seq.Checked));
                UpdateList();
            }
        }
        private void _del_Click(object sender, EventArgs e)
        {
            if (manager.Count > 0)
            {
                manager[manager_index].labels.RemoveAt(_list.SelectedIndex);
                UpdateList();
            }
        }
        private void _clear_Click(object sender, EventArgs e)
        {
            if (manager.Count > 0)
            {
                manager[manager_index].labels.Clear();
                manager[manager_index].compiled_labels.Clear();
                UpdateList();
            }
        }
        private void _compile_Click(object sender, EventArgs e)
        {
            if (manager.Count > 0)
            {
                if (manager[manager_index].labels.Count > 0)
                {
                    UpdateList();
                    List<Labels> temp = new List<Labels>();
                    manager[manager_index].labels[manager[manager_index].labels.Count - 1].seq = false;
                    for (int i = 0; i < manager[manager_index].labels.Count; i++)
                    {
                        if (manager[manager_index].labels[i].seq)
                        {
                            // ОЦЕНКА СЛЕДУЮЩЕЙ КЛЮЧЕВОЙ ТОЧКИ
                            int next = -1;
                            MyRect rect_next = new MyRect();
                            if (manager[manager_index].labels[i + 1] != null)
                            {
                                next = manager[manager_index].labels[i + 1].frame;
                                rect_next.x = manager[manager_index].labels[i + 1].x;
                                rect_next.y = manager[manager_index].labels[i + 1].y;
                                rect_next.w = manager[manager_index].labels[i + 1].w;
                                rect_next.h = manager[manager_index].labels[i + 1].h;
                            }
                            else
                            {
                                next = frames.Count - 1;
                                rect_next.x = manager[manager_index].labels[i].x;
                                rect_next.y = manager[manager_index].labels[i].y;
                                rect_next.w = manager[manager_index].labels[i].w;
                                rect_next.h = manager[manager_index].labels[i].h;
                            }

                            // ИНТЕРПОЛЯЦИЯ
                            for (int j = manager[manager_index].labels[i].frame; j < frames.Count; j++)
                            {
                                if (next == j)
                                {
                                    temp.Add(new Labels(
                                        j, rect_next.x, rect_next.y, rect_next.w, rect_next.h, true));
                                    break;
                                }
                                float t = (j - manager[manager_index].labels[i].frame) / (float)(next - manager[manager_index].labels[i].frame);
                                temp.Add(new Labels(j,
                                    Lerp(manager[manager_index].labels[i].x, rect_next.x, t),
                                    Lerp(manager[manager_index].labels[i].y, rect_next.y, t),
                                    Lerp(manager[manager_index].labels[i].w, rect_next.w, t),
                                    Lerp(manager[manager_index].labels[i].h, rect_next.h, t),
                                    true));
                            }
                        }
                    }
                    manager[manager_index].compiled_labels = temp;
                    UpdateList();
                }
            }
        }

        // ПРОЧЕЕ
        float Lerp(float a, float b, float t)
        {
            float delta = b - a;
            return a + delta * t;
        }

        // МЕНЕДЖЕР ОБЪЕКТОВ
        void UpdateObjects()
        {
            _manager.Items.Clear();
            for (int i = 0; i < manager.Count; i++)
            {
                _manager.Items.Add(manager[i]);
            }
        }
        private void _manager_SelectedIndexChanged(object sender, EventArgs e)
        {
            manager_index = _manager.SelectedIndex;
            UpdateList();
        }
        private void _addobj_Click(object sender, EventArgs e)
        {
            manager.Add(new ObjectManager(manager.Count));
            manager_index = manager.Count - 1;
            UpdateObjects();
            _manager.SelectedIndex = manager_index;
        }
        private void _delobj_Click(object sender, EventArgs e)
        {
            manager.RemoveAt(manager_index);
            manager_index = 0;
            UpdateObjects();
            _manager.SelectedIndex = 0;
        }

        // ВЫВОД
        private void _create_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(@"dataset-" + _trainorval.Text))
            {
                Directory.Delete(@"dataset-" + _trainorval.Text, true);
            }
            Directory.CreateDirectory(@"dataset-" + _trainorval.Text + @"/annotations");
            Directory.CreateDirectory(@"dataset-" + _trainorval.Text + @"/train2017");
            Directory.CreateDirectory(@"dataset-" + _trainorval.Text + @"/val2017");
            COCO coco = new COCO(manager, frames, W_real, H_real, _trainorval.Text, _progress);
            string json = JsonConvert.SerializeObject(coco);
            File.WriteAllText(_trainorval.Text == "train" ?
                @"dataset-" + _trainorval.Text + @"/annotations/instances_train2017.json" :
                @"dataset-" + _trainorval.Text + @"/annotations/instances_val2017.json",
                json);
            frames[0].bitmap.Save(@"dataset-" + _trainorval.Text + "/" + frames[0].file_name, ImageFormat.Jpeg);
            MessageBox.Show("Датасет " + _trainorval.Text + " создан!");
        }

        // ЛИСТ
        private void _list_DoubleClick(object sender, EventArgs e)
        {
            if (_list.Items.Count > 0)
            {
                var temp = manager[manager_index].labels[_list.SelectedIndex];
                _x.Text = temp.x + "";
                _y.Text = temp.y + "";
                _w.Text = temp.w + "";
                _h.Text = temp.h + "";
                _seq.Checked = temp.seq;
            }
        }
    }

}
