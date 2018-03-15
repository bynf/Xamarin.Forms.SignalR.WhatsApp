using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace topSOSLog.CustomEffects
{
    public class UnderlineEffect:RoutingEffect
    {
        public const string EffectNamespace = "topSOSLog";
        public UnderlineEffect() : base($"{EffectNamespace}.{nameof(UnderlineEffect)}")
        {

        }
    }
}
