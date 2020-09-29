using System.Collections.Generic;

namespace LG.AbpDtoGenerator.GeneratorModels
{
    public class TranslatorTextList
    {
        public string CultureSortName { get; set; }

        public string CultureDisplayName { get; set; }

        public List<KeyValuePair<string, string>> TransTestList { get; set; }
    }
}