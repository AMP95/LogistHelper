using Shared;
using System.IO;
using System.Text.Json;

namespace LogistHelper.Models.Settings
{
    public class JsonRepository : ISettingsRepository<Settings>
    {
        private string _fileName = "settings.txt";
        private Settings _settings;
        private ILogger _logger;

        public JsonRepository(ILogger logger)
        {
            _logger = logger;
            InitSettings();
        }

        private void InitSettings() 
        {
            if (!File.Exists(_fileName)) 
            {
                Settings settings = new Settings()
                {
                    ServerUri = "https://localhost:7081/api",
                    DaDataApiKey = "00475e8fb9e3d1877e8b9e0d5d5f269c2a5a7f90",
                    DaDataSecretKey = "a9bf357e95073eff9a20f62532fb0db1ebfa7bc3",
                    TruckModels = new List<CarModelSearch>() 
                    { 
                        new CarModelSearch() { Standart = "МАЗ", SearchInputs = "маз,мас,maz" },
                        new CarModelSearch() { Standart = "БелАЗ", SearchInputs = "белаз,белас,билаз,билас,belaz" },
                        new CarModelSearch() { Standart = "БАЗ", SearchInputs = "баз,бас,baz" },
                        new CarModelSearch() { Standart = "ГАЗ", SearchInputs = "газ,гас,gaz" },
                        new CarModelSearch() { Standart = "ЗиЛ", SearchInputs = "зил,zil" },
                        new CarModelSearch() { Standart = "УралАЗ", SearchInputs = "уралаз,uralaz" },
                        new CarModelSearch() { Standart = "КАМАЗ", SearchInputs = "камаз,kamaz" },
                        new CarModelSearch() { Standart = "Краз", SearchInputs = "краз,kraz" },
                        new CarModelSearch() { Standart = "УАЗ", SearchInputs = "уаз,uaz" },
                        new CarModelSearch() { Standart = "Sollers", SearchInputs = "соллерс,солерс,sollers,solers,ыщддукы,ыщдукы,cjkkthc,cjkthc" },
                        new CarModelSearch() { Standart = "DAF", SearchInputs = "daf,даф,вфа,lfa" },
                        new CarModelSearch() { Standart = "Dongfeng", SearchInputs = "dongfeng,donfeng,донфенг,донгфенг,донгфэнг,донфэнг,вщтпаутп,вщтаутп,ljyatyu,ljyuatyu,ljya\'yu,ljya\'yu" },
                        new CarModelSearch() { Standart = "ERF", SearchInputs = "erf,ирф,ука,bha" },
                        new CarModelSearch() { Standart = "FAW", SearchInputs = "faw,фав,фоу,афц,afd,aje" },
                        new CarModelSearch() { Standart = "Ford", SearchInputs = "ford,форд,ащкв,ajhl" },
                        new CarModelSearch() { Standart = "Fiat", SearchInputs = "fiat,фиат,ашфе,abfn" },
                        new CarModelSearch() { Standart = "FOTON", SearchInputs = "foton,фотон,ащещт,ajnjy" },
                        new CarModelSearch() { Standart = "Freightliner", SearchInputs = "freightliner,freghtliner,fredliner,фрейтлайнер,фретлайнер,фрейдлайнер,фредлайнер,акуфшпредштук,акупредштук,акувдштук,ahtqnkfqyth,ahtnkfqyth,ahtlkfqyth" },
                        new CarModelSearch() { Standart = "Howo", SearchInputs = "howo,[jdj,хово,рщцщ" },
                        new CarModelSearch() { Standart = "Hyundai", SearchInputs = "hyundai,hyunday,hundai,hunday,[tylt,[`ylt,[tylfq,[`ylfq,хенде,хёнде,хендай,хёндай,рнгтвфг,рнгтвфн,ргтвфш,рнтвфн" },
                        new CarModelSearch() { Standart = "International", SearchInputs = "international,bynthyfwbjyfk,bynthytiyk,интернационал,интернешнл,штеуктфешщтыд" },
                        new CarModelSearch() { Standart = "Isuzu", SearchInputs = "isuzu,bcepe,исузу,шыгяг" },
                        new CarModelSearch() { Standart = "Iveco", SearchInputs = "iveco,bdtrj,ивеко,шмусщ" },
                        new CarModelSearch() { Standart = "Kenworth", SearchInputs = "kenworth,kenwort,kenvorth,kenvort,rtydjhn,кенворт,лутцщкер,лутцщке,лутмщкер,лутмщке" },
                        new CarModelSearch() { Standart = "KIA", SearchInputs = "kia,rbf,rbz,киа,кия,лшф" },
                        new CarModelSearch() { Standart = "LDV", SearchInputs = "ldv,kld,лдв,двм" },
                        new CarModelSearch() { Standart = "MAN", SearchInputs = "man,vfy,ман,ьфт" },
                        new CarModelSearch() { Standart = "Mazda", SearchInputs = "mazda,vfplf,мазда,ьфявф" },
                        new CarModelSearch() { Standart = "Mercedes-Benz", SearchInputs = "mercedes-benz,mercedes benz,vthctltc,мерседес,ьукыувуы-иутя,ьукыувуы иутя" },
                        new CarModelSearch() { Standart = "Mack", SearchInputs = "mack,mac,mak,vfr,мак,ьфсл,ьфс,ьфл" },
                        new CarModelSearch() { Standart = "Peugeot", SearchInputs = "peugeot,pegeot,peugot,pegot,pegeot,gt;j,gb;j,пежо,пижо,зугпуще,зугпще,зупуще,зупще" },
                        new CarModelSearch() { Standart = "Peterbilt", SearchInputs = "peterbilt,peterbild,piterbilt,piterbild,gtnth,bkn,gtnth,bkn,gbnth,bkl,gbnth,bkn,петербилт,петербилд,питербилт,питербилд,зуеукишде,зуеукишдв,зшеукишде,зшеукишдв" },
                        new CarModelSearch() { Standart = "Renault", SearchInputs = "renault,renalt,reno,htyj,рено,кутфгде,кутфде,кутщ" },
                        new CarModelSearch() { Standart = "Scania", SearchInputs = "scania,crfybz,crfybf,скания,сканиа,ысфтшф" },
                        new CarModelSearch() { Standart = "Sterling", SearchInputs = "sterling,cnehkbyu,cnthkbyr,стерлинг,стерлинк,ыеукдштп" },
                        new CarModelSearch() { Standart = "SISU", SearchInputs = "sisu,cbce,cbpe,сису,сизу,ышыг" },
                        new CarModelSearch() { Standart = "SAIC", SearchInputs = "saic,cfbr,cfbu,cfqr,саик,саиг,сайк,ыфшс" },
                        new CarModelSearch() { Standart = "Tesla", SearchInputs = "tesla,ntckf,тесла,еуыдф" },
                        new CarModelSearch() { Standart = "Tata", SearchInputs = "tata,nfnf,тата,ефеф" },
                        new CarModelSearch() { Standart = "Tatra", SearchInputs = "tatra,tarta,nfhnf,nfnhf,татра,тарта,ефекф,ефкеф" },
                        new CarModelSearch() { Standart = "Toyota", SearchInputs = "toyota,toiota,njqjnf,nj`nf,nfqjnf,nf`nf,тойота,тоёта,тайота,таёта,ещнщеф,ещшщеф" },
                        new CarModelSearch() { Standart = "Volkswagen", SearchInputs = "volkswagen,folkswagen,ajkmrcdfuty,djkmrcdfuty,фольксваген,вольксваген,мщдлыцфпут,ащдлыцфпут" },
                        new CarModelSearch() { Standart = "Volvo", SearchInputs = "volvo,djkmdj,вольво,мщдмщ" },
                        new CarModelSearch() { Standart = "Nissan", SearchInputs = "nissan,nisan,ybcfy,ybccfy,ниссан,нисан,тшыыфт,тшыфт" },
                        new CarModelSearch() { Standart = "Agrale", SearchInputs = "agrale,argale,fhufk,fuhfk,аграл,аргал,фпкфду,фкпфду" },
                        new CarModelSearch() { Standart = "Shacman", SearchInputs = "shacman,shakman,ifrvfy,cfrvfy,xfrvfy,шакман,сакман,чакман,ырфсьфт,ырфльфт" },
                        new CarModelSearch() { Standart = "SITRAK", SearchInputs = "sitrak,sirtak,cbnhfr,cbhnfr,ситрак,сиртак,ышекфл,ышкефл" },
                        new CarModelSearch() { Standart = "BAW", SearchInputs = "baw,,fj,,fe,,fd,бао,бау,бав,ифц" },
                        new CarModelSearch() { Standart = "BharatBenz", SearchInputs = "bharatbenz,bharat benz,baratbenz,barat benz,,fhfn,tyw,,fhfn,tyw,баратбенцббарат бенц,ирфкфеиутя,ирфкфе иутя,ифкфеиутя,ифкфе иутя" },
                        new CarModelSearch() { Standart = "Beifang Benchi", SearchInputs = "beifang benchi,beefang benchi,befang benchi,,bafyu ,tyxb,бифанг бенчи,иушафтп иутсрш,иууафтп иутсрш,ишафтп иутсрш" },
                        new CarModelSearch() { Standart = "Chevrolet", SearchInputs = "chevrolet,shevrolet,itdhjkt,шевроле,срумкщдуе,ырумкщдуе" },
                        new CarModelSearch() { Standart = "Citroën", SearchInputs = "citroën,citroen,cbnhjtn,wbnhjty,ситроен,цитроен,сшекщут" },
                        new CarModelSearch() { Standart = "САМС", SearchInputs = "camc,kamk,camk,kamc,rfvr,cfvr,cfvc,rfvc,камк,самк,камс,самс,сфьс,сфьл,лфьс,лфьл" },
                        new CarModelSearch() { Standart = "Changan", SearchInputs = "changan,shangan,xfyufy,ifyufy,cfyufy,санган,шанган,чанган,срфтпфт,ырфтпфт" },
                        new CarModelSearch() { Standart = "Chery", SearchInputs = "chery,cheri,xthb,чери,срукн,срукш" },
                        new CarModelSearch() { Standart = "CIMC", SearchInputs = "cimc,kimc,cimk,kimk,rbvr,rbvc,cbrv,cbvc,кимк,кимс,симк,симс,сшьс,лшьс,сшьл,лшьл" },
                        new CarModelSearch() { Standart = "Dayun", SearchInputs = "dayun,daun,daiun,lf.y,lfey,даюн,даун,вфнгт,вфшгт" },
                        new CarModelSearch() { Standart = "JAC", SearchInputs = "jac,jak,l;fr,джак,офс,офл" },
                        new CarModelSearch() { Standart = "JMC", SearchInputs = "jmc,оьс" },
                        new CarModelSearch() { Standart = "Hino", SearchInputs = "hino,[byj,хино,hino" },
                        new CarModelSearch() { Standart = "Hongyan", SearchInputs = "hongyan,hongan,[jyufy,[jyuzy,хонган,хонгян,рщтпнфт,рщтпфт" },
                        new CarModelSearch() { Standart = "Great Wall", SearchInputs = "great wall,greatwall,gretwall,uhbn dfk,uhbndfk,грит вал,гритвал,грит вол,гритвол,грейтвол,грейт вол,грейтвал,грейт вал" },
                        new CarModelSearch() { Standart = "Gonow", SearchInputs = "gonow,ujyje,ujyjd,гонов,гоноу,пщтщц" },
                        new CarModelSearch() { Standart = "Lifan", SearchInputs = "lifan,lefan,kbafy,ktafy,лифан,лефн,дшафт,дуафт" },
                        new CarModelSearch() { Standart = "Naveco", SearchInputs = "naveco,naveko,yfdtrj,yfdtcj,навеко,навесо,тфмусщ,тфмулщ" },
                        new CarModelSearch() { Standart = "Western Star", SearchInputs = "western star,westernstar,dtcnthy cnfh,dtcnthycnfh,вестерн стар,вестернстар,цуыеукт ыефк,цуыеуктыефк" },
                        new CarModelSearch() { Standart = "Workhorse", SearchInputs = "workhorse,worhorse,djhr[jhc,djh[jhc,воркхорс,ворхорс,цщклрщкыу,цщкрщкыу" },
                        new CarModelSearch() { Standart = "Wuling", SearchInputs = "wuling,wiuling,wuiling,dekbyu,dbekbyu,debkbyu,вулинг,виулинг,вуилиг,цгдштп,цшгдштп,цгшдштп" },
                    }
                };

                SaveSettings(settings);    
            }
        }

        public Settings GetSettings()
        {
            if (_settings == null) 
            {
                try 
                {
                    string str = File.ReadAllText(_fileName);
                    _settings = JsonSerializer.Deserialize<Settings>(str);
                }
                catch (Exception ex)
                {
                    _logger.Log(ex, ex.Message, LogLevel.Error);
                }

            }
            return _settings;
        }

        public void ResetSettings()
        {
            _settings = null;
        }

        public void SaveSettings(Settings settings)
        {
            _settings = settings;
            try
            {
                using (var file = File.OpenWrite(_fileName)) 
                {
                    JsonSerializer.Serialize<Settings>(file, _settings);
                }
            }
            catch (Exception ex) 
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
            }
        }
    }
}
