using LG.AbpDtoGenerator.Models;
using LG.AbpDtoGenerator.ViewModels;

namespace LG.AbpDtoGenerator.LGFeature
{
    public static class Global
    {
        public static string SolutionPath { get; set; }

        public static SolutionInfoModel SolutionInfo { get; set; }

        public static MainPageViewModel MainViewModel { get; set; }

        public static PropertySelectorPageModel PropertyViewModel { get; set; }

        public static BasicOptionCfg Option { get; set; }

        public static EntityModel Entity { get; set; }

        public static LGOptionCfg LGOption { get; set; }
    }
}
