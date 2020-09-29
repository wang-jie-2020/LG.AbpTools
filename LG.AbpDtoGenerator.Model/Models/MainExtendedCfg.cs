using LG.AbpDtoGenerator.Base;

namespace LG.AbpDtoGenerator.Models
{
    public class MainExtendedCfg : BaseViewModel
    {
        public bool IsXstSolution
        {
            get
            {
                return this.isXstSolution;
            }
            set
            {
                this.isXstSolution = value;
                base.InvokePropertyChanged("IsXstSolution");
            }
        }

        private bool isXstSolution;
    }
}