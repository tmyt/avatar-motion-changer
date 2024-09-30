using UnityEngine;

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Inspector
{
    internal static class RectangleUtils
    {
        public static Rect[] SplitHorizontal(Rect rect, int splitInto, float elementWidth)
        {
            var rects = new Rect[splitInto];
            
            if (elementWidth > 1 && elementWidth < rect.width)
            {
                rect.x += rect.width - elementWidth;
                rect.width = elementWidth;
            }
            
            for (var i = 0; i < splitInto; i++)
            {
                rects[i] = rect;
                rects[i].width /= splitInto;
                if (i > 0)
                {
                    rects[i].x = rects[i - 1].x + rects[i - 1].width;
                }
            }
            
            for (var i = 0; i < splitInto - 1; i++)
            {
                rects[i].width -= 12;
            }
            
            return rects;
        }
    }
}
