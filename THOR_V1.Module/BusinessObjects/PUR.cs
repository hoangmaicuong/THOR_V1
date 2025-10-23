using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THOR_V1.Module.BusinessObjects
{
    [System.ComponentModel.DefaultProperty("ITEMNO")]
    [DefaultClassOptions, ImageName("Forms")]
    [NavigationItem(GroupName = "Purchasing Order")]
    public class SageItem : BaseObject
    {
        private string itemNo;
        private string suppierCode;
        //private string segmentCode;
        //private string nameofSegmentCode;
        private string vendorID;
        //private string category;
        private string description;


        private string currencyAcc;
        private bool accpacActiveStatus;

        //private Double lastestPrice;
        private Double lastestUSDPrice;

        //private Double latestCost;
        //private Double latestCost1;
        private Double wastage;

        //private Component componentID;
        public SageItem(Session session) : base(session) { }

        [RuleRequiredField("AccpacMaterial is required", DefaultContexts.Save, "AccpacMaterial is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Indexed]
        public string ITEMNO
        {
            get
            {
                return itemNo;
            }
            set
            {
                itemNo = value;
            }
        }

        [Indexed]
        [Size(250)]
        public string SupplierCode
        {
            get
            {
                return suppierCode;
            }
            set
            {
                suppierCode = value;
            }
        }


        [RuleRequiredField("AccpacMaterial Description is required", DefaultContexts.Save, "AccpacMaterial Description is required")]
        [Size(500)]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }

        }

        private Double qtyStock;
        public Double QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        private string binNum;
        public string BIN
        {
            get { return binNum; }
            set { binNum = value; }
        }

        private Units stockUM;
        public Units StockUM
        {
            get { return stockUM; }
            set { stockUM = value; }
        }

        public bool AccpacActiveStatus
        {
            get { return accpacActiveStatus; }
            set { accpacActiveStatus = value; }
        }

        public bool UpdateManualy { get; set; }
        public Double AccpacStandardCost { get; set; }

        public string CurrencyAcc
        {
            get
            {
                return currencyAcc;
            }
            set
            {
                currencyAcc = value;
            }
        }

        private Units costingUM;
        public Units CostingUM
        {
            get { return costingUM; }
            set { costingUM = value; }
        }

        public Double Cost_UMFactor { get; set; }

        public Double Cost_UMCost { get; set; }

        public Double SAGECategory { get; set; }

        public string VendorID
        {
            get
            {
                return vendorID;
            }
            set
            {
                vendorID = value;
            }
        }

        private DateTime createDate;
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        private String createdBy;
        public String CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }



        [Custom("EditMask", "###,##0.###")]
        public Double LatestUSDPrice
        {
            get { return lastestUSDPrice; }
            set { lastestUSDPrice = value; }
        }

        [Custom("EditMask", "###,##0.####")]
        public Double Wastage
        {
            get
            {
                return wastage;
            }
            set
            {
                wastage = value;
            }
        }

        [Association("SageItem-FinishPanel", typeof(FinishPanel))]
        public XPCollection FinishPanels
        {
            get { return GetCollection("FinishPanels"); }
        }

        protected override void OnDeleting()
        {

            base.OnDeleting();
        }



        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (createDate == DateTime.MinValue)
                createDate = DateTime.Now;

            if (String.IsNullOrEmpty(createdBy))
                createdBy = SecuritySystem.CurrentUserName;
            accpacActiveStatus = true;
        }


    }
}
