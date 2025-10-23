using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace THOR_V1.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("BO_Product")]
    [System.ComponentModel.DefaultProperty("TagNum")]
    [DefaultListViewOptions(true, NewItemRowPosition.None)]
    [NavigationItem(GroupName = "Turn Over Slip")]
    public class Tag : BaseObject //XPBaseObject
    {
        private Item itemID;

        //     private SampleItem sampleItemID;
        private PODetail poDetailID;
        private Supplier supplierID;
        private String comments;
        private bool isWithComp;
        private bool tagShipped;
        private bool isGetTagTOWH;
        private DateTime createDate;
        //   private SODetailItem soDetailItemID;

        public Tag(Session session) : base(session) { }


        private int _tag;
        public int TagNum
        {
            get => _tag;
            set => SetPropertyValue(nameof(TagNum), ref _tag, value);
        }

        [RuleRequiredField("RuleRequiredField for Tag ItemCode.", DefaultContexts.Save)]
        [Association("Tag-Item", typeof(Item))]
        public Item ItemID
        {
            get { return itemID; }
            set
            {
                itemID = value;
                //phuongnv rem 2024-07-12 ***************
                /*if (!IsLoading)
                {
                    OnChanged("ItemID");
                    UpdatePOList();
                }*/
            }
        }

        public Item ComponentID { get; set; }

        public Item WOODID { get; set; }

        //[Persistent]
        //[DataSourceProperty("PODataSource")] //phuongnv rem 2024-07-12 ***************
        [Association("Tag-PODetail", typeof(PODetail))]
        public PODetail PODetailID
        {
            get { return poDetailID; }
            set { poDetailID = value; }
        }
        public bool IsWithComp
        {
            get
            {
                return isWithComp;
            }
            set
            {
                if (isWithComp == value)
                    return;
                isWithComp = value;
            }
        }
        [Association("Tag-Supplier", typeof(Supplier))]
        public Supplier SupplierID
        {
            get { return supplierID; }
            set { supplierID = value; }
        }

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        private string commentsPO;
        public String CommentsPO
        {
            get { return commentsPO; }
            set { commentsPO = value; }
        }
        private string planToShip;
        public String PlanToSip
        {
            get { return planToShip; }
            set { planToShip = value; }
        }


        public bool IsTagShipped { get; set; }
        public bool IsGetTagTOWH
        {
            get { return isGetTagTOWH; }
            set { isGetTagTOWH = value; }
        }
        public bool TagShipped
        {
            get { return tagShipped; }
            set { tagShipped = value; }
        }
        private String location;
        public String Location
        {
            get { return location; }
            set { location = value; }
        }

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            createDate = DateTime.Today;
        }
        //********************************************************
        //Create by: phuongnv
        //Create date: 24/03/2016
        //Description: Allocate Tag when QC check ok
        //********************************************************
        public bool IsCompleteSet { get; set; }

        public bool IsPackingFinished { get; set; }

        protected override void OnDeleting()
        {
            /*foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)  //mi.IsAggregated &&
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }*/
            base.OnDeleting();
        }


        //********************************************************
        //Create by: phuongnv
        //Create date: 01/09/2016
        //Description: When user change Item code, write tracking for alerting to WH
        //********************************************************
        protected override void OnSaved()
        {
            string strItemID = "";
            base.OnSaved();
            if (itemID != null)
            {
                strItemID = itemID.Oid.ToString();
            }
        }

        [Association("TOSStock-Tag", typeof(TOSStock))]
        public XPCollection TOSStocks
        {
            get
            {
                return GetCollection("TOSStocks");
            }
        }

        /*[Association("Tag-OutputCBM", typeof(OutputCBM))]
        public XPCollection OutputCBMs
        {
            get
            {
                return GetCollection("OutputCBMs");
            }
        }*/

        [Association("TOSSUP-Tag", typeof(TOSSUP))]
        public XPCollection TOSSUPs
        {
            get
            {
                return GetCollection("TOSSUPs");
            }
        }

        [Association("TOSSUPITW-Tag", typeof(TOSSUPITW))]
        public XPCollection TOSSUPITWs
        {
            get
            {
                return GetCollection("TOSSUPITWs");
            }
        }

        [Association("TOSITWSUP-Tag", typeof(TOSITWSUP))]
        public XPCollection TOSITWSUPs
        {
            get
            {
                return GetCollection("TOSITWSUPs");
            }
        }

        [Association("TOSITWWIP-Tag", typeof(TOSITWWIP))]
        public XPCollection TOSITWWIPs
        {
            get
            {
                return GetCollection("TOSITWWIPs");
            }
        }

        [Association("TOSWIPITW-Tag", typeof(TOSWIPITW))]
        public XPCollection TOSWIPITWs
        {
            get
            {
                return GetCollection("TOSWIPITWs");
            }
        }

        [Association("TOSWIPQC-Tag", typeof(TOSWIPQC))]
        public XPCollection TOSWIPQCs
        {
            get
            {
                return GetCollection("TOSWIPQCs");
            }
        }

        [Association("TOSQCWIP-Tag", typeof(TOSQCWIP))]
        public XPCollection TOSQCWIPs
        {
            get
            {
                return GetCollection("TOSQCWIPs");
            }
        }

        [Association("TOSQCPACK-Tag", typeof(TOSQCPACK))]
        public XPCollection TOSQCPACKs
        {
            get
            {
                return GetCollection("TOSQCPACKs");
            }
        }

        [Association("TOSPACKQC-Tag", typeof(TOSPACKQC))]
        public XPCollection TOSPACKQCs
        {
            get
            {
                return GetCollection("TOSPACKQCs");
            }
        }

        [Association("TOSPACKFG-Tag", typeof(TOSPACKFG))]
        public XPCollection TOSPACKFGs
        {
            get
            {
                return GetCollection("TOSPACKFGs");
            }
        }

        [Association("TOSFGPACK-Tag", typeof(TOSFGPACK))]
        public XPCollection TOSFGPACKs
        {
            get
            {
                return GetCollection("TOSFGPACKs");
            }
        }

        [Association("TOSFGShip-Tag", typeof(TOSFGShip))]
        public XPCollection TOSFGShips
        {
            get
            {
                return GetCollection("TOSFGShips");
            }
        }

        [Association("ShipmentDetailPOPacking-Tag", typeof(ShipmentDetailPOPacking))]
        public XPCollection ShipmentDetailPOPackings
        {
            get
            {
                return GetCollection("ShipmentDetailPOPackings");
            }
        }

        //[Association("CreateShipment-Tag", typeof(CreateShipment))]
        //public XPCollection CreateShipments
        //{
        //    get
        //    {
        //        return GetCollection("CreateShipments");
        //    }
        //}



    }

    [System.ComponentModel.DefaultProperty("ShipmentDetailID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class ShipmentDetailPOPacking : BaseObject
    {
        private ShipmentDetailPO shipmentDetailPOID;
        private Tag tagID;

        public ShipmentDetailPOPacking(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("ShipmentDetailPOPacking is required", DefaultContexts.Save, "ShipmentDetail is required")]
        [Association("ShipmentDetailPOPacking-ShipmentDetailPO", typeof(ShipmentDetailPO))]
        public ShipmentDetailPO ShipmentDetailPOID
        {
            get { return shipmentDetailPOID; }
            set
            {
                shipmentDetailPOID = value;
            }
        }
        [Association("ShipmentDetailPOPacking-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }

    }

    //----------FGShip
    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSFGShip Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSFGShip QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSFGShip IsLocked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSFGShip : BaseObject
    {
        private Tag tagID;
        private String comments;


        public TOSFGShip(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSFGShip Tag.", DefaultContexts.Save)]
        [Association("TOSFGShip-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public int QtyStock { get; set; }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;

                /*
                CriteriaOperator fil = new BinaryOperator("WorkingDate", this.EntryDate);
                XPCollection<DayEndLog> cp = new XPCollection<DayEndLog>(Session, fil, null);
                foreach (DayEndLog c in cp)
                {
                    ret = true;//c.IsLocked;
                }
                */
                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }
    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSFGPACK Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSFGPACK QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSFGPACK Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSFGPACK : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSFGPACK(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSFGPACK Tag.", DefaultContexts.Save)]
        [Association("TOSFGPACK-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSPACKFG Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSPACKFG QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSPACKFG Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSPACKFG : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSPACKFG(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSPACKFG Tag.", DefaultContexts.Save)]
        [Association("TOSPACKFG-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSPACKQC Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSPACKQC QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSPACKQC Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSPACKQC : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSPACKQC(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSPACKQC Tag.", DefaultContexts.Save)]
        [Association("TOSPACKQC-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSQCPACK Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSQCPACK QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSQCPACK Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSQCPACK : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSQCPACK(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSQCPACK Tag.", DefaultContexts.Save)]
        [Association("TOSQCPACK-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSQCWIP Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSQCWIP QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSQCWIP Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSQCWIP : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSQCWIP(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSQCWIP Tag.", DefaultContexts.Save)]
        [Association("TOSQCWIP-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSWIPQC Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSWIPQC QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSWIPQC Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSWIPQC : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSWIPQC(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSWIPQC Tag.", DefaultContexts.Save)]
        [Association("TOSWIPQC-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSWIPITW Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSWIPITW QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSWIPITW Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSWIPITW : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSWIPITW(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSWIPITW Tag.", DefaultContexts.Save)]
        [Association("TOSWIPITW-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSITWWIP Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSITWWIP QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSITWWIP Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSITWWIP : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSITWWIP(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSITWWIP Tag.", DefaultContexts.Save)]
        [Association("TOSITWWIP-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSITWSUP Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSITWSUP QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSITWSUP Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSITWSUP : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSITWSUP(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSITWSUP Tag.", DefaultContexts.Save)]
        [Association("TOSITWSUP-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }


    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSSUPITW Stock Qty Criteria", DefaultContexts.Save, "QtyStock = 1 ", "TOSSUPITW QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSSUPITW Is Locked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSSUPITW : BaseObject
    {
        private Tag tagID;
        //private DateTime EntryDate;
        private String comments;


        public TOSSUPITW(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOSSUPITW Tag.", DefaultContexts.Save)]
        [Association("TOSSUPITW-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;
        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private int qtyStock;
        public int QtyStock
        {
            get { return qtyStock; }
            set { qtyStock = value; }
        }

        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;


                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
                userName = SecuritySystem.CurrentUserName;

            base.OnSaving();
        }


    }

    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("TagID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    [RuleCriteria("TOSSUP Stock Qty Criteria", DefaultContexts.Save, "QtyStock >= 0 AND QtyStock <= 1 ", "TOSSUP QtyStock is not equal 1  or duplicate transaction.", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TOSSUP isLocked Criteria", DefaultContexts.Save, "IsLocked=false ", "Tag DayEndLog is Locked.", SkipNullOrEmptyValues = false)]
    public class TOSSUP : BaseObject
    {
        private Tag tagID;
        private String comments;

        public TOSSUP(Session session) : base(session) { }

        [RuleRequiredField("RuleRequiredField for TOSSUP Tag.", DefaultContexts.Save)]
        [Association("TOSSUP-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }
        DateTime entryDate;//= DateTime.Today;

        [Persistent]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }

        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        int qtystock = 0;
        public int QtyStock
        {
            get
            {

                //int qtysupplier = 0;
                //int qtysupwip = 0;
                //int qtywipsup = 0;


                //if (this.Oid != null)
                //    if (Session.Connection != null)
                //    {

                //        CriteriaOperator filterpo = new BinaryOperator("TagID", this.TagID);
                //        filterpo = filterpo & new BinaryOperator("AreaID", 1);
                //        filterpo = filterpo & CriteriaOperator.Parse("DateDiffDay(EntryDate,?)=0", this.EntryDate);
                //        XPCollection<TOSStock> cpdp = new XPCollection<TOSStock>(Session, filterpo, null);
                //        cpdp.TopReturnedObjects = 1;
                //        qtystock = cpdp.Count;

                //        CriteriaOperator filter = new BinaryOperator("TagID", this.TagID);
                //        filter = filter & CriteriaOperator.Parse("DateDiffDay(EntryDate,?)=0", this.EntryDate);

                //        if (qtystock < 1)
                //        {
                //            XPCollection<TOSSUP> ss = new XPCollection<TOSSUP>(Session, filter, null);
                //            qtysupplier = ss.Count;
                //        }


                //        XPCollection<TOSSUPWIP> ss1 = new XPCollection<TOSSUPWIP>(Session, filter, null);
                //        qtysupwip = ss1.Count;

                //        XPCollection<TOSWIPSUP> ss2 = new XPCollection<TOSWIPSUP>(Session, filter, null);
                //        qtywipsup = ss2.Count;




                //    }
                return qtystock;
            }
            set => qtystock = value;
        }
        public bool IsLocked
        {
            get
            {
                bool ret = false;

                if ((DateTime.Now - this.EntryDate).Days != 0)
                    ret = true;
                else
                    ret = false;

                /*
                CriteriaOperator fil = new BinaryOperator("WorkingDate", this.EntryDate,BinaryOperatorType.Less);
                XPCollection<DayEndLog> cp = new XPCollection<DayEndLog>(Session, fil, null);
                foreach (DayEndLog c in cp)
                {
                    ret = true;//c.IsLocked;
                }
                */
                return ret;
            }
        }

        [Size(100)]
        private String userName;
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            entryDate = DateTime.Today;

            if (string.IsNullOrEmpty(comments))
                comments = "Transaction manually by WHS";
        }

        protected override void OnSaving()
        {
            if (String.IsNullOrEmpty(userName))
            {
                userName = SecuritySystem.CurrentUserName;

                base.OnSaving();
            }
        }
    }


    [DefaultClassOptions, ImageName("Forms")]
    [System.ComponentModel.DefaultProperty("AreaID")]
    [NavigationItem(GroupName = "Turn Over Slip")]
    public class TOSStock : BaseObject
    {
        private DateTime entryDate;
        private Area? areaID;
        private Tag tagID;
        private int qty;
        //private PODetail poDetailID;

        public TOSStock(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("RuleRequiredField for TOS Stock EntryDate.", DefaultContexts.Save)]
        [Indexed]
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        [RuleRequiredField("RuleRequiredField for TOS Stock AreaID.", DefaultContexts.Save)]
        public Area? AreaID
        {
            get { return areaID; }
            set { areaID = value; }
        }

        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        [RuleRequiredField("RuleRequiredField for TOSStock Item.", DefaultContexts.Save)]
        [Association("TOSStock-Tag", typeof(Tag))]
        public Tag TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }

        private bool isShip;
        [Persistent]
        public bool IsShip { get { return isShip; } }
    }
}
public enum Area
{
    SUP = 1,
    ITW = 2,
    WIP = 3,
    QC = 4,
    PACK = 5,
    FG = 6,
};