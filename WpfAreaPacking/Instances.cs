namespace WpfAreaPacking
{
    public class Slab
    {
        public int WRHS_OBJ_ID { get; set; }
        public int X_COORD { get; set; }
        public int Y_COORD { get; set; }
        public float WIDTH { get; set; }
        public float LENGTH { get; set; }
        
        // public float Z_COORD { get; set; }
         public string DISPLAY_NAME { get; set; }
        // public float? Y_COORD_TEMP { get; set; }
        // public float X_OFFSET { get; set; }
        // public int Y_OFFSET { get; set; }
        // public float Z_OFFSET { get; set; }
        // public string DESCRIPTION { get; set; }
    }

    /// <summary>
    /// Контейнер для <see cref="Box"/>
    /// </summary>
    internal abstract class INode
    {
        public float pos_x;
        public float pos_y;
        public float height;
        public float length;
    }

    internal class Node : INode { 
        public Node rightNode;
        public Node bottomNode;
        public float pos_x;
        public float pos_y;
        public float height;
        public float length;
        public bool isOccupied;
    }

    /// <summary>
    /// Обертка над слябом для его позиционирования в зоне
    /// </summary>
    internal class Box
    {
        public int pp;
        public int Id;
        public float length;
        public float height;
        public float volume;
        public Node position;
        public Slab Slab { get; set; }
    }
}