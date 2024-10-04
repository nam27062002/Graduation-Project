using UnityEngine;

namespace Alkawa
{
    public static class ColorExtensions
    {
        public static string ToHexStringRGB(this Color _Color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", FloatToHexInt(_Color.r), FloatToHexInt(_Color.g), FloatToHexInt(_Color.b));
        }
        
        public static uint ToHexUint(this Color c) => ((uint)(c.a * 255) << 24) | ((uint)(c.r * 255) << 16) | ((uint)(c.g * 255) << 8) | (uint)(c.b * 255);
        public static Color FromHexToRGBA(uint hex) => new Color(((hex >> 16) & 0xff) / 255f, ((hex >>  8) & 0xff) / 255f, (hex & 0xff) / 255f, ((hex >> 24) & 0xff) / 255f);

        public static string ToHexStringRGBA(this Color _Color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", FloatToHexInt(_Color.r), FloatToHexInt(_Color.g), FloatToHexInt(_Color.b), FloatToHexInt(_Color.a));
        }

        private static int FloatToHexInt(float _float)
        {
            return (int)(_float * 255);
        }

        public static Color SetAlpha(this Color _this, float _alpha)
        {
            return new Color(_this.r, _this.g, _this.b, _alpha);
        }

        public static Color SetRGB(this Color _this, byte _r, byte _g, byte _b)
        {
            return new Color(_r / 255.0f, _g / 255.0f, _b / 255.0f);
        }

        public static Color SetRGB(this Color _this, float _r, float _g, float _b)
        {
            return new Color(_r, _g, _b, _this.a);
        }

        public static Color SetRGB(this Color _this, Color _color)
        {
            return new Color(_color.r, _color.g, _color.b, _this.a);
        }

        public static int RInt255(this Color _this)
        {
            return (int)(_this.r * 255);
        }
        public static int GInt255(this Color _this)
        {
            return (int)(_this.g * 255);
        }
        public static int BInt255(this Color _this)
        {
            return (int)(_this.b * 255);
        }

        public static Color Complementary(this Color _this)
        {
            float H, S, V;
            Color.RGBToHSV(_this, out H, out S, out V);
            H = (H + 0.5f) % 1f;

            return Color.HSVToRGB(H, S, V).Bright();
        }

        public static Color MonochromeVariation(this Color _this)
        {
            float H, S, V;
            Color.RGBToHSV(_this, out H, out S, out V);
            if (V > 0.5f)
                V -= 0.2f;
            else
                V += 0.2f;
            return Color.HSVToRGB(H, S, V);
        }

        public static Color Darker(this Color _this, float amount = 0.2f)
        {
            float H, S, V;
            Color.RGBToHSV(_this, out H, out S, out V);
            V -= amount;
            return Color.HSVToRGB(H, S, V);
        }

        public static Color Lighter(this Color _this, float amount = 0.2f)
        {
            float H, S, V;
            Color.RGBToHSV(_this, out H, out S, out V);
            V += amount;
            return Color.HSVToRGB(H, S, V);
        }

        public static Color Saturate(this Color _this)
        {
            float H, S, V;
            Color.RGBToHSV(_this, out H, out S, out V);
            S = 1f;
            return Color.HSVToRGB(H, S, V);
        }

        public static Color Bright(this Color _this)
        {
            float H, S, V;
            Color.RGBToHSV(_this, out H, out S, out V);
            V = 1f;
            return Color.HSVToRGB(H, S, V);
        }
    }
}
