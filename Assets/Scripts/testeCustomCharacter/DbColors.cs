using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DbColors
{

    [System.Serializable]
    private class LocalPreJson
    {
        public float4[] dado;
    }
    public static List<Color> LoadColors(float4[] f4)
    {
        
        List<Color> listaDeCores = new List<Color>();
        foreach (var f in f4)
        {
            listaDeCores.Add(new Color(f.x, f.y, f.z, f.w));
        }

        return listaDeCores;
    }


    public static Color[] ColorsByDb(RegistroDeCores sdb)
    {
        string skin = "{\"dado\":[{\"x\":1.0,\"y\":1.0,\"z\":1.0,\"w\":1.0},{\"x\":1.0,\"y\":0.8627451062202454,\"z\":0.5803921818733215,\"w\":1.0},{\"x\":1.0,\"y\":1.0,\"z\":0.41960784792900088,\"w\":1.0},{\"x\":0.9386420249938965,\"y\":0.6294423341751099,\"z\":0.607356607913971,\"w\":1.0},{\"x\":1.0,\"y\":0.6705882549285889,\"z\":0.6470588445663452,\"w\":1.0},{\"x\":0.7683894634246826,\"y\":0.6026583909988403,\"z\":0.45199382305145266,\"w\":1.0},{\"x\":1.0,\"y\":0.6392157077789307,\"z\":0.364705890417099,\"w\":1.0},{\"x\":1.0,\"y\":0.6039215922355652,\"z\":0.25882354378700259,\"w\":1.0},{\"x\":0.7683894634246826,\"y\":0.4459672272205353,\"z\":0.30434250831604006,\"w\":1.0},{\"x\":0.7683894634246826,\"y\":0.45802041888237,\"z\":0.229010209441185,\"w\":1.0},{\"x\":1.0,\"y\":0.40392157435417178,\"z\":0.062745101749897,\"w\":1.0},{\"x\":1.0,\"y\":0.7843137383460999,\"z\":0.03921568766236305,\"w\":1.0},{\"x\":0.7683894634246826,\"y\":0.5303394198417664,\"z\":0.17175765335559846,\"w\":1.0},{\"x\":0.7683894634246826,\"y\":0.6418312191963196,\"z\":0.2621564269065857,\"w\":1.0},{\"x\":0.7683894634246826,\"y\":0.6840173006057739,\"z\":0.3465285897254944,\"w\":1.0},{\"x\":0.5713573694229126,\"y\":0.4458828270435333,\"z\":0.33161136507987978,\"w\":1.0},{\"x\":0.5713573694229126,\"y\":0.407792329788208,\"z\":0.31816765666007998,\"w\":1.0},{\"x\":0.5713573694229126,\"y\":0.28007712960243227,\"z\":0.16580568253993989,\"w\":1.0},{\"x\":0.5713573694229126,\"y\":0.31144580245018008,\"z\":0.10306838899850846,\"w\":1.0},{\"x\":0.5713573694229126,\"y\":0.32040825486183169,\"z\":0.14788073301315309,\"w\":1.0},{\"x\":0.5713573694229126,\"y\":0.3629799783229828,\"z\":0.01792493835091591,\"w\":1.0},{\"x\":0.5713573694229126,\"y\":0.26439282298088076,\"z\":0.1770087629556656,\"w\":1.0},{\"x\":0.4187096357345581,\"y\":0.1937558352947235,\"z\":0.1297178864479065,\"w\":1.0},{\"x\":0.4187096357345581,\"y\":0.24629980325698853,\"z\":0.039407964795827869,\"w\":1.0},{\"x\":0.4187096357345581,\"y\":0.32183173298835757,\"z\":0.10837191343307495,\"w\":1.0},{\"x\":0.4187096357345581,\"y\":0.3546716868877411,\"z\":0.18390384316444398,\"w\":1.0},{\"x\":0.4187096357345581,\"y\":0.26928776502609255,\"z\":0.2922757565975189,\"w\":1.0},{\"x\":0.2717704772949219,\"y\":0.1651938259601593,\"z\":0.1865091621875763,\"w\":1.0},{\"x\":0.2717704772949219,\"y\":0.17478571832180024,\"z\":0.16945689916610719,\"w\":1.0},{\"x\":0.2717704772949219,\"y\":0.22700828313827516,\"z\":0.13428658246994019,\"w\":1.0},{\"x\":0.2717704772949219,\"y\":0.18544338643550874,\"z\":0.008526133373379708,\"w\":1.0},{\"x\":0.2717704772949219,\"y\":0.12256315350532532,\"z\":0.04369642958045006,\"w\":1.0},{\"x\":0.2717704772949219,\"y\":0.15560193359851838,\"z\":0.10231359302997589,\"w\":1.0},{\"x\":0.2717704772949219,\"y\":0.12895776331424714,\"z\":0.11403702199459076,\"w\":1.0},{\"x\":0.4187096357345581,\"y\":0.19868183135986329,\"z\":0.17569385468959809,\"w\":1.0},{\"x\":0.3933905363082886,\"y\":0.18666766583919526,\"z\":0.08793435245752335,\"w\":1.0}]}";
        string main = "{\"dado\":[{\"x\":0.20000000298023225,\"y\":0.20000000298023225,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.4000000059604645,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.6000000238418579,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.800000011920929,\"z\":0.800000011920929,\"w\":1.0},{\"x\":1.0,\"y\":1.0,\"z\":1.0,\"w\":1.0},{\"x\":0.07058823853731156,\"y\":1.0,\"z\":1.0,\"w\":1.0},{\"x\":0.062745101749897,\"y\":0.5333333611488342,\"z\":1.0,\"w\":1.0},{\"x\":0.08235294371843338,\"y\":0.06666667014360428,\"z\":1.0,\"w\":1.0},{\"x\":0.5254902243614197,\"y\":0.07058823853731156,\"z\":1.0,\"w\":1.0},{\"x\":1.0,\"y\":0.0784313753247261,\"z\":1.0,\"w\":1.0},{\"x\":1.0,\"y\":0.07058823853731156,\"z\":0.5372549295425415,\"w\":1.0},{\"x\":1.0,\"y\":0.07058823853731156,\"z\":0.07058823853731156,\"w\":1.0},{\"x\":1.0,\"y\":0.5411764979362488,\"z\":0.062745101749897,\"w\":1.0},{\"x\":1.0,\"y\":1.0,\"z\":0.08235294371843338,\"w\":1.0},{\"x\":0.5254902243614197,\"y\":1.0,\"z\":0.07058823853731156,\"w\":1.0},{\"x\":0.08235294371843338,\"y\":1.0,\"z\":0.08235294371843338,\"w\":1.0},{\"x\":0.062745101749897,\"y\":1.0,\"z\":0.5254902243614197,\"w\":1.0},{\"x\":0.2235294133424759,\"y\":1.0,\"z\":1.0,\"w\":1.0},{\"x\":0.2235294133424759,\"y\":0.6196078658103943,\"z\":1.0,\"w\":1.0},{\"x\":0.2235294133424759,\"y\":0.20392157137393952,\"z\":1.0,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.21176470816135407,\"z\":1.0,\"w\":1.0},{\"x\":0.9882352948188782,\"y\":0.2235294133424759,\"z\":1.0,\"w\":1.0},{\"x\":1.0,\"y\":0.2235294133424759,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":1.0,\"y\":0.21960784494876862,\"z\":0.21176470816135407,\"w\":1.0},{\"x\":1.0,\"y\":0.6078431606292725,\"z\":0.2235294133424759,\"w\":1.0},{\"x\":1.0,\"y\":1.0,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.6117647290229797,\"y\":1.0,\"z\":0.2235294133424759,\"w\":1.0},{\"x\":0.21176470816135407,\"y\":1.0,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.21176470816135407,\"y\":1.0,\"z\":0.6117647290229797,\"w\":1.0},{\"x\":0.3529411852359772,\"y\":1.0,\"z\":1.0,\"w\":1.0},{\"x\":0.3529411852359772,\"y\":0.6823529601097107,\"z\":1.0,\"w\":1.0},{\"x\":0.3529411852359772,\"y\":0.364705890417099,\"z\":1.0,\"w\":1.0},{\"x\":0.6784313917160034,\"y\":0.364705890417099,\"z\":1.0,\"w\":1.0},{\"x\":1.0,\"y\":0.3529411852359772,\"z\":1.0,\"w\":1.0},{\"x\":1.0,\"y\":0.3490196168422699,\"z\":0.6784313917160034,\"w\":1.0},{\"x\":1.0,\"y\":0.364705890417099,\"z\":0.3529411852359772,\"w\":1.0},{\"x\":1.0,\"y\":0.6823529601097107,\"z\":0.3529411852359772,\"w\":1.0},{\"x\":1.0,\"y\":1.0,\"z\":0.3529411852359772,\"w\":1.0},{\"x\":0.6666666865348816,\"y\":1.0,\"z\":0.34117648005485537,\"w\":1.0},{\"x\":0.3529411852359772,\"y\":1.0,\"z\":0.3529411852359772,\"w\":1.0},{\"x\":0.3529411852359772,\"y\":1.0,\"z\":0.6784313917160034,\"w\":1.0},{\"x\":0.48235294222831728,\"y\":1.0,\"z\":0.9882352948188782,\"w\":1.0},{\"x\":0.4941176474094391,\"y\":0.7568627595901489,\"z\":1.0,\"w\":1.0},{\"x\":0.4941176474094391,\"y\":0.501960813999176,\"z\":1.0,\"w\":1.0},{\"x\":0.7411764860153198,\"y\":0.4901960790157318,\"z\":1.0,\"w\":1.0},{\"x\":1.0,\"y\":0.501960813999176,\"z\":1.0,\"w\":1.0},{\"x\":1.0,\"y\":0.501960813999176,\"z\":0.7411764860153198,\"w\":1.0},{\"x\":1.0,\"y\":0.49803921580314639,\"z\":0.5058823823928833,\"w\":1.0},{\"x\":1.0,\"y\":0.7529411911964417,\"z\":0.5058823823928833,\"w\":1.0},{\"x\":1.0,\"y\":1.0,\"z\":0.4941176474094391,\"w\":1.0},{\"x\":0.7411764860153198,\"y\":1.0,\"z\":0.48235294222831728,\"w\":1.0},{\"x\":0.4941176474094391,\"y\":1.0,\"z\":0.4941176474094391,\"w\":1.0},{\"x\":0.48235294222831728,\"y\":1.0,\"z\":0.7411764860153198,\"w\":1.0},{\"x\":0.0564705915749073,\"y\":0.800000011920929,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.0501960813999176,\"y\":0.4266667068004608,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.0658823549747467,\"y\":0.0533333383500576,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.4203921854496002,\"y\":0.0564705915749073,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.062745101749897,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.0564705915749073,\"z\":0.4298039376735687,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.0564705915749073,\"z\":0.0564705915749073,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.432941198348999,\"z\":0.0501960813999176,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.800000011920929,\"z\":0.0658823549747467,\"w\":1.0},{\"x\":0.4203921854496002,\"y\":0.800000011920929,\"z\":0.0564705915749073,\"w\":1.0},{\"x\":0.0658823549747467,\"y\":0.800000011920929,\"z\":0.0658823549747467,\"w\":1.0},{\"x\":0.0501960813999176,\"y\":0.800000011920929,\"z\":0.4203921854496002,\"w\":1.0},{\"x\":0.1788235306739807,\"y\":0.800000011920929,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.1788235306739807,\"y\":0.49568629264831545,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.1788235306739807,\"y\":0.1631372570991516,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.48000001907348635,\"y\":0.169411763548851,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.7905882596969605,\"y\":0.1788235306739807,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.1788235306739807,\"z\":0.48000001907348635,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.1756862848997116,\"z\":0.169411763548851,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.4862745404243469,\"z\":0.1788235306739807,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.800000011920929,\"z\":0.1600000113248825,\"w\":1.0},{\"x\":0.4894118010997772,\"y\":0.800000011920929,\"z\":0.1788235306739807,\"w\":1.0},{\"x\":0.169411763548851,\"y\":0.800000011920929,\"z\":0.1600000113248825,\"w\":1.0},{\"x\":0.169411763548851,\"y\":0.800000011920929,\"z\":0.4894118010997772,\"w\":1.0},{\"x\":0.2823529541492462,\"y\":0.800000011920929,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.2823529541492462,\"y\":0.5458824038505554,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.2823529541492462,\"y\":0.2917647063732147,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.5427451133728027,\"y\":0.2917647063732147,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.2823529541492462,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.2792156934738159,\"z\":0.5427451133728027,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.2917647063732147,\"z\":0.2823529541492462,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.5458824038505554,\"z\":0.2823529541492462,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.800000011920929,\"z\":0.2823529541492462,\"w\":1.0},{\"x\":0.5333333611488342,\"y\":0.800000011920929,\"z\":0.2729412019252777,\"w\":1.0},{\"x\":0.2823529541492462,\"y\":0.800000011920929,\"z\":0.2823529541492462,\"w\":1.0},{\"x\":0.2823529541492462,\"y\":0.800000011920929,\"z\":0.5427451133728027,\"w\":1.0},{\"x\":0.38588234782218935,\"y\":0.800000011920929,\"z\":0.7905882596969605,\"w\":1.0},{\"x\":0.3952941298484802,\"y\":0.6054902076721191,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.3952941298484802,\"y\":0.4015686511993408,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.5929412245750427,\"y\":0.3921568691730499,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.4015686511993408,\"z\":0.800000011920929,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.4015686511993408,\"z\":0.5929412245750427,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.3984313905239105,\"z\":0.4047059118747711,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.6023529767990112,\"z\":0.4047059118747711,\"w\":1.0},{\"x\":0.800000011920929,\"y\":0.800000011920929,\"z\":0.3952941298484802,\"w\":1.0},{\"x\":0.5929412245750427,\"y\":0.800000011920929,\"z\":0.38588234782218935,\"w\":1.0},{\"x\":0.3952941298484802,\"y\":0.800000011920929,\"z\":0.3952941298484802,\"w\":1.0},{\"x\":0.38588234782218935,\"y\":0.800000011920929,\"z\":0.5929412245750427,\"w\":1.0},{\"x\":0.04235294461250305,\"y\":0.6000000238418579,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.0376470610499382,\"y\":0.320000022649765,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.04941176995635033,\"y\":0.04000000283122063,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.31529414653778078,\"y\":0.04235294461250305,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.0470588281750679,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.04235294461250305,\"z\":0.32235297560691836,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.04235294461250305,\"z\":0.04235294461250305,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.32470589876174929,\"z\":0.0376470610499382,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.6000000238418579,\"z\":0.04941176995635033,\"w\":1.0},{\"x\":0.31529414653778078,\"y\":0.6000000238418579,\"z\":0.04235294461250305,\"w\":1.0},{\"x\":0.04941176995635033,\"y\":0.6000000238418579,\"z\":0.04941176995635033,\"w\":1.0},{\"x\":0.0376470610499382,\"y\":0.6000000238418579,\"z\":0.31529414653778078,\"w\":1.0},{\"x\":0.13411764800548554,\"y\":0.6000000238418579,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.13411764800548554,\"y\":0.37176471948623659,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.13411764800548554,\"y\":0.1223529502749443,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.36000001430511477,\"y\":0.12705883383750916,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.5929412245750427,\"y\":0.13411764800548554,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.13411764800548554,\"z\":0.36000001430511477,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.1317647099494934,\"z\":0.12705883383750916,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.3647059202194214,\"z\":0.13411764800548554,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.6000000238418579,\"z\":0.12000000476837158,\"w\":1.0},{\"x\":0.3670588433742523,\"y\":0.6000000238418579,\"z\":0.13411764800548554,\"w\":1.0},{\"x\":0.12705883383750916,\"y\":0.6000000238418579,\"z\":0.12000000476837158,\"w\":1.0},{\"x\":0.12705883383750916,\"y\":0.6000000238418579,\"z\":0.3670588433742523,\"w\":1.0},{\"x\":0.21176472306251527,\"y\":0.6000000238418579,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.21176472306251527,\"y\":0.40941178798675539,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.21176472306251527,\"y\":0.21882353723049165,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.40705886483192446,\"y\":0.21882353723049165,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.21176472306251527,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.20941178500652314,\"z\":0.40705886483192446,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.21882353723049165,\"z\":0.21176472306251527,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.40941178798675539,\"z\":0.21176472306251527,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.6000000238418579,\"z\":0.21176472306251527,\"w\":1.0},{\"x\":0.40000003576278689,\"y\":0.6000000238418579,\"z\":0.20470589399337769,\"w\":1.0},{\"x\":0.21176472306251527,\"y\":0.6000000238418579,\"z\":0.21176472306251527,\"w\":1.0},{\"x\":0.21176472306251527,\"y\":0.6000000238418579,\"z\":0.40705886483192446,\"w\":1.0},{\"x\":0.2894117832183838,\"y\":0.6000000238418579,\"z\":0.5929412245750427,\"w\":1.0},{\"x\":0.29647061228752139,\"y\":0.45411768555641177,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.29647061228752139,\"y\":0.3011764883995056,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.44470590353012087,\"y\":0.29411765933036806,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.3011764883995056,\"z\":0.6000000238418579,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.3011764883995056,\"z\":0.44470590353012087,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.2988235354423523,\"z\":0.30352944135665896,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.4517647325992584,\"z\":0.30352944135665896,\"w\":1.0},{\"x\":0.6000000238418579,\"y\":0.6000000238418579,\"z\":0.29647061228752139,\"w\":1.0},{\"x\":0.44470590353012087,\"y\":0.6000000238418579,\"z\":0.2894117832183838,\"w\":1.0},{\"x\":0.29647061228752139,\"y\":0.6000000238418579,\"z\":0.29647061228752139,\"w\":1.0},{\"x\":0.2894117832183838,\"y\":0.6000000238418579,\"z\":0.44470590353012087,\"w\":1.0},{\"x\":0.02823529578745365,\"y\":0.4000000059604645,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.0250980406999588,\"y\":0.2133333534002304,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.03294117748737335,\"y\":0.0266666691750288,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.2101960927248001,\"y\":0.02823529578745365,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.0313725508749485,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.02823529578745365,\"z\":0.21490196883678437,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.02823529578745365,\"z\":0.02823529578745365,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.2164705991744995,\"z\":0.0250980406999588,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.4000000059604645,\"z\":0.03294117748737335,\"w\":1.0},{\"x\":0.2101960927248001,\"y\":0.4000000059604645,\"z\":0.02823529578745365,\"w\":1.0},{\"x\":0.03294117748737335,\"y\":0.4000000059604645,\"z\":0.03294117748737335,\"w\":1.0},{\"x\":0.0250980406999588,\"y\":0.4000000059604645,\"z\":0.2101960927248001,\"w\":1.0},{\"x\":0.08941176533699036,\"y\":0.4000000059604645,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.08941176533699036,\"y\":0.24784314632415772,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.08941176533699036,\"y\":0.0815686285495758,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.24000000953674317,\"y\":0.0847058817744255,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.3952941298484802,\"y\":0.08941176533699036,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.08941176533699036,\"z\":0.24000000953674317,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.0878431424498558,\"z\":0.0847058817744255,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.24313727021217347,\"z\":0.08941176533699036,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.4000000059604645,\"z\":0.08000000566244126,\"w\":1.0},{\"x\":0.2447059005498886,\"y\":0.4000000059604645,\"z\":0.08941176533699036,\"w\":1.0},{\"x\":0.0847058817744255,\"y\":0.4000000059604645,\"z\":0.08000000566244126,\"w\":1.0},{\"x\":0.0847058817744255,\"y\":0.4000000059604645,\"z\":0.2447059005498886,\"w\":1.0},{\"x\":0.1411764770746231,\"y\":0.4000000059604645,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.1411764770746231,\"y\":0.2729412019252777,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.1411764770746231,\"y\":0.14588235318660737,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.27137255668640139,\"y\":0.14588235318660737,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.1411764770746231,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.13960784673690797,\"z\":0.27137255668640139,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.14588235318660737,\"z\":0.1411764770746231,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.2729412019252777,\"z\":0.1411764770746231,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.4000000059604645,\"z\":0.1411764770746231,\"w\":1.0},{\"x\":0.2666666805744171,\"y\":0.4000000059604645,\"z\":0.13647060096263886,\"w\":1.0},{\"x\":0.1411764770746231,\"y\":0.4000000059604645,\"z\":0.1411764770746231,\"w\":1.0},{\"x\":0.1411764770746231,\"y\":0.4000000059604645,\"z\":0.27137255668640139,\"w\":1.0},{\"x\":0.19294117391109467,\"y\":0.4000000059604645,\"z\":0.3952941298484802,\"w\":1.0},{\"x\":0.1976470649242401,\"y\":0.30274510383605959,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.1976470649242401,\"y\":0.2007843255996704,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.29647061228752139,\"y\":0.19607843458652497,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.2007843255996704,\"z\":0.4000000059604645,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.2007843255996704,\"z\":0.29647061228752139,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.19921569526195527,\"z\":0.20235295593738557,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.3011764883995056,\"z\":0.20235295593738557,\"w\":1.0},{\"x\":0.4000000059604645,\"y\":0.4000000059604645,\"z\":0.1976470649242401,\"w\":1.0},{\"x\":0.29647061228752139,\"y\":0.4000000059604645,\"z\":0.19294117391109467,\"w\":1.0},{\"x\":0.1976470649242401,\"y\":0.4000000059604645,\"z\":0.1976470649242401,\"w\":1.0},{\"x\":0.19294117391109467,\"y\":0.4000000059604645,\"z\":0.29647061228752139,\"w\":1.0},{\"x\":0.014117647893726826,\"y\":0.20000000298023225,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.0125490203499794,\"y\":0.1066666767001152,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.016470588743686677,\"y\":0.0133333345875144,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.10509804636240006,\"y\":0.014117647893726826,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.01568627543747425,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.014117647893726826,\"z\":0.10745098441839218,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.014117647893726826,\"z\":0.014117647893726826,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.10823529958724976,\"z\":0.0125490203499794,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.20000000298023225,\"z\":0.016470588743686677,\"w\":1.0},{\"x\":0.10509804636240006,\"y\":0.20000000298023225,\"z\":0.014117647893726826,\"w\":1.0},{\"x\":0.016470588743686677,\"y\":0.20000000298023225,\"z\":0.016470588743686677,\"w\":1.0},{\"x\":0.0125490203499794,\"y\":0.20000000298023225,\"z\":0.10509804636240006,\"w\":1.0},{\"x\":0.04470588266849518,\"y\":0.20000000298023225,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.04470588266849518,\"y\":0.12392157316207886,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.04470588266849518,\"y\":0.0407843142747879,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.12000000476837158,\"y\":0.04235294088721275,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.1976470649242401,\"y\":0.04470588266849518,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.04470588266849518,\"z\":0.12000000476837158,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.0439215712249279,\"z\":0.04235294088721275,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.12156863510608673,\"z\":0.04470588266849518,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.20000000298023225,\"z\":0.04000000283122063,\"w\":1.0},{\"x\":0.1223529502749443,\"y\":0.20000000298023225,\"z\":0.04470588266849518,\"w\":1.0},{\"x\":0.04235294088721275,\"y\":0.20000000298023225,\"z\":0.04000000283122063,\"w\":1.0},{\"x\":0.04235294088721275,\"y\":0.20000000298023225,\"z\":0.1223529502749443,\"w\":1.0},{\"x\":0.07058823853731156,\"y\":0.20000000298023225,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.07058823853731156,\"y\":0.13647060096263886,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.07058823853731156,\"y\":0.07294117659330368,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.13568627834320069,\"y\":0.07294117659330368,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.07058823853731156,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.06980392336845398,\"z\":0.13568627834320069,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.07294117659330368,\"z\":0.07058823853731156,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.13647060096263886,\"z\":0.07058823853731156,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.20000000298023225,\"z\":0.07058823853731156,\"w\":1.0},{\"x\":0.13333334028720857,\"y\":0.20000000298023225,\"z\":0.06823530048131943,\"w\":1.0},{\"x\":0.07058823853731156,\"y\":0.20000000298023225,\"z\":0.07058823853731156,\"w\":1.0},{\"x\":0.07058823853731156,\"y\":0.20000000298023225,\"z\":0.13568627834320069,\"w\":1.0},{\"x\":0.09647058695554733,\"y\":0.20000000298023225,\"z\":0.1976470649242401,\"w\":1.0},{\"x\":0.09882353246212006,\"y\":0.15137255191802979,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.09882353246212006,\"y\":0.1003921627998352,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.14823530614376069,\"y\":0.09803921729326248,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.1003921627998352,\"z\":0.20000000298023225,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.1003921627998352,\"z\":0.14823530614376069,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.09960784763097763,\"z\":0.10117647796869278,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.1505882441997528,\"z\":0.10117647796869278,\"w\":1.0},{\"x\":0.20000000298023225,\"y\":0.20000000298023225,\"z\":0.09882353246212006,\"w\":1.0},{\"x\":0.14823530614376069,\"y\":0.20000000298023225,\"z\":0.09647058695554733,\"w\":1.0},{\"x\":0.09882353246212006,\"y\":0.20000000298023225,\"z\":0.09882353246212006,\"w\":1.0},{\"x\":0.09647058695554733,\"y\":0.20000000298023225,\"z\":0.14823530614376069,\"w\":1.0}]}";

        LocalPreJson preSkin = JsonUtility.FromJson<LocalPreJson>(skin);
        LocalPreJson preMain = JsonUtility.FromJson<LocalPreJson>(main);

        switch (sdb)
        {
            case RegistroDeCores.skin: return LoadColors(preSkin.dado).ToArray();//ColorDbManager.LoadColors("DateColors/skinColors.crs").ToArray();
            default: return LoadColors(preMain.dado).ToArray();//ColorDbManager.LoadColors("DateColors/mainColors.crs").ToArray();
        };
    }

    public static int GetApproximateColorIndex(Color[] cores, Color cor)
    {
        int retorno = 0;
        Vector3 Vb = new Vector3(cor.r, cor.g, cor.b);
        for (int i = 0; i < cores.Length; i++)
        {
            Color c = cores[retorno];
            Vector3 Vr = new Vector3(c.r, c.g, c.b);
            c = cores[i];
            Vector3 Vt = new Vector3(c.r, c.g, c.b);
            if (Vector3.Distance(Vb, Vt) < Vector3.Distance(Vb, Vr))
            {
                retorno = i;
            }
        }

        return retorno;
    }

    
}