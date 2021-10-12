using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace WpfAreaPacking
{
    public class AreaPacker
    {
        // private List<Box> _boxes;
        // private Node rootNode;
        // private readonly MainWindow _mainWindow;
        // private readonly Canvas _canvas;
        //
        // #region const
        //
        // const float BOX_MARGIN = 5;
        // const float AREA_MARGIN = 5;
        // const int SCALE = 100;
        // private bool isInvertX; 
        // private bool isInvertY;
        //
        // #endregion
        //
        // public AreaPacker(Canvas canvas, MainWindow mainWindow)
        // {
        //     _mainWindow = mainWindow;
        //     _canvas = canvas;
        //     
        //     var slabs = JsonConvert.DeserializeObject<IEnumerable<Slab>>(JsonData.Json).ToList();
        //     
        //     slabs.ForEach(s=> s.WIDTH = s.WIDTH/SCALE);
        //     slabs.ForEach(s=> s.LENGTH = s.LENGTH/SCALE);
        //     
        //     var slabBoxes = slabs.Select(s=> new Box{ pp = s.WRHS_OBJ_ID, height = s.WIDTH, length = s.LENGTH}).ToList();
        //     _boxes = new List<Box>
        //     {
        //         new () {pp = 1, length = 150, height = 100},
        //         new () {pp = 2, length = 200, height = 100},
        //         new () {pp = 3, length = 150, height = 100},
        //         new () {pp = 4, length = 200, height = 100},
        //         new () {pp = 5, length = 130, height = 100},
        //         new () {pp = 6, length = 200, height = 100},
        //     };
        //
        //     //_boxes = slabBoxes;
        //     
        //     // root node
        //     rootNode = new Node
        //     {
        //         length = int.TryParse(mainWindow.AreaLength.Text, out int valLen) ? valLen : 0, 
        //         height = int.TryParse(mainWindow.AreaHeight.Text, out int valHeight) ? valHeight : 0, 
        //         pos_x = 0, pos_y = 0
        //     };
        //
        //     // margins
        //     _boxes.ForEach(b=> b.height += BOX_MARGIN);
        //     _boxes.ForEach(b=> b.length += BOX_MARGIN);
        //     rootNode.height -= AREA_MARGIN;
        //     rootNode.length -= AREA_MARGIN;
        //     
        //     // Sort 
        //     _boxes.ForEach(x => x.volume = (x.length * x.height));
        //     _boxes = (_mainWindow.CheckBoxSort.IsChecked ?? false) 
        //         ? _boxes.OrderByDescending(x => x.volume).ThenBy(x=> x.pp).ToList() 
        //         : _boxes.OrderBy(x=> x.pp).ToList();
        //     
        //     // directions
        //     isInvertX = _mainWindow.CheckBoxInvertX.IsChecked ?? false;
        //     isInvertY = _mainWindow.CheckBoxInvertY.IsChecked ?? false;
        //     
        //     Pack();
        //     Display();
        // }
        //
        // #region  Pack
        //
        // private void Pack()
        // {
        //     foreach (var box in _boxes)
        //     {
        //         var node = FindNode(rootNode, box.height, box.length);
        //         if (node != null)
        //         {
        //             box.position = SplitNode(node, box.height, box.length);
        //         }
        //     }
        // }
        //
        // private Node FindNode(Node rootNode, double boxHeight, double boxLength)
        // {
        //     if (rootNode.isOccupied) 
        //     {
        //         var nextNode = FindNode(rootNode.bottomNode, boxHeight, boxLength);
        //         if (nextNode == null)
        //         {
        //             // var firstNode = _allNodes.LastOrDefault(); 
        //             //_allNodes.Clear();
        //             //if(firstNode != null) nextNode = FindNode(firstNode.rightNode, boxHeight, boxLength);
        //             
        //             nextNode = FindNode(rootNode.rightNode, boxHeight, boxLength);
        //         }
        //         return nextNode;
        //     }
        //     else if (boxHeight <= rootNode.height && boxLength <= rootNode.length)
        //     {
        //         return rootNode;
        //     }
        //     else 
        //     {
        //         return null;
        //     }
        // }
        //
        // private Node SplitNode(Node node, float boxHeight, float boxLength) 
        // {
        //     node.isOccupied = true;
        //     node.bottomNode = new Node
        //     {
        //         pos_x = node.pos_x, 
        //         pos_y = node.pos_y + boxHeight,
        //         length = node.length, 
        //         height = node.height - boxHeight 
        //     };
        //     node.rightNode = new Node
        //     {
        //         pos_x = node.pos_x + boxLength, 
        //         pos_y = node.pos_y, 
        //         length = node.length - boxLength, 
        //         height = boxHeight
        //     };
        //     /*
        //     node.bottomNode = new Node
        //     {
        //         pos_x = node.pos_x, 
        //         pos_y = node.pos_y + boxHeight,
        //         length = node.length, 
        //         height = node.height - boxHeight 
        //     };
        //     node.rightNode = new Node
        //     {
        //         pos_x = node.pos_x + boxLength, 
        //         pos_y = node.pos_y, 
        //         length = node.length - boxLength, 
        //         height = boxHeight
        //     };
        //     */
        //     return node;
        // }
        //
        // #endregion
        //
        // #region  Display
        //
        // private void Display()
        // {
        //     _canvas.Children.Clear();
        //     _mainWindow.tbInfo.Text = "";
        //     _canvas.Width = rootNode.length + AREA_MARGIN;
        //     _canvas.Height = rootNode.height + AREA_MARGIN;
        //     foreach (var box in _boxes)
        //     {
        //         if (box.position == null)
        //         {
        //             _mainWindow.tbInfo.Text += box.pp + "; ";
        //             continue;
        //         }
        //
        //         var boxPosX = box.position.pos_x + BOX_MARGIN;
        //         var boxPosY = box.position.pos_y + BOX_MARGIN;
        //         var boxLength = box.length - BOX_MARGIN;
        //         var boxHeight = box.height - BOX_MARGIN;
        //
        //         Console.WriteLine($"{box.pp} {boxPosX};{boxPosY};{boxLength};{boxHeight}");
        //         
        //         // rect
        //         var rectangle = new Rectangle
        //         {
        //             Height = (int)boxHeight,
        //             Width = (int)boxLength
        //         };
        //         rectangle.Fill = new SolidColorBrush(boxLength >=200 ? Colors.LightCoral : Colors.LightSlateGray);
        //         rectangle.Stroke = new SolidColorBrush(Colors.Black);
        //         _canvas.Children.Add(rectangle);
        //         CheckDirectionAndSetPosition(rectangle, boxPosX, boxPosY);
        //
        //         // tb
        //         var textBlock = new TextBlock();
        //         textBlock.Text = $"{box.pp}({boxPosX},{boxPosY})";
        //         textBlock.Foreground = new SolidColorBrush(Colors.White);
        //         _canvas.Children.Add(textBlock);
        //         CheckDirectionAndSetPosition(textBlock, boxPosX, boxPosY, boxLength/2, boxHeight/2);
        //     }
        // }
        //
        // private void CheckDirectionAndSetPosition(UIElement element, double x, double y, double offsetLength = 0, double offsetHeight = 0)
        // {
        //     if (isInvertX) 
        //         Canvas.SetRight(element, x + offsetLength);
        //     else
        //         Canvas.SetLeft(element, x + offsetLength);
        //     if(isInvertY)
        //         Canvas.SetTop(element, y + offsetHeight);
        //     else 
        //         Canvas.SetBottom(element, y + offsetHeight);
        // }
        //
        // #endregion
    }
}
