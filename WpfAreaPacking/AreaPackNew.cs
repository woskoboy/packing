using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfAreaPacking
{
    public class AreaPackerNew
    {
        private List<Box> _boxes;
        private Node rootNode;
        private readonly MainWindow _mainWindow;
        private readonly Canvas _canvas;
        private List<List<Box>> _allColumns  = new List<List<Box>>();

        private float _areaLength; 
        private float _areaHeight; 
        
        #region const
        
        //const float BOX_MARGIN = 0; const float AREA_MARGIN = 0;
        const float SCALE = 100;
        const float BOX_MARGIN = 500/SCALE; 
        const float AREA_MARGIN = 500/SCALE;
        private bool isInvertX; 
        private bool isInvertY;
        

        #endregion

        public AreaPackerNew(Canvas canvas, MainWindow mainWindow)
        {
            #region Controls

            _mainWindow = mainWindow;
            _canvas = canvas;

            _areaLength = int.TryParse(_mainWindow.AreaLength.Text, out int valLen) ? valLen : 0;
            _areaHeight = int.TryParse(_mainWindow.AreaHeight.Text, out int valHeight) ? valHeight : 0;
            
            // directions
            isInvertX = _mainWindow.CheckBoxInvertX.IsChecked ?? false;
            isInvertY = _mainWindow.CheckBoxInvertY.IsChecked ?? false;

            #endregion
            
            // Prepare Slabs
            int pp = 0;
            var slabs = JsonConvert.DeserializeObject<IEnumerable<Slab>>(JsonData.Json).ToList();
            _boxes = slabs.Select(s=> new Box { pp = ++pp, Id = s.WRHS_OBJ_ID, height = s.WIDTH, length = s.LENGTH, Slab = s}).ToList();
            SortAndScaleBoxes();
            
            /*_boxes = new List<Box>
            {
                new () {pp = 1, length = 150, height = 50},
                new () {pp = 2, length = 200, height = 50},
                new () {pp = 3, length = 150, height = 50},
                new () {pp = 4, length = 200, height = 50},
                new () {pp = 5, length = 130, height = 50},
                new () {pp = 6, length = 130, height = 50},
                new () {pp = 7, length = 100, height = 50},
                new () {pp = 8, length = 100, height = 50},
            };*/

            InitArea();
            
            Pack();
            Display();
        }

        #region  privates
        
        private void Pack()
        {
            AddColumn(_boxes, 0, 0);
            if (_mainWindow.CheckBoxShift.IsChecked ?? false)
            {
                ShiftEachColumn();
            }
        }

        #region init

        private void SortAndScaleBoxes()
        {
            foreach (var b in _boxes)
            {
                b.height = b.height / SCALE + BOX_MARGIN;
                b.length = b.length / SCALE + BOX_MARGIN;
                b.volume = b.length * b.height;
            }
            // Sort 
            _boxes = (_mainWindow.CheckBoxSort.IsChecked ?? false) 
                ? _boxes.OrderByDescending(x => x.volume).ThenBy(x=> x.pp).ToList() 
                : _boxes.ToList();
        }

        private void InitArea()
        {
            // area root node container, margins and scale
            rootNode = new Node {
                pos_x = 0, pos_y = 0,
                length = _areaLength / SCALE - AREA_MARGIN,
                height = _areaHeight / SCALE - AREA_MARGIN
            };
        }

        #endregion

        #region Pack
        
        private void AddColumn(List<Box> boxes, float startPositionX, float _startPositionY)
        {
            var column = new List<Box>();
            var nextColumn = new List<Box>();
            var startPositionY = _startPositionY;

            foreach (var box in boxes)
            {
                var columnSumHeight = column.Sum(b => b.height);
                if (columnSumHeight + box.height <= rootNode.height)
                {
                    box.position = new Node
                    {
                        pos_x = startPositionX, length = box.length, 
                        pos_y = startPositionY, height = box.height 
                    };
                    column.Add(box);
                    startPositionY += box.height;
                }
                else
                {
                    if (column.Count > 0 || _allColumns.Count > 0)
                    {
                        var maxLengthColumns = _allColumns.Count > 0 
                            ? _allColumns.Sum(c => c.Max(b => b.length))
                            : column.Max(b => b.length);
                        
                        if (maxLengthColumns + box.length <= rootNode.length)
                        {
                            nextColumn.Add(box);
                        }
                    }
                }
            }
            _allColumns.Add(column);
            if (nextColumn.Count > 0)
            {
                var maxLengthColumns = _allColumns.Sum(c => c.Max(b => b.length));
                AddColumn(nextColumn, maxLengthColumns, 0); 
            }
        }

        #region shift

        private void ShiftEachColumn()
        {
            for (int i = 0; i <= _allColumns.Count-1; i++)
            {
                var shiftedColumn = ShiftColumnElements(_allColumns[i]);
                _allColumns[i] = shiftedColumn;
            }
        }

        /*private List<Box> ShiftElements(List<Box> column)
        {
            var last = column.Last();
            for (int i = column.Count - 2; i >= 0; i--)
            {
                column[i + 1] = column[i];
            }
            column[0] = last;
            return column;
        }*/
        
        /// <summary>
        /// Циклический сдвиг элементов коллекции от начала к концу
        /// </summary>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        private List<Box> ShiftColumnElements(List<Box> sourceColumn)
        {
            var targetColumn = new Box[sourceColumn.Count];
            
            var lastPosition = sourceColumn.Last().position;
            var firstPosition = sourceColumn.First().position;
            
            targetColumn[0] = sourceColumn.Last();
            targetColumn[0].position = firstPosition;
            
            for (int j=1; j <= sourceColumn.Count-1; j++)
            {
                var tmp = sourceColumn[j];
                sourceColumn[j - 1].position = tmp.position;
                targetColumn[j] = sourceColumn[j - 1];
            }
            
            targetColumn[^1].position = lastPosition;
            
            CheckDistanse(targetColumn);
            
            return targetColumn.ToList();
        }
        
        /// <summary>
        /// После сдвига из-за разной ширины слябов могут сместится отступы между слябами,
        /// поэтому приводим отступы к заданному значению
        /// </summary>
        /// <param name="boxes"></param>
        private void CheckDistanse(Box[] boxes)
        {
            for (var i=0; i <= boxes.Length-2; i++)
            {
                var next = boxes[i+1];
                var diffHeight = next.position.pos_y - (boxes[i].position.pos_y +  boxes[i].height);
                if (diffHeight > 0)
                {
                    next.position.pos_y -= diffHeight;
                }
            }
        }
        
        /*private List<Box> ShiftElements(List<Box> column)
        {
            var bArr = new Box[column.Count];
            
            // меняем индексацию
            var indexes = new int[column.Count];
            indexes[0] =  column.Count;
            for (int i=1;  i <= column.Count-1; i++)
            {
                indexes[i] =  i;
            }

            int j = 0;
            var lastPosition = column.Last().position;
            foreach (var index in indexes)
            {
                var tmp = column[j];
                column[index - 1].position = tmp.position;
                bArr[j] = column[index - 1];
                j++;
            }
            bArr[^1].position = lastPosition;
            return bArr.ToList();
        }*/

        #endregion
        
        #endregion

        #region  Display
        
        private void Display()
        {
            _canvas.Children.Clear();
            _mainWindow.tbInfo.Text = "";

            _canvas.Width = rootNode.length + AREA_MARGIN;
            _canvas.Height = rootNode.height  + AREA_MARGIN; 
            
            var areaRectangle = new Rectangle 
            {
                Height =  _canvas.Height, 
                Width =  _canvas.Width,
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.Aquamarine)
            };
            CheckDirectionAndSetPosition(areaRectangle, 0, 0);
            
            foreach (var column in _allColumns)
            foreach (var box in column)  //foreach (var box in _boxes)
            {
                if (box.position == null)
                {
                    _mainWindow.tbInfo.Text += box.pp + "; ";
                    continue;
                }

                var boxPosX = box.position.pos_x + BOX_MARGIN;
                var boxPosY = box.position.pos_y + BOX_MARGIN;
                //var boxLength = box.length - BOX_MARGIN; //var boxHeight = box.height - BOX_MARGIN;
                
                var slab = box.Slab;
                slab.X_COORD = (int)boxPosX;
                slab.Y_COORD = (int)boxPosY;

                // rect
                var rectangle = new Rectangle 
                {
                    //Height = (int)boxHeight, Width = (int)boxLength,
                    Height = slab.WIDTH/SCALE, Width = slab.LENGTH/SCALE,
                    Fill = new SolidColorBrush(Colors.LightSlateGray), 
                    Stroke = new SolidColorBrush(Colors.Black)
                };
                CheckDirectionAndSetPosition(rectangle, slab.X_COORD, slab.Y_COORD);
                //CheckDirectionAndSetPosition(rectangle, boxPosX, boxPosY);

                // tb
                var textBlock = new TextBlock
                {
                    //Text = $"{box.pp} - {box.Id} {boxHeight/10}", // {box.length}x{box.height}  ({boxPosX},{boxPosY})
                    Text = $"{box.pp} - {box.Id} { slab.WIDTH/SCALE}",
                    Foreground = new SolidColorBrush(Colors.Black)
                };
                CheckDirectionAndSetPosition(textBlock, slab.X_COORD, slab.Y_COORD, 0, 0);
                //CheckDirectionAndSetPosition(textBlock, boxPosX, boxPosY, 0, 0);
            }
        }

        private void CheckDirectionAndSetPosition(UIElement element, double x, double y, double offsetLength = 0, double offsetHeight = 0)
        {
            _canvas.Children.Add(element);
            if (isInvertX) 
                Canvas.SetRight(element, x + offsetLength);
            else
                Canvas.SetLeft(element, x + offsetLength);
            if(isInvertY)
                Canvas.SetTop(element, y + offsetHeight);
            else 
                Canvas.SetBottom(element, y + offsetHeight);
        }
        
        #endregion
        
        #endregion
    }
}
