using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data;

namespace PRUNner.Backend.UserDataParser
{
    public static class UserDataReader
    {
        public static PlanetaryBase Load()
        {
            var jObject = JObject.Parse(File.ReadAllText(UserDataPaths.UserDataFolder + UserDataPaths.PlanetFile));
            return ReadPlanet(jObject);
        }
        
        public static PlanetaryBase ReadPlanet(JObject obj)
        {
            var result = new PlanetaryBase(PlanetData.Get(obj.GetValue(nameof(PlanetaryBase.Planet))!.ToObject<string>()!)!);
            
            ReadInfrastructureBuildings((JObject) obj[nameof(PlanetaryBase.InfrastructureBuildings)]!, result.InfrastructureBuildings);
            ReadProductionBuildings((JArray) obj[nameof(PlanetaryBase.ProductionBuildings)]!, result);
            ReadExpertAllocation((JObject) obj[nameof(PlanetaryBase.ExpertAllocation)]!, result.ExpertAllocation);
            ReadConsumableData((JObject) obj[nameof(PlanetaryBase.ProvidedConsumables)]!, result.ProvidedConsumables);

            return result;
        }

        private static void ReadProductionBuildings(JArray buildingArray, PlanetaryBase planetaryBase)
        {
            planetaryBase.ProductionBuildings.Clear();

            foreach (var buildingObject in buildingArray.Cast<JObject>())
            {
                var building = planetaryBase.AddBuilding(BuildingData.GetOrThrow(buildingObject.GetValue(nameof(PlanetBuilding.Building))?.ToObject<string>() ?? ""));
                building.Amount = buildingObject.GetValue(nameof(PlanetBuilding.Amount))?.ToObject<int>() ?? 0;

                building.Production.Clear();
                foreach (var productionObject in buildingObject.GetValue(nameof(PlanetBuilding.Production))!.Cast<JObject>())
                {
                    var production = building.AddProduction();
                    production.Percentage = productionObject.GetValue(nameof(PlanetBuildingProductionQueueElement.Percentage))!.ToObject<double>();
                    var recipeName = productionObject.GetValue(nameof(PlanetBuildingProductionQueueElement.ActiveRecipe))!.ToObject<string>();

                    production.ActiveRecipe = building.AvailableRecipes.SingleOrDefault(x => x.RecipeName.Equals(recipeName));
                }
            }
        }

        private static void ReadConsumableData(JObject jObject, ProvidedConsumables providedConsumables)
        {
            providedConsumables.DW = jObject.GetValue(nameof(ProvidedConsumables.DW))?.ToObject<bool>() ?? true;
            providedConsumables.RAT = jObject.GetValue(nameof(ProvidedConsumables.RAT))?.ToObject<bool>() ?? true;
            providedConsumables.OVE = jObject.GetValue(nameof(ProvidedConsumables.OVE))?.ToObject<bool>() ?? true;
            providedConsumables.EXO = jObject.GetValue(nameof(ProvidedConsumables.EXO))?.ToObject<bool>() ?? true;
            providedConsumables.PT = jObject.GetValue(nameof(ProvidedConsumables.PT))?.ToObject<bool>() ?? true;
            providedConsumables.MED = jObject.GetValue(nameof(ProvidedConsumables.MED))?.ToObject<bool>() ?? true;
            providedConsumables.HMS = jObject.GetValue(nameof(ProvidedConsumables.HMS))?.ToObject<bool>() ?? true;
            providedConsumables.SCN = jObject.GetValue(nameof(ProvidedConsumables.SCN))?.ToObject<bool>() ?? true;
            providedConsumables.FIM = jObject.GetValue(nameof(ProvidedConsumables.FIM))?.ToObject<bool>() ?? true;
            providedConsumables.HSS = jObject.GetValue(nameof(ProvidedConsumables.HSS))?.ToObject<bool>() ?? true;
            providedConsumables.PDA = jObject.GetValue(nameof(ProvidedConsumables.PDA))?.ToObject<bool>() ?? true;
            providedConsumables.MEA = jObject.GetValue(nameof(ProvidedConsumables.MEA))?.ToObject<bool>() ?? true;
            providedConsumables.LC = jObject.GetValue(nameof(ProvidedConsumables.LC))?.ToObject<bool>() ?? true;
            providedConsumables.WS = jObject.GetValue(nameof(ProvidedConsumables.WS))?.ToObject<bool>() ?? true;
            providedConsumables.COF = jObject.GetValue(nameof(ProvidedConsumables.COF))?.ToObject<bool>() ?? false;
            providedConsumables.PWO = jObject.GetValue(nameof(ProvidedConsumables.PWO))?.ToObject<bool>() ?? false;
            providedConsumables.KOM = jObject.GetValue(nameof(ProvidedConsumables.KOM))?.ToObject<bool>() ?? false;
            providedConsumables.REP = jObject.GetValue(nameof(ProvidedConsumables.REP))?.ToObject<bool>() ?? false;
            providedConsumables.ALE = jObject.GetValue(nameof(ProvidedConsumables.ALE))?.ToObject<bool>() ?? false;
            providedConsumables.SC = jObject.GetValue(nameof(ProvidedConsumables.SC))?.ToObject<bool>() ?? false;
            providedConsumables.GIN = jObject.GetValue(nameof(ProvidedConsumables.GIN))?.ToObject<bool>() ?? false;
            providedConsumables.VG = jObject.GetValue(nameof(ProvidedConsumables.VG))?.ToObject<bool>() ?? false;
            providedConsumables.WIN = jObject.GetValue(nameof(ProvidedConsumables.WIN))?.ToObject<bool>() ?? false;
            providedConsumables.NST = jObject.GetValue(nameof(ProvidedConsumables.NST))?.ToObject<bool>() ?? false;
        }

        public static void ReadInfrastructureBuildings(JObject obj, PlanetaryBaseInfrastructure infrastructure)
        {
            infrastructure.HB1.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB1))?.ToObject<int>() ?? 0;
            infrastructure.HB2.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB2))?.ToObject<int>() ?? 0;
            infrastructure.HB3.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB3))?.ToObject<int>() ?? 0;
            infrastructure.HB4.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB4))?.ToObject<int>() ?? 0;
            infrastructure.HB5.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB5))?.ToObject<int>() ?? 0;
            infrastructure.HBB.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HBB))?.ToObject<int>() ?? 0;
            infrastructure.HBC.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HBC))?.ToObject<int>() ?? 0;
            infrastructure.HBM.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HBM))?.ToObject<int>() ?? 0;
            infrastructure.HBL.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HBL))?.ToObject<int>() ?? 0;
            infrastructure.STO.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.STO))?.ToObject<int>() ?? 0;
        }        
        
        public static void ReadExpertAllocation(JObject obj, ExpertAllocation expertAllocation)
        {
            expertAllocation.Agriculture.Count = obj.GetValue(nameof(ExpertAllocation.Agriculture))?.ToObject<int>() ?? 0;
            expertAllocation.Chemistry.Count = obj.GetValue(nameof(ExpertAllocation.Chemistry))?.ToObject<int>() ?? 0;
            expertAllocation.Construction.Count = obj.GetValue(nameof(ExpertAllocation.Construction))?.ToObject<int>() ?? 0;
            expertAllocation.Electronics.Count = obj.GetValue(nameof(ExpertAllocation.Electronics))?.ToObject<int>() ?? 0;
            expertAllocation.FoodIndustries.Count = obj.GetValue(nameof(ExpertAllocation.FoodIndustries))?.ToObject<int>() ?? 0;
            expertAllocation.FuelRefining.Count = obj.GetValue(nameof(ExpertAllocation.FuelRefining))?.ToObject<int>() ?? 0;
            expertAllocation.Manufacturing.Count = obj.GetValue(nameof(ExpertAllocation.Manufacturing))?.ToObject<int>() ?? 0;
            expertAllocation.Metallurgy.Count = obj.GetValue(nameof(ExpertAllocation.Metallurgy))?.ToObject<int>() ?? 0;
            expertAllocation.ResourceExtraction.Count = obj.GetValue(nameof(ExpertAllocation.ResourceExtraction))?.ToObject<int>() ?? 0;
        }
        
        
    }
}