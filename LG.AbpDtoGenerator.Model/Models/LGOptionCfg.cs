using LG.AbpDtoGenerator.Base;

namespace LG.AbpDtoGenerator.Models
{
    public class LGOptionCfg : BaseViewModel
    {
        public bool IsLGFeature
        {
            get
            {
                return this.isLGFeature;
            }
            set
            {
                this.isLGFeature = value;
                base.InvokePropertyChanged("IsLGFeature");
            }
        }

        private bool isLGFeature = true;
    }
}