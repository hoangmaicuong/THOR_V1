using System.Drawing;

namespace THOR_V1.Module.BusinessObjects
{
    public interface IPictureItem
    {
        string ID { get; }
        Image Image { get; }
        string Text { get; }
    }
}
