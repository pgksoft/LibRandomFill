using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RandomFill {
  public enum TypeFill { All, LineGradient, RadialGradient }
  internal enum DirectLinearGradient { LeftRight, TopBottom, DiagonalLeftRight, DiagonalRightLeft }
  public sealed class RandomFills {
    private RandomFills() {
      rand = new Random();
      propInfoColors = typeof(Colors).GetProperties();
    }

    public static RandomFills Instance() {
      if (uniqueInstance == null) {
        uniqueInstance = new RandomFills();
      }
      return uniqueInstance;
    }

    #region Fields
    private static RandomFills uniqueInstance;
    private Random rand;
    readonly PropertyInfo[] propInfoColors;
    private DirectLinearGradient[] directLineGradient = new DirectLinearGradient[] {
      DirectLinearGradient.LeftRight,
      DirectLinearGradient.TopBottom,
      DirectLinearGradient.DiagonalLeftRight,
      DirectLinearGradient.DiagonalRightLeft
    };
    #endregion

    #region Properties
    public int CountColors { get { return propInfoColors.Length; } }
    #endregion

    public Color GetColor() => (Color)propInfoColors[rand.Next(0, CountColors - 1)].GetValue(null);
    public SolidColorBrush GetSolidColorBrush() => new SolidColorBrush(GetColor());
    public LinearGradientBrush GetLinearGradientBrush() {
      DirectLinearGradient direct = directLineGradient[rand.Next(0, 3)];
      LinearGradientBrush myBrush = new LinearGradientBrush {
        StartPoint = GetStartPoint(direct),
        EndPoint = GetEndPoint(direct)
      };
      int CountStops = rand.Next(2, 5);
      double offSet = 0;
      double stepSet = 1.0 / CountStops;
      for (int i = 1; i <= CountStops; i++) {
        if (i == CountStops) {
          offSet = 1;
        } else if (i == 1) {
          offSet = 0;
        } else {
          offSet = stepSet * i;
        }
        myBrush.GradientStops.Add(new GradientStop(GetColor(), offSet));
      }
      return myBrush;
    }
    public RadialGradientBrush GetRadialGradientBrush() {
      RadialGradientBrush myBrush = new RadialGradientBrush {
        GradientOrigin = new Point(rand.NextDouble(), rand.NextDouble())
      };
      int CountStops = rand.Next(3, 10);
      double offSet = 0;
      double stepSet = 1.0 / CountStops;
      for (int i = 1; i <= CountStops; i++) {
        if (i == CountStops) {
          offSet = 1;
        } else if (i == 1) {
          offSet = 0;
        } else {
          offSet = stepSet * i;
        }
        myBrush.GradientStops.Add(new GradientStop(GetColor(), offSet));
      }
      return myBrush;
    }
    public GradientBrush GetGradientBrush() {
      if (rand.Next(0, 2) == 0) {
        return GetLinearGradientBrush();
      } else return GetRadialGradientBrush();
    }
    private Point GetStartPoint(DirectLinearGradient direct) {
      switch (direct) {
        case DirectLinearGradient.LeftRight:
          return new Point(0, 0.5);
        case DirectLinearGradient.TopBottom:
          return new Point(0.5, 0);
        case DirectLinearGradient.DiagonalLeftRight:
          return new Point(0, 0);
        case DirectLinearGradient.DiagonalRightLeft:
          return new Point(1, 0);
        default:
          return new Point(0, 0);
      }
    }
    private Point GetEndPoint(DirectLinearGradient direct) {
      switch (direct) {
        case DirectLinearGradient.LeftRight:
          return new Point(1, 0.5);
        case DirectLinearGradient.TopBottom:
          return new Point(0.5, 1);
        case DirectLinearGradient.DiagonalLeftRight:
          return new Point(1, 1);
        case DirectLinearGradient.DiagonalRightLeft:
          return new Point(0, 1);
        default:
          return new Point(1, 1);
      }
    }
  }
}
