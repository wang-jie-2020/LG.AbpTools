﻿using System.Collections.Generic;

namespace LG.AbpDtoGenerator.CodeAnalysis.TranslationServices
{
    public class TransRoot
    {
        public string to { get; set; }

        public List<Trans_resultItem> trans_result { get; set; }
    }
}