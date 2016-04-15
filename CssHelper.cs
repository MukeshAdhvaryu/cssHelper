using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//classes & structs
namespace WebHelper
{
    /// <summary>
    /// This is an extension class designed to assist c# class to Css.
    /// Enum values set in design classes get parsed to strings to form valid css.
    /// </summary>
    public static class CssHelper
    {
        public static string newline = System.Environment.NewLine;

        public static string GetName(this Enum value)
        {
            return value.ToString();
        }
        public static T GetValue<T>(string value)
        {
            if (typeof(T).BaseType == typeof(Enum))
            {
                try
                {
                    return (T)Enum.Parse(typeof(T), value, true);
                }
                catch { }
            }
            return default(T);
        }
        public static T GetValue<T>(int value)
        {
            if (typeof(T).BaseType == typeof(Enum))
            {
                try
                {
                    return (T)(object)value;
                }
                catch { }
            }
            return default(T);
        }

        public static T GetValue<T>(string value, out bool success)
        {
            success = false;
            if (typeof(T).BaseType == typeof(Enum))
            {
                try
                {
                    T val = (T)Enum.Parse(typeof(T), value, true);
                    success = true;
                    return val;
                }
                catch { }
            }
            return default(T);
        }
        public static T GetValue<T>(this Enum value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch { }
            return default(T);
        }
        public static T GetConcateValue<T>(this Enum nameprefix, Enum namesuffix)
        {
            string s = GetName(nameprefix) + GetName(namesuffix);
            if (typeof(T).BaseType == typeof(Enum))
            {
                return (T)Enum.Parse(typeof(T), s, true);
            }
            return default(T);
        }
        public static String GetConcateName(this Enum prefix, Enum suffix)
        {
            return prefix.GetName() + suffix.GetName();
        }
        public static direction Invert(this direction value)
        {
            return GetValue<direction>(-(int)value);
        }
        public static string CssValue(this Enum value)
        {
            return value.GetName().Replace('_', '-');
        }
        public static string CssSyntax(this Enum value)
        {
            var name = value.GetType().Name.Replace('_', '-') + ":";
            return name + value.CssValue() + ";";
        }
        public static string CssName<T>()
        {
            return typeof(T).Name.Replace('_', '-') + ":";
        }
    }

    #region interfaces
    public interface ICss
    {
        string ToCss();
    }
    public interface IUnit:ICloneable, ICss
    {

    }
    public interface IGradient : ICss
    {
        RGB[] colors { get; set; }
        bool repeat { get; set; }
        gradientType type { get; }
    }
    public interface IBackground : ICss
    {
        bkgOption? value { get; set; }
        IGradient gradient { get; set; }
        RGB? color { get; set; }
        BackgroundSize? size { get; set; }
        BackgroundPosition? position { get; set; }
        background_attachment? attachment { get; set; }
        background_clip? clip { get; set; }
        background_image? imageOption { get; set; }
        background_origin? origin { get; set; }
        background_repeat? repeat { get; set; }
        string imageurl { get; set; }
    }
    #endregion

    #region classes
    /// <summary>
    /// Sets all the background properties in one declaration
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Background : IBackground
    {
        #region properties
        public virtual bkgOption? value { get; set; }
        public virtual Gradient gradient { get; set; }
        public virtual RGB? color { get; set; }
        public virtual BackgroundSize? size { get; set; }
        public virtual BackgroundPosition? position { get; set; }
        public virtual  background_attachment? attachment { get; set; }
        public virtual  background_clip? clip { get; set; }
        public virtual  background_image? imageOption { get; set; }
        public virtual  background_origin? origin { get; set; }
        public virtual  background_repeat? repeat { get; set; }
        public virtual string imageurl { get; set; }
        #endregion

        #region constructors
        public Background(RGB? color = null, Gradient gradient = null, bkgOption? option = null,
            BackgroundSize? size = null, BackgroundPosition? position = null,
            background_attachment? attachment = null, background_clip? clip = null, background_image? imageOption = null,
            background_origin? origin = null, background_repeat? repaeat = null,  string imageurl = null)
        {
            this.value = option;
            this.gradient = gradient;
            this.color = color;
            this.size = size;
            this.position = position;
            this.attachment = attachment;
            this.clip = clip;
            this.imageOption = imageOption;
            this.origin = origin;
            this.repeat = repaeat;
            this.imageurl = imageurl;
        }
        #endregion

        #region methods
        public string ToCss()
        {
            StringBuilder sb = new StringBuilder(100);
            if (value != null)
            {
                sb.Append("background:" + value.Value.CssValue() + ";");
                sb.Append(CssHelper.newline);
            }
            if (color != null)
            {
                sb.Append("background-color:" + color.ToString() + ";");
                sb.Append(CssHelper.newline);
            }
            var s = (gradient != null) ? gradient.ToCss() + CssHelper.newline : null;
            sb.Append(s);

           
            if (size != null)
            {
                sb.Append(size.ToString());
                sb.Append(CssHelper.newline);
            }
            if (position != null)
            {
                sb.Append(position.Value.ToString());
                sb.Append(CssHelper.newline);
            }
            if (attachment != null)
            {
                sb.Append(attachment.Value.CssSyntax());
                sb.Append(CssHelper.newline);
            }
            if (clip != null)
            {
                sb.Append(clip.Value.CssSyntax());
                sb.Append(CssHelper.newline);
            }
            if (origin != null)
            {
                sb.Append(origin.Value.CssSyntax());
                sb.Append(CssHelper.newline);
            }
            if (repeat != null)
            {
                sb.Append(repeat.Value.CssSyntax());
                sb.Append(CssHelper.newline);
            }
            if (imageOption != null)
            {
                sb.Append(imageOption.Value.CssSyntax());
                sb.Append(CssHelper.newline);
            }
            if (!string.IsNullOrEmpty(imageurl))
            {
                sb.Append(CssHelper.CssName<background_image>() +
                    "url('" + imageurl + "')" + ";");
                sb.Append(CssHelper.newline);
            }
           
            return sb.ToString();
        }
        public override string ToString()
        {
            return ToCss();
        }
        #endregion

        public static implicit operator string(Background value)
        {
            return value.ToString();
        }
        #region interface declaration
        IGradient IBackground.gradient
        {
            get
            {
                return this.gradient;
            }
            set
            {
                if (value is Gradient)
                    this.gradient = value as Gradient;
            }
        }
        #endregion
    }
  
    /// <summary>
    /// Represents Css Border 
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Border : ICss
    {
        public Border(BdrWidth width = null, border_style? style = null,
            RGB? color = null, direction? type = null) { }

        #region properties
        public BdrWidth width { get; set; }
        public border_style? style { get; set; }
        public RGB? color { get; set; }
        public direction? type { get; set; }
        #endregion

        #region methods
        public string ToCss()
        {
            string s = "border{0}:";
            s = string.Format(s, ((type != null) ? type.GetName() : ""));
            if (width != null) s += width.ToString() + " ";
            if (style != null) s += style.Value.GetName();
            if (color != null) s += color.ToString() + ";";
            return s;
        }
        public override string ToString()
        {
            return ToCss();
        }
        #endregion
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class CssImage : ICss
    {
        public CssImage(string url=null, image_orientation? orientation=null, image_rendering? rendering=null)
        {
            this.url = url;
            this.orientation = orientation;
            this.rendering = rendering;
        }

        public virtual string url { get; set; }
        public virtual image_orientation? orientation { get; set; }
        public virtual image_rendering? rendering { get; set; }
        public Unit width { get; set; }
        public Unit height { get; set; }
        public string ToCss()
        {
            throw new NotImplementedException();
        }
    }
  
    // gradients
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class AngularGradient : Gradient
    {
        #region constructors
        public AngularGradient(bool repeat = false, bool asImage=false,
            string imageUrl=null, int angle = 90, params RGB[] colors)
            : base(repeat: repeat,asImage:asImage, imageUrl:imageUrl, colors: colors)
        {
            this.angle = angle;
        }

        public AngularGradient(params RGB[] colors)
            : base(colors: colors)
        {
            this.colors = colors;
        }
        #endregion

        #region properties
        public virtual int? angle { get; set; }
        protected override gradientType type
        {
            get { return gradientType.linear_gradient; }
        }
        #endregion

        public override string ToCss()
        {
            var a = ((angle ?? 90).ToString() + "deg,");

            var s = base.parsedValue();
            StringBuilder sb = new StringBuilder(5);
            sb.Append(string.Format(s, "-webkit-", "", a));
            sb.Append(CssHelper.newline);
            sb.Append(string.Format(s, "-o-", "", a));
            sb.Append(CssHelper.newline);

            sb.Append(string.Format(s, "-moz-", "", a));
            sb.Append(CssHelper.newline);

            sb.Append(string.Format(s, "", "", a));
            sb.Append(CssHelper.newline);
            return sb.ToString();
        }
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class LinearGradient : Gradient
    {
        #region constructors
        public LinearGradient(bool repeat = false, bool asImage = false,
            string imageUrl = null, RadialPos position1 = null,
            RadialPos position2 = null, params RGB[] colors)
            : base(repeat: repeat,asImage:asImage,imageUrl:imageUrl, colors: colors)
        {
            this.position1 = position1;
            this.position2 = position2;
        }

        public LinearGradient(params RGB[] colors)
            : base(colors: colors)
        {
        }

        #endregion

        #region properties
        public virtual RadialPos position1 { get; set; }
        public virtual RadialPos position2 { get; set; }
        protected override gradientType type
        {
            get { return gradientType.linear_gradient; }
        }
        #endregion

        #region methods
        public override string ToCss()
        {
            var s =  format();
            var std =  format(browser.standard);
            var webkit =  format(browser.webkit);
            StringBuilder sb = new StringBuilder(5);

            sb.Append(string.Format(webkit, "-webkit-"));
            sb.Append(CssHelper.newline);

            sb.Append(string.Format(s, "-o-"));
            sb.Append(CssHelper.newline);

            sb.Append(string.Format(s, "-moz-"));
            sb.Append(CssHelper.newline);

            sb.Append(string.Format(std, ""));
            sb.Append(CssHelper.newline);
            return sb.ToString();
        }

        string format(browser b = 0)
        {
            string s = null;
            string first = (b == browser.standard) ? "to " : "";

            if (position1 != null) s = position1[b] + " ";
            if (position2 != null) s += position2[b];

            if (!string.IsNullOrEmpty(s)) s += ",";
            return string.Format(base.parsedValue(),"{0}", first, s);
        }
        #endregion

    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class RadialGradient : Gradient
    {
        public RadialGradient(bool repeat = false, bool asImage = false,
            string imageUrl = null,
            Radial? radial = null, params RGB[] colors)
            : base(repeat: repeat, asImage:asImage,imageUrl:imageUrl,  colors: colors)
        {
            this.Radial = radial ?? new Radial();
        }

        public RadialGradient(params RGB[] colors)
            : base(colors: colors) { }

        protected override gradientType type
        {
            get { return gradientType.radial_gradient; }
        }
        public Radial Radial { get; set; }

        public override string ToCss()
        {
            var prefix = Radial[browser.any] + ",";
            var stdprefix = Radial[browser.standard] + ",";

            var result = base.parsedValue();

            StringBuilder sb = new StringBuilder(5);
            sb.Append(string.Format(result, "-webkit-","",prefix));
            sb.Append(CssHelper.newline);
            sb.Append(string.Format(result, "-o-", "", prefix));
            sb.Append(CssHelper.newline);

            sb.Append(string.Format(result, "-moz-", "", prefix));
            sb.Append(CssHelper.newline);

            sb.Append(string.Format(result, "", "", stdprefix));
            sb.Append(CssHelper.newline);
            return sb.ToString();

        }
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class Gradient : IGradient
    {
        #region constructors
        protected Gradient(bool repeat = false,bool asImage=false, string imageUrl=null, params RGB[] colors)
        {
            this.colors = colors;
            this.asImage = asImage;
            this.imageUrl = imageUrl;
            this.repeat = repeat;
        }
        #endregion

        #region properties
        public virtual RGB[] colors { get; set; }
        public virtual bool asImage { get; set; }
        public virtual string imageUrl { get; set; }

        protected abstract gradientType type { get; }

        public virtual bool repeat { get; set; }
        #endregion

        #region methods
        public abstract string ToCss();
        protected string parsedValue()
        {
            var tag = "background" + (asImage ? "-image" : "")+":";
            tag += ((imageUrl != null) ? "url('" + imageUrl + "')," : "") + "{0}";

            return tag+ (repeat ? "repeating-" : "") + type.CssValue() + "(" + "{1}{2}" +
                    string.Join(",", this.colors) +
                    ")"  + ";";
        }
        public override string ToString()
        {
            return this.ToCss();
        }
        #endregion

        public static implicit operator string(Gradient value)
        {
            return value.ToCss();
        }

        gradientType IGradient.type
        {
            get { return this.type; }
        }
    }
 
    // units
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Unit : ICloneable
    {
        #region variables
        protected int? _value;
        protected unitType _type = unitType.px;
        #endregion

        #region properties
        public virtual int? value
        {
            get { return _value; }
            set { _value = value; }
        }
        public virtual unitType type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string this[browser browser]
        {
            get
            {
                if (_type == unitType.inherit || _type == unitType.initial) 
                    return _type.GetName();

                if (_value == null) return parseToCSS(browser);
                var val = _value.Value.ToString();
                switch (_type)
                {
                    case unitType.prcnt:
                        return val + "%";
                    case unitType.inch:
                        return val + "in";
                    default:
                        return val + _type.GetName();
                }
            }
        }
        protected virtual string parseToCSS(browser browser)
        {
            return null;
        }
        #endregion

        #region constructors
        protected Unit() { }
        public Unit(int unit, unitType type)
        {
            this._value = unit;
            this._type = type;
        }
        public Unit(int unit)
        {
            this._value = unit;
            this._type = unitType.px;
        }
        #endregion

        #region methods
        public virtual void CopyFrom(Unit unit)
        {
            this._value = unit._value;
            this._type = unit._type;
        }
        public Unit Clone()
        {
            var unit = new Unit();
            unit.CopyFrom(this);
            return unit;
        }

        public override string ToString()
        {
            return this[browser.any];
        }
        #endregion

        #region operator overloading
        public static implicit operator string(Unit unit)
        {
            return unit.ToString();
        }
        public static implicit operator int(Unit unit)
        {
            return unit.value ?? 1;
        }
        public static implicit operator Unit(int value)
        {
            return new Unit(value);
        }
        #endregion

        #region interface implementation
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class RadialPos : Unit
    {
        direction? _position;

        #region constructors
        RadialPos() { }
        public RadialPos(int unit, unitType type)
            : base(unit, type)
        {
            this._position = null;
        }
        public RadialPos(int unit)
            : base(unit)
        {
            this._position = null;
        }
        public RadialPos(direction position)
            : base()
        {
            this._position = position;
        }
        #endregion

        #region properties
        public virtual direction? position
        {
            get { return _position; }
            set { _position = value; }
        }
        protected override string parseToCSS(browser browser)
        {
            var pos = _position ?? direction.center;
            switch (browser)
            {
                case browser.webkit:
                    return pos.Invert().GetName();
                default:
                    return pos.GetName();
            }
        }
        #endregion

        #region methods
        public override void CopyFrom(Unit unit)
        {
            base.CopyFrom(unit);
            if (unit is RadialPos)
                _position = (unit as RadialPos)._position;
        }
        public new RadialPos Clone()
        {
            RadialPos rp = new RadialPos();
            rp.CopyFrom(this);
            return rp;
        }

        public override string ToString()
        {
            if (_value == null)
                return (_position == null) ?
                    "center" : _position.Value.GetName();
            else return base.ToString();
        }
        #endregion

        #region operator overloading
        public static implicit operator RadialPos(direction position)
        {
            return new RadialPos(position);
        }
        public static implicit operator direction(RadialPos unit)
        {
            return unit.position ?? direction.center;
        }
        public static implicit operator string(RadialPos unit)
        {
            return unit.ToString();
        }
        public static implicit operator RadialPos(int unit)
        {
            return new RadialPos(unit);
        }
        public static implicit operator int(RadialPos unit)
        {
            return (unit).value ?? 1;
        }
        #endregion
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class BkgPos : Unit
    {
        background_position? _position;

        #region constructors
        BkgPos() { }
        public BkgPos(int unit, unitType type)
            : base(unit, type)
        {
            this._position = null;
        }
        public BkgPos(int unit)
            : base(unit)
        {
            this._position = null;
        }
        public BkgPos(background_position position)
            : base()
        {
            this._position = position;
        }
        #endregion

        #region properties
        public virtual background_position? position
        {
            get { return _position; }
            set { _position = value; }
        }
        protected override string parseToCSS(browser browser)
        {
            if (_position != null)
                return _position.Value.GetName();
            return null;
        }
        #endregion

        #region methods
        public override void CopyFrom(Unit unit)
        {
            base.CopyFrom(unit);
            if (unit is BkgPos)
                _position = (unit as BkgPos)._position;
        }
        public new BkgPos Clone()
        {
            BkgPos rp = new BkgPos();
            rp.CopyFrom(this);
            return rp;
        }

        public override string ToString()
        {
            return this[browser.any];
        }
        #endregion

        #region operator overloading
        public static implicit operator BkgPos(background_position position)
        {
            return new BkgPos(position);
        }
        public static implicit operator background_position(BkgPos unit)
        {
            return unit._position ?? background_position.initial;
        }
        public static implicit operator string(BkgPos unit)
        {
            return unit.ToString();
        }
        public static implicit operator BkgPos(int unit)
        {
            return new BkgPos(unit);
        }
        public static implicit operator int(BkgPos unit)
        {
            return (unit).value ?? 1;
        }
        #endregion
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class BdrWidth : Unit
    {
        border_width? _width;

        #region constructors
        BdrWidth() { }
        public BdrWidth(int unit, unitType type)
            : base(unit, type)
        {
            this._width = null;
        }
        public BdrWidth(int unit)
            : base(unit)
        {
            this._width = null;
        }
        public BdrWidth(border_width position)
            : base()
        {
            this._width = position;
        }
        #endregion

        #region properties
        public virtual border_width? position
        {
            get { return _width; }
            set { _width = value; }
        }
        protected override string parseToCSS(browser browser)
        {
            if (_width != null)
                return _width.Value.GetName();
            return null;
        }
        #endregion

        #region methods
        public override void CopyFrom(Unit unit)
        {
            base.CopyFrom(unit);
            if (unit is BdrWidth)
                _width = (unit as BdrWidth)._width;
        }
        public new BdrWidth Clone()
        {
            BdrWidth rp = new BdrWidth();
            rp.CopyFrom(this);
            return rp;
        }

        public override string ToString()
        {
            return this[browser.any];
        }
        #endregion

        #region operator overloading
        public static implicit operator BdrWidth(border_width position)
        {
            return new BdrWidth(position);
        }
        public static implicit operator border_width(BdrWidth unit)
        {
            return unit._width ?? border_width.initial;
        }
        public static implicit operator string(BdrWidth unit)
        {
            return unit.ToString();
        }
        public static implicit operator BdrWidth(int unit)
        {
            return new BdrWidth(unit);
        }
        public static implicit operator int(BdrWidth unit)
        {
            return (unit).value ?? 1;
        }
        #endregion
    }
    #endregion

    #region structs
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public struct RGB
    {
        int a, r, g, b;
        Unit _unit;
        colorMode _mode;

        public Unit unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public int R
        {
            get { return r; }
            set { r = value; }
        }
        public int G
        {
            get { return g; }
            set { g = value; }
        }
        public int B
        {
            get { return b; }
            set { b = value; }
        }
        public int Opacity
        {
            get { return 255 - a; }
            set { a = 255 - value; }
        }
        public colorMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        
        public RGB(Color color, int? transparency = null, Unit unit = null)
            : this(color.R, color.G, color.B, transparency ?? 0, unit) { }

        public RGB(string color, int? transparency = null, Unit unit = null, colorMode coloras=colorMode.html)
        {
            Color _color = Color.Black;
            try
            {
                _color = System.Drawing.ColorTranslator.FromHtml(color);
            }
            catch (Exception)
            {
                try
                {
                    _color = System.Drawing.Color.FromName(color);
                }
                catch { ; }
            }
            _unit = unit;
            r = _color.R;
            g = _color.G;
            b = _color.B;
            a = 255 - _color.A;
            this._mode = coloras;
        }

        public RGB(int r, int g, int b, int transparency, Unit unit = null)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 255 - transparency;
            _unit = unit;
            this._mode = colorMode.html;
        }
        public RGB(int r, int g, int b, Unit unit = null)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 255;
            _unit = unit;
            this._mode = colorMode.html;
        }

        public override string ToString()
        {
            var result = "";
            switch (_mode)
            {
                case colorMode.html:
                default:
                    result = ColorTranslator.ToHtml(this);
                    break;
                case colorMode.rgba:
                    result = string.Format("rgba({0},{1},{2},{3})", r, g, b, Opacity);
                    break;
            }
            return result + ((_unit != null) ? " " + _unit.ToString() : "");
        }

        public static implicit operator Color(RGB value)
        {
            return Color.FromArgb(value.Opacity, value.r, value.g, value.b);
        }
        public static implicit operator RGB(Color value)
        {
            return new RGB(value.R, value.G, value.B, 255 - value.A);
        }
        public static implicit operator RGB(string value)
        {
            return new RGB(value);
        }
        public static implicit operator string(RGB color)
        {
            return color.ToString();
        }
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Radial
    {
        #region variables
        RadialPos _p1, _p2;
        RadialSize? _size;
        shape _shape;
        #endregion

        #region constructors
        public Radial(RadialSize? size1 = null, shape? shape = null, RadialPos position1 = null, RadialPos position2 = null)
        {
            this._p1 = position1;
            if (position2 == null && position1 != null)
                this._p2 = position1.Clone();
            else this._p2 = position2;
            this._size = size1;
            this._shape = shape ?? WebHelper.shape.circle;
        }

        public Radial(int size1, int? size2 = null, shape? shape = null, RadialPos position1 = null, RadialPos position2 = null)
        {
            var first = position1 ?? position2;
            this._p1 = first;
            if (position2 == null && this._p1 != null)
                this._p2 = position1.Clone();
            else this._p2 = position2;
            this._size = new RadialSize(size1: size1, size2: size2);
            this._shape = shape ?? WebHelper.shape.circle;
        }
        #endregion

        public string this[browser browser]
        {
            get
            {
                var result = _shape.GetName() + " ";
                result += (_size != null) ? _size.ToString() : "";
                var pos1 = (_p1 == null) ? "center" : _p1.ToString();
                var pos2 = (_p2 == null) ? pos1 : _p2.ToString();
                switch (browser)
                {
                    case browser.standard:
                        return result + " at " + pos1 + " " + pos2;
                    default:
                        return pos1 + " " + pos2 + "," + result;
                }
                /*background:-moz-radial-gradient(100px 100px, circle closest-corner, #FF0000,#FFFF00,#008000);*/
                /*background:radial-gradient(circle closest-corner at 100px 100px,#FF0000,#FFFF00,#008000);*/
            }
        }
        public override string ToString()
        {
            return this[browser.standard];
        }
        public static implicit operator string(Radial value)
        {
            return value[browser.standard];
        }
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public struct RadialSize
    {
        rdsSize? _size;
        Unit _s1, _s2;

        public rdsSize? size
        {
            get { return _size; }
            set { _size = value; }
        }
        public Unit S1
        {
            get { return _s1; }
            set { _s1 = value; }
        }
        public Unit S2
        {
            get { return _s2; }
            set { _s2 = value; }
        }


        public RadialSize(Unit size1, Unit size2 = null)
        {
            var first = size1 ?? size2;
            this._s1 = first;
            if (size2 != null) this._s2 = size2;
            else
                this._s2 = (this._s1 != null) ? this._s1.Clone() : null;
            this._size = null;
        }
        public RadialSize(rdsSize size1)
        {
            this._s1 = null;
            this._s2 = null;
            this._size = size1;
        }
        public RadialSize(int size1, unitType? type1 = null, int? size2 = null, unitType? type2 = null)
        {
            this._s1 = new Unit(size1, type1 ?? unitType.px);
            if (size2 != null)
                this._s2 = new Unit(size2.Value, type2 ?? unitType.px);
            else this._s2 = (this._s1 != null) ? this._s1.Clone() : null;
            this._size = null;
        }

        public override string ToString()
        {
            if (_s1 == null || _s2 == null)
                return ((_size == null) ? rdsSize.closest_corner : _size.Value).CssValue();
            else return _s1.ToString() + " " + _s2.ToString();
        }

        public static implicit operator RadialSize(rdsSize size)
        {
            return new RadialSize(size1: size);
        }
        public static implicit operator RadialSize(int size)
        {
            return new RadialSize(size1: size);
        }
        public static implicit operator string(RadialSize size)
        {
            return size.ToString();
        }
        public static implicit operator rdsSize(RadialSize unit)
        {
            return unit._size ?? rdsSize.closest_side;
        }
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public struct BackgroundSize
    {
        background_size? _size;
        Unit _size1, _size2;

        public background_size? size
        {
            get { return _size; }
            set { _size = value; }
        }
        public Unit size1
        {
            get { return _size1; }
            set { _size1 = value; }
        }
        public Unit size2
        {
            get { return _size2; }
            set { _size2 = value; }
        }


        public BackgroundSize(Unit size1, Unit size2 = null)
        {
            var first = size1 ?? size2;
            this._size1 = first;
            if (size2 != null) this._size2 = size2;
            else
                this._size2 = (this._size1 != null) ? this._size1.Clone() : null;
            this._size = null;
        }
        public BackgroundSize(background_size size1)
        {
            this._size1 = null;
            this._size2 = null;
            this._size = size1;
        }
        public BackgroundSize(int size1, unitType? type1 = null, int? size2 = null, unitType? type2 = null)
        {
            this._size1 = new Unit(size1, type1 ?? unitType.px);
            if (size2 != null)
                this._size2 = new Unit(size2.Value, type2 ?? unitType.px);
            else this._size2 = (this._size1 != null) ? this._size1.Clone() : null;
            this._size = null;
        }

        public override string ToString()
        {
            if (_size1 == null || _size2 == null)
                return ((_size == null) ? background_size.initial : _size.Value).CssSyntax();
            else return CssHelper.CssName<background_size>() + _size1.ToString() + " " + _size2.ToString() + ";";
        }

        public static implicit operator BackgroundSize(background_size size)
        {
            return new BackgroundSize(size1: size);
        }
        public static implicit operator BackgroundSize(int size)
        {
            return new BackgroundSize(size1: size);
        }
        public static implicit operator string(BackgroundSize size)
        {
            return size.ToString();
        }
        public static implicit operator background_size(BackgroundSize unit)
        {
            return unit._size ?? background_size.initial;
        }
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public struct BackgroundPosition
    {
        BkgPos _position1, _position2;

        public BkgPos Position1
        {
            get { return _position1; }
            set { _position1 = value; }
        }
        public BkgPos Position2
        {
            get { return _position2; }
            set { _position2 = value; }
        }

        public BackgroundPosition(BkgPos position1, BkgPos position2 = null)
        {
            var first = position1 ?? position2;
            this._position1 = first;
            if (position2 != null) this._position2 = position2;
            else
                this._position2 = (this._position1 != null) ? this._position1.Clone() : null;
        }

        public BackgroundPosition(int position1, unitType? type1 = null, int? position2 = null, unitType? type2 = null)
        {
            this._position1 = new BkgPos(position1, type1 ?? unitType.px);
            if (position2 != null)
                this._position2 = new BkgPos(position2.Value, type2 ?? unitType.px);
            else this._position2 = (this._position1 != null) ? this._position1.Clone() : null;
        }

        public override string ToString()
        {
            var s = (_position1 != null) ? _position1.ToString() + " " : null;
            s += (_position2 != null) ? _position2.ToString() + ";" : null;
            if (s != null)
                return CssHelper.CssName<background_position>() + s;
            return null;
        }

        public static implicit operator BackgroundPosition(background_position position)
        {
            return new BackgroundPosition(position1: position, position2: position);
        }
        public static implicit operator BackgroundPosition(int position)
        {
            return new BackgroundPosition(position1: position);
        }
        public static implicit operator string(BackgroundPosition position)
        {
            return position.ToString();
        }
    }
    #endregion
}
//enums
namespace WebHelper
{
    /// <summary>
    /// Shapes for radial gradient
    /// </summary>
    public enum shape
    {
        /// <summary>
        /// Draws circle gradient
        /// </summary>
        circle,
        /// <summary>
        /// Draws ellipse gradient
        /// </summary>
        ellipse
    }
    public enum position
    {
        absolute,
        @static,
        @fixed,
        inherit,
        initial,
        relate,
    }
    /// <summary>
    /// Defines unit of measurement used to determine value of font-size, height, width etc.
    /// </summary>
    public enum unitType
    {
        /// <summary>
        /// pixels (1px = 1/96th of 1in)
        /// </summary>
        px,
        /// <summary>
        /// Relative to the font-size of the element (2em means 2 times the size of the current font)
        /// </summary>
        em,
        /// <summary>
        /// Relative to the x-height of the current font (rarely used) 
        /// </summary>
        ex,
        /// <summary>
        /// Relative to width of the "0" (zero)
        /// </summary>
        ch,
        /// <summary>
        /// Relative to font-size of the root element
        /// </summary>
        rem,
        /// <summary>
        /// Relative to 1% of the width of the viewport
        /// </summary>
        vw,
        /// <summary>
        /// Relative to 1% of the height of the viewport
        /// </summary>
        vh,
        /// <summary>
        /// Relative to 1% of viewport's smaller dimension
        /// </summary>
        vmin,	//
        /// <summary>
        /// Relative to 1% of viewport's larger dimension
        /// </summary>
        vmax,
        /// <summary>
        /// Relative on % basis
        /// </summary>
        prcnt,
        /// <summary>
        /// centimeters
        /// </summary>
        cm,
        /// <summary>
        /// millimeters
        /// </summary>
        mm,
        /// <summary>
        /// inches (1in = 96px = 2.54cm)
        /// </summary>
        inch,
        /// <summary>
        /// points (1pt = 1/72 of 1in)
        /// </summary>
        pt,
        /// <summary>
        /// picas (1pc = 12 pt)
        /// </summary>
        pc,
        /// <summary>
        /// Sets this property to its default value
        /// </summary>
        initial,
        /// <summary>
        /// Inherits this property from its parent element
        /// </summary>
        inherit,
        /// <summary>
        /// sets it to auto mode
        /// </summary>
        auto,
    }
    /// <summary>
    /// Defines direction of an element position or operation
    /// </summary>
    public enum direction
    {
        /// <summary>
        /// Center alignment
        /// </summary>
        center = 0,
        /// <summary>
        /// Left alignment
        /// </summary>
        left = 1,
        /// <summary>
        /// Top alignment
        /// </summary>
        top = 2,
        /// <summary>
        /// Right alignment
        /// </summary>
        right = -1,
        /// <summary>
        /// Bottom alignment
        /// </summary>
        bottom = -2
    }
    /// <summary>
    /// Defines direct background options
    /// </summary>
    public enum bkgOption
    {
        none,
        border_box,
        bottom, center,
        content_box,
        @fixed,
        inherit,
        initial,
        left,
        local,
        no_repeat,
        pading_box,
        repeat,
        repeat_x,
        repeat_y,
        right,
        round,
        scroll,
        space,
        top
    }
    public enum rdsSize
    {
        closest_side,
        farthest_side,
        closest_corner,
        farthest_corner,
    }
    public enum browser
    { 
        any,
        standard,
        webkit 
    }
    public enum colorMode
    {
        html,
        rgba,
    }
    public enum gradientType
    {
        linear_gradient,
        radial_gradient
    }

    /// <summary>
    /// Sets whether a background image is fixed or scrolls with the rest of the page
    /// </summary>
    public enum background_attachment
    {
        /// <summary>
        /// The background scrolls along with the element. This is default
        /// </summary>
        scroll,
        /// <summary>
        /// The background is fixed with regard to the viewport
        /// </summary>
        @fixed,
        /// <summary>
        /// The background scrolls along with the element's contents
        /// </summary>
        local,
        /// <summary>
        /// Sets this property to its default value. Read about initial
        /// </summary>
        initial,
        /// <summary>
        /// Inherits this property from its parent element. Read about inherit
        /// </summary>
        inherit,
    }

    /// <summary>
    /// Sets the background image for an element
    /// </summary>
    public enum background_image
    {
        /// <summary>
        /// No background image will be displayed. This is default
        /// </summary>
        none,
        /// <summary>
        /// Sets this property to its default value. Read about initial
        /// </summary>
        initial,
        /// <summary>
        /// Inherits this property from its parent element
        /// </summary>
        inherit,
    }

    /// <summary>
    /// Sets the starting position of a background image
    /// </summary>
    public enum background_position
    {
        /// <summary>
        /// Center alignment
        /// </summary>
        center = 0,
        /// <summary>
        /// Left alignment
        /// </summary>
        left = 1,
        /// <summary>
        /// Top alignment
        /// </summary>
        top = 2,
        /// <summary>
        /// Right alignment
        /// </summary>
        right = -1,
        /// <summary>
        /// Bottom alignment
        /// </summary>
        bottom = -2,
        /// <summary>
        /// Sets this property to its default value.
        /// </summary>
        initial,
        /// <summary>
        /// Inherits this property from its parent element.
        /// </summary>
        inherit,
    }

    /// <summary>
    /// Sets how a background image will be repeated
    /// </summary>
    public enum background_repeat
    {
        /// <summary>
        /// The background image will be repeated both vertically and horizontally.
        /// </summary>
        repeat,
        /// <summary>
        /// The background image will be repeated only horizontally.
        /// </summary>
        repeat_x,
        /// <summary>
        /// The background image will be repeated only vertically.
        /// </summary>
        repeat_y,
        /// <summary>
        /// The background-image will not be repeated.
        /// </summary>
        no_repeat,
        /// <summary>
        /// Sets this property to its default value. Read about initial	Play it »
        /// </summary>
        initial,
        /// <summary>
        /// Inherits this property from its parent element.
        /// </summary>
        inherit,
    }
    /// <summary>
    /// Specify the painting area of the background:
    /// </summary>
    public enum background_clip
    {
        /// <summary>
        /// Default value. The background is clipped to the border box.
        /// </summary>
        border_box,
        /// <summary>
        /// The background is clipped to the padding box.
        /// </summary>
        padding_box,
        /// <summary>
        /// The background is clipped to the content box.
        /// </summary>
        content_box,
        /// <summary>
        /// Sets this property to its default value.
        /// </summary>
        initial,
        /// <summary>
        /// Inherits this property from its parent element. 	
        /// </summary>
        inherit,
    }
    /// <summary>
    /// Specifies the positioning area of the background images
    /// </summary>
    public enum background_origin
    {
        /// <summary>
        /// Default value. The background image is positioned relative to the padding box.
        /// </summary>
        padding_box,
        /// <summary>
        /// The background image is positioned relative to the border box.
        /// </summary>
        border_box,
        /// <summary>
        /// The background image is positioned relative to the content box.
        /// </summary>
        content_box,
        /// <summary>
        /// Sets this property to its default value.
        /// </summary>
        initial,
        /// <summary>
        /// Inherits this property from its parent element. 
        /// </summary>
        inherit,
    }
    /// <summary>
    /// Specifies the size of the background images
    /// </summary>
    public enum background_size
    {
        /// <summary>
        /// Default value. The background-image contains its width and height
        /// </summary>
        auto,
        /// <summary>
        //Sets the width and height of the background image. The first value sets the width, 
        ///the second value sets the height. If only one value is given, the second is set to "auto"
        cover,
        /// <summary>
        ///	Scale the image to the largest size such that both its width and its height can fit inside the content area
        /// </summary>
        contain,
        /// <summary>
        ///	Sets this property to its default value. Read about initial
        /// </summary>
        initial,
        /// <summary>
        //	Inherits this property from its parent element. Read about inherit
        /// </summary>
        inherit,
    }

    /// <summary>
    /// The border-width property sets the width of an element's four borders. This property can have from one to four values.
    /// CSS syntax: border-width: medium|thin|thick|length|initial|inherit;
    /// </summary>
    public enum border_width
    {
        /// <summary>
        ///Specifies a thin border
        ///</summary>
        thin,
        /// <summary>
        ///Specifies a thick border
        ///</summary>
        thick,
        /// <summary>
        ///Sets this property to its default value. Read about initial
        ///</summary>
        initial,
        /// <summary>
        ///Inherits this property from its parent element. Read about inherit
        ///</summary>
        inherit,
    }
    /// <summary>
    /// The border-style property sets the style of an element's four borders. This property can have from one to four values.
    /// border-style:none|hidden|dotted|dashed|solid|double|groove|ridge|inset|outset|initial|inherit;
    /// </summary>
    public enum border_style
    {
        /// <summary>
        ///Default value. Specifies no border
        ///</summary>
        none,
        /// <summary>
        ///The same as ""none"", except in border conflict resolution for table elements
        ///</summary>
        hidden,
        /// <summary>
        ///Specifies a dotted border
        ///</summary>
        dotted,
        /// <summary>
        ///Specifies a dashed border
        ///</summary>
        dashed,
        /// <summary>
        ///Specifies a solid border
        ///</summary>
        solid,
        /// <summary>
        ///Specifies a double border
        ///</summary>
        @double,
        /// <summary>
        ///Specifies a 3D grooved border. The effect depends on the border-color value
        ///</summary>
        groove,
        /// <summary>
        ///Specifies a 3D ridged border. The effect depends on the border-color value
        ///</summary>
        ridge,
        /// <summary>
        ///Specifies a 3D inset border. The effect depends on the border-color value
        ///</summary>
        inset,
        /// <summary>
        ///Specifies a 3D outset border. The effect depends on the border-color value
        ///</summary>
        outset,
        /// <summary>
        ///Sets this property to its default value. Read about initial
        ///</summary>
        initial,
        /// <summary>
        ///Inherits this property from its parent element. Read about inherit
        ///</summary>
        inherit,

    }
    public enum image_orientation
    {
        auto,inherit,initial
    }
    public enum image_rendering
    {
        auto, inherit, initial, optimizeQuality, optimizeSpeed
    }
    public enum common
    {
        /// <summary>
        ///Sets this property to its default value. Read about initial
        ///</summary>
        initial,
        /// <summary>
        ///Inherits this property from its parent element. Read about inherit
        ///</summary>
        inherit,
        /// <summary>
        /// Sets this peoperty to auto adjust mode
        /// </summary>
        auto,
    }
}
