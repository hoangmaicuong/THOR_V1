using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Office.Drawing;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using THOR_V1.Module.DatabaseUpdate;

namespace THOR_V1.Module.BusinessObjects
{
    [System.ComponentModel.DefaultProperty("BaseMaterialName")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class MainSubstrate : BaseObject
    {
        private String mainSubstrateName;

        public MainSubstrate(Session session) : base(session) { }

        [Size(50)]
        [RuleRequiredField("MainSubstrateName", DefaultContexts.Save, "MainSubstrateName")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String MainSubstrateName
        {
            get { return mainSubstrateName; }
            set { mainSubstrateName = value; }
        }

        [Association("FinishList-MainSubstrate", typeof(FinishList))]
        public XPCollection FinishLists
        {
            get
            {
                return GetCollection("FinishLists");
            }
        }
        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
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
            }
            base.OnDeleting();
        }
    }

    [System.ComponentModel.DefaultProperty("Version")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Finishing")]
    public class FVersion : BaseObject
    {
        public FVersion(Session session) : base(session) { }

        [RuleRequiredField("FVersion Code is required", DefaultContexts.Save, "FVersion Code is required")]
        public string Version { get; set; }

        public DateTime VersionDate { get; set; }

        [Association("FinishList-FVersion", typeof(FinishList))]
        public XPCollection FinishLists
        {
            get
            {
                return GetCollection("FinishLists");
            }
        }

    }

    [System.ComponentModel.DefaultProperty("BrushingName")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class Brushing : BaseObject
    {

        public Brushing(Session session) : base(session) { }

        [RuleRequiredField("BrushingName is required", DefaultContexts.Save, "BrushingName is required")]
        public String BrushingName { get; set; }

        public int Position { get; set; }

        [Association("FinishList-Brushing", typeof(FinishList))]
        public XPCollection Brushings
        {
            get { return GetCollection("Brushings"); }
        }

    }

    [System.ComponentModel.DefaultProperty("FinishCode")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    [Appearance("DisableAllFieldsWhenApproved", TargetItems = "*", Enabled = false, Criteria = "Approved = true", Context = "DetailView", Priority = 1)]
    //[Appearance("KeepApprovedEnabled", TargetItems = "Approved", Enabled = true, Context = "DetailView", Priority = 2)]
    public class FinishList : BaseObject
    {
        private String finishCode;
        private String finishName;
        private string maxFinishCode = "";
        private String preVersion = "";
        private String preCust = "";
        public FinishList(Session session) : base(session) { }


        //[RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [RuleRequiredField("SystemName is required", DefaultContexts.Save, "SystemName is required")]
        public String SystemName { get; set; }

        private String systemFilePath;
        public String SystemFilePath
        {
            get { return systemFilePath; }
            set
            {
                systemFilePath = value;
            }
        }

        private MainSubstrate mainSubstrate;


        [Association("FinishList-MainSubstrate", typeof(FinishList))]
        public MainSubstrate MainSubstrate
        {
            get { return mainSubstrate; }
            set
            {
                mainSubstrate = value;
            }
        }


        private FVersion fVersion;

        //[RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Association("FinishList-FVersion", typeof(FinishList))]
        public FVersion Version
        {
            get { return fVersion; }
            set
            {
                fVersion = value;
            }
        }

        private Customer customer;

        //[RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Association("FinishList-Customer", typeof(FinishList))]
        public Customer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
            }
        }

        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [RuleRequiredField("Finish Code is required", DefaultContexts.Save, "Finish Code is required")]
        [Size(20)]
        public String FinishCode
        {
            get
            {
                if (!IsLoading && !IsSaving && !IsDeleted)
                {

                    if (Customer != null && Version != null)
                    {
                        if (preCust != Customer.ShortName)
                        {
                            //MessageBox.Show("preCust : " + preCust + "Customer.ShortName" + Customer.ShortName);
                            DBAccess bs = new DBAccess();
                            maxFinishCode = bs.GetMaxFinishinCode(Customer.ShortName);
                            preCust = Customer.ShortName;
                            finishCode = $"{maxFinishCode}-{Version?.Version}";
                        }
                        else
                        {
                            if (preVersion != Version.Version)
                            {
                                maxFinishCode = finishCode.Replace(preVersion, "");

                                finishCode = $"{maxFinishCode}{Version?.Version}";
                                preVersion = Version.Version;
                            }
                        }



                    }

                }

                return finishCode;
            }
            set
            {
                SetPropertyValue("FinishCode", ref finishCode, value);
            }
        }

        [ValueConverter(typeof(DevExpress.Xpo.Metadata.ImageValueConverter)), Delayed]
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, DetailViewImageEditorMode = ImageEditorMode.PictureEdit, ListViewImageEditorCustomHeight = 40)]
        public Image FImage
        {
            get { return GetDelayedPropertyValue<System.Drawing.Image>("FImage"); }
            set { SetDelayedPropertyValue<System.Drawing.Image>("FImage", value); }

        }

        [Size(100)]
        [RuleRequiredField("Finish Name is required", DefaultContexts.Save, "Finish Name is required")]
        public String FinishName
        {
            get
            {
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    if (!String.IsNullOrEmpty(SystemName) && MainSubstrate != null)
                        finishName = $"{SystemName} ON {MainSubstrate?.MainSubstrateName}";
                }

                return finishName;
            }
            set
            {
                SetPropertyValue("FinishName", ref finishName, value);
            }
        }

        private Brushing brushing;
        [Association("FinishList-Brushing", typeof(FinishList))]
        public Brushing Brushing
        {
            get { return brushing; }
            set { SetPropertyValue("Brushing", ref brushing, value); }
        }

        private FDistressing distressing;
        [Association("FinishList-FDistressing", typeof(FinishList))]
        public FDistressing Distressing
        {
            get { return distressing; }
            set { SetPropertyValue("Distressing", ref distressing, value); }
        }

        [Association("ItemFinishList-FinishList", typeof(ItemFinishList))]
        public XPCollection ItemFinishLists
        {
            get
            {
                return GetCollection("ItemFinishLists");
            }
        }

        //[RuleRequiredField("Finish Description Name is required", DefaultContexts.Save, "Finish Description Name is required")]
        [Size(500)]
        public String OtherSpecs { get; set; }

        public FinishSheen Sheen { get; set; }



        public DryFilmThickness DFT { get; set; }


        [Size(250)]
        public String VersionNotes { get; set; }

        private Supplier developedBy;
        public Supplier DevelopedBy
        {
            get => developedBy;
            set => SetPropertyValue(nameof(DevelopedBy), ref developedBy, value);
        }

        private DateTime developedDate;
        public DateTime DevelopedDate
        {
            get => developedDate;
            set => SetPropertyValue(nameof(DevelopedDate), ref developedDate, value);
        }

        private THInCharge thIncharge;
        public THInCharge THIncharge
        {
            get => thIncharge;
            set => SetPropertyValue(nameof(THIncharge), ref thIncharge, value);
        }


        //private bool approved;
        //public bool Approved
        //{
        //    get => approved;
        //    set
        //    {
        //        if (SetPropertyValue(nameof(Approved), ref approved, value))
        //        {
        //            if (approved)
        //            {
        //                approvedBy = SecuritySystem.CurrentUserName;
        //                approvedDate = DateTime.Now;

        //            }
        //            else
        //            {
        //                approvedBy = "";
        //                approvedDate = DateTime.MinValue;
        //            }

        //        }
        //    }
        //}

        private String approvedBy;
        public String ApprovedBy
        {
            get => approvedBy;
            set => SetPropertyValue(nameof(ApprovedBy), ref approvedBy, value);
        }

        private DateTime approvedDate;
        public DateTime ApprovedDate
        {
            get => approvedDate;
            set => SetPropertyValue(nameof(ApprovedDate), ref approvedDate, value);
        }

        private DateTime unApprovedDate;
        public DateTime UnapprovedDate
        {
            get => unApprovedDate;
            set => SetPropertyValue(nameof(UnapprovedDate), ref unApprovedDate, value);
        }



        private bool active;
        public bool Active
        {
            get => active;
            set => SetPropertyValue(nameof(Active), ref active, value);
        }

        private DateTime createDate;
        public DateTime CreateDate
        {
            get => createDate;
            set => SetPropertyValue(nameof(CreateDate), ref createDate, value);
        }

        //[Association("Item-FinishList", typeof(Item))]
        //public XPCollection Items
        //{
        //    get { return GetCollection("Items"); }
        //}

        [Association("FinishList-FinishProcess", typeof(FinishProcess))]
        public XPCollection FinishProcesss
        {
            get
            {
                return GetCollection("FinishProcesss");
            }
        }

        [Association("FinishList-FinishPanel", typeof(FinishPanel))]
        public XPCollection FinishPanels
        {
            get { return GetCollection("FinishPanels"); }
        }



        public override void AfterConstruction()
        {

            base.AfterConstruction();

            if (developedDate == DateTime.MinValue)
                developedDate = DateTime.Now;

            if (createDate == DateTime.MinValue)
                createDate = DateTime.Now;

            active = true;

            CriteriaOperator filter = new BinaryOperator("Version", "V10");
            XPCollection<FVersion> u = new XPCollection<FVersion>(Session, filter, null);
            if (u.Count > 0)
            {
                u.TopReturnedObjects = 1;
                fVersion = u[0];
            }

            CriteriaOperator filter2 = new BinaryOperator("ShortName", "SAR");
            XPCollection<Customer> u2 = new XPCollection<Customer>(Session, filter2, null);
            if (u2.Count > 0)
            {
                u2.TopReturnedObjects = 1;
                customer = u2[0];
            }

            if (Customer != null && Version != null)
            {
                preCust = Customer?.ShortName;
                preVersion = Version?.Version;
            }

        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            preCust = Customer?.ShortName;
            preVersion = Version?.Version;

            //MessageBox.Show("preCust : " + preCust + "preVersion :" + preVersion);
        }


        protected override void OnSaving()
        {

            String destinationFile = "";
            String destinationFolder = "";

            //if (unApprovedDate == DateTime.MinValue && approvedDate != DateTime.MinValue && approved == false)
            //{
            //    unApprovedDate = DateTime.Now;
            //    approvedDate = DateTime.MinValue;
            //}

            if (!String.IsNullOrEmpty(SystemFilePath))
            {
                string sourceFile = SystemFilePath.ToString().Trim();
                destinationFolder = ConfigurationManager.AppSettings["FNSFilePath"];
                destinationFile = destinationFolder + @"\" + Path.GetFileName(sourceFile);
                //MessageBox.Show("SystemFilePath: " + destinationFile);
                SystemFilePath = destinationFile;

                ClassAccessFolder _acc = new ClassAccessFolder();
                _acc.CopyFileFromLocalToServer(sourceFile, destinationFile, "phuongnv", "talentohouse.local", "Panda@123");
                try
                {
                    File.Copy(sourceFile, destinationFile, true);
                }
                catch (Exception ex)
                {
                    
                }

            }

            base.OnSaving();


        }
    }
    [System.ComponentModel.DefaultProperty("Distressing")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class FDistressing : BaseObject
    {

        public FDistressing(Session session) : base(session) { }

        [RuleRequiredField("Distressing is required", DefaultContexts.Save, "BrushingName is required")]
        public String Distressing { get; set; }

        public int Position { get; set; }

        [Association("FinishList-FDistressing", typeof(FinishList))]
        public XPCollection FDistressings
        {
            get { return GetCollection("FDistressings"); }
        }

    }
    [System.ComponentModel.DefaultProperty("FinishListID.FinishCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Finishing")]
    public class ItemFinishList : BaseObject
    {
        private Item itemID;
        private FinishList finishListID;
        private String comments;
        public ItemFinishList(Session session)
            : base(session)
        {
        }

        [Association("ItemFinishList-Item", typeof(Item))]
        public Item ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }
        //       [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Association("ItemFinishList-FinishList", typeof(FinishList))]
        public FinishList FinishListID
        {
            get { return finishListID; }
            set { finishListID = value; }
        }

        private AppliedOn appliedOn;
        public AppliedOn AppliedOn
        {
            get { return appliedOn; }
            set { appliedOn = value; }
        }

        [Size(200)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private bool isMain;
        public bool IsMain
        {
            get { return isMain; }
            set { isMain = value; }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            if (isMain == true)
            {
                DBAccess _db = new DBAccess();
                int nRst = _db.UpdateItemFinishCode(ItemID.Oid.ToString(), FinishListID.Oid.ToString());
            }
        }
    }

    [System.ComponentModel.DefaultProperty("Sheen")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class FinishSheen : BaseObject
    {

        public FinishSheen(Session session) : base(session) { }

        [RuleRequiredField("FinishSheen is required", DefaultContexts.Save, "FinishSheen is required")]
        public int? Sheen { get; set; }

    }
    [System.ComponentModel.DefaultProperty("DFT")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class DryFilmThickness : BaseObject
    {

        public DryFilmThickness(Session session) : base(session) { }

        [RuleRequiredField("DryFilmThickness DTF is required", DefaultContexts.Save, "DryFilmThickness DTF is required")]
        public string DFT { get; set; }
        public int SortList { get; set; }
    }
    [System.ComponentModel.DefaultProperty("FinishCode")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class FinishProcess : BaseObject
    {
        private int step;

        public FinishProcess(Session session) : base(session) { }

        [Association("FinishList-FinishProcess", typeof(FinishList))]
        public FinishList FinishCode { get; set; }

        public int Step
        {
            get { return step; ; }
            set { step = value; }
        }

        private ProcessStep processStep;

        [Association("ProcessStep-FinishProcess", typeof(ProcessStep))]
        public ProcessStep ProcessStep
        {
            get => processStep;
            set => SetPropertyValue(nameof(ProcessStep), ref processStep, value);
        }


        [Association("MixFormula-FinishProcess", typeof(MixFormula))]
        public MixFormula MixFormulaID { get; set; }

        public DryTime DryMin { get; set; }

        [RuleRequiredField("FinishProcess Operation is required", DefaultContexts.Save, "FinishProcess Operation is required")]
        [Association("OperationList-FinishProcess", typeof(OperationList))]
        public OperationList OperationListID { get; set; }

        //[PersistentAlias(@"Op min/m2")]
        public int OpMinMet { get; set; }

        //[PersistentAlias(@"gr/m2")]
        public int GrMet { get; set; }

        //public int Potlife { get; set; }

    }
    [System.ComponentModel.DefaultProperty("DryMin")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class FinishPanel : BaseObject
    {
        private string makeBy;
        private DateTime makeDate;
        private bool approved;
        private string approvedBy;

        public FinishPanel(Session session) : base(session) { }

        public String PanelCode { get; set; }

        [Association("SageItem-FinishPanel", typeof(SageItem))]
        public SageItem SubstrateMaterial { get; set; }

        [Association("FinishList-FinishPanel", typeof(FinishList))]
        public FinishList FinishCode { get; set; }

        public string MakeBy
        {
            get { return makeBy; }
            set { makeBy = value; }
        }

        public DateTime MakeDate
        {
            get { return makeDate; }
            set { makeDate = value; }
        }

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }
        public string ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        public int Copies { get; set; }

        public bool Retired { get; set; }

        [Size(500)]
        public String ReasonForRetirement { get; set; }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            if (makeDate == DateTime.MinValue)
                makeDate = DateTime.Now;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (approved)
            {
                if (string.IsNullOrEmpty(approvedBy))
                {
                    approvedBy = SecuritySystem.CurrentUserName;
                }
            }
            else
                approvedBy = "";
        }

    }
    [System.ComponentModel.DefaultProperty("FinishListID.FinishCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Finishing")]
    public class AppliedOn : BaseObject
    {
        private string applyName;

        private String comments;
        public AppliedOn(Session session) : base(session) { }

        public string ApplyName
        {
            get { return applyName; }
            set { applyName = value; }
        }

        [Size(250)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
    }
    [System.ComponentModel.DefaultProperty("FinishCode")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class ProcessStep : BaseObject
    {
        public ProcessStep(Session session) : base(session) { }

        public String ProcessName { get; set; }

        [Association("ProcessStep-FinishProcess", typeof(FinishProcess))]
        public XPCollection FinishProcesss
        {
            get
            {
                return GetCollection("FinishProcesss");
            }
        }

    }
    [System.ComponentModel.DefaultProperty("MixListID")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    [DefaultListViewOptions(true, NewItemRowPosition.Bottom)]
    public class MixFormula : BaseObject
    {

        public MixFormula(Session session) : base(session) { }

        [RuleRequiredField("MixFormula MixListID is required", DefaultContexts.Save, "MixFormula MixListID is required")]
        [Association("MixList-MixFormula", typeof(MixList))]
        public MixList MixCode { get; set; }

        public SageItem Material { get; set; }
        public Double Blend { get; set; }

        public Double MixingRatioGram { get; set; }

        public Double MixingRatioPercent { get; set; }

        public Double Consumption_m2 { get; set; }

        [Association("MixFormula-FinishProcess", typeof(FinishProcess))]
        public XPCollection FinishProcesss
        {
            get
            {
                return GetCollection("FinishProcesss");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("DryMin")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class DryTime : BaseObject
    {

        public DryTime(Session session) : base(session) { }

        [RuleRequiredField("DryTime Dry Minnute is required", DefaultContexts.Save, "DryTime Dry Minnute is required")]
        public int? DryMin { get; set; }

        [RuleRequiredField("DryTime DryHours is required", DefaultContexts.Save, "DryTime DryHours is required")]
        public int? DryHours { get; set; }
    }
    [System.ComponentModel.DefaultProperty("Operation")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class OperationList : BaseObject
    {

        public OperationList(Session session) : base(session) { }

        [RuleRequiredField("OperationList Operation is required", DefaultContexts.Save, "OperationList Operation is required")]
        public string Operation { get; set; }

        [Size(250)]
        public String OperationDescription { get; set; }

        public bool MixOperation { get; set; }

        public int SortList { get; set; }

        [Association("OperationList-FinishProcess", typeof(FinishProcess))]
        public XPCollection FinishProcesss
        {
            get
            {
                return GetCollection("FinishProcesss");
            }
        }

    }
    [System.ComponentModel.DefaultProperty("MixCode")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class MixList : BaseObject
    {
        private String mixMatCodesString;
        private Double totalBlend;

        public MixList(Session session) : base(session) { }

        [RuleRequiredField("MixList MixCode is required", DefaultContexts.Save, "MixList MixCode is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public string MixCode { get; set; }

        public WetFilmThickness WFT { get; set; }

        public ViscosityList Viscosity { get; set; }

        public String PotLife { get; set; }

        [Persistent]
        public String MixMatCodesString
        {
            get
            {
                if (this.Oid != null && !IsLoading && !IsSaving && !IsDeleted)
                {
                    String strRst = "";
                    CriteriaOperator filter = new BinaryOperator("MixListID", this.Oid);
                    XPCollection<MixFormula> MixFs = new XPCollection<MixFormula>(Session, filter);

                    foreach (MixFormula mf in MixFs)
                    {
                        if (mf.Material != null)
                        {
                            if (String.IsNullOrEmpty(strRst))
                                strRst = mf.Material.SupplierCode;
                            else
                                strRst = strRst + mf.Material.SupplierCode;
                        }

                    }
                    mixMatCodesString = strRst;
                }

                return mixMatCodesString;
            }
        }


        private String mixMatAccpacCodesString;
        [Persistent]
        public String MixMatAccpacCodesString
        {
            get
            {
                if (this.Oid != null && !IsLoading && !IsSaving && !IsDeleted)
                {
                    totalBlend = 0;
                    String strRst = "";
                    CriteriaOperator filter = new BinaryOperator("MixListID", this.Oid);
                    XPCollection<MixFormula> MixFs = new XPCollection<MixFormula>(Session, filter);

                    foreach (MixFormula mf in MixFs)
                    {
                        if (mf.Material != null)
                        {
                            if (String.IsNullOrEmpty(strRst))
                            {
                                strRst = mf.Material.ITEMNO + '-' + mf.Blend.ToString();
                            }
                            else
                            {
                                strRst = strRst + "@" + mf.Material.ITEMNO + '-' + mf.Blend.ToString();
                            }
                        }
                        totalBlend = totalBlend + mf.Blend;
                    }
                    mixMatAccpacCodesString = strRst;

                }

                return mixMatAccpacCodesString;
            }

        }


        [Persistent]
        public Double TotalBlend
        {
            get
            {
                if (this.Oid != null && !IsLoading && !IsSaving && !IsDeleted)
                {
                    Double total = 0;
                    CriteriaOperator filter = new BinaryOperator("MixListID", this.Oid);
                    XPCollection<MixFormula> MixFs = new XPCollection<MixFormula>(Session, filter);

                    foreach (MixFormula mf in MixFs)
                    {
                        total = total + mf.Blend;
                    }
                    totalBlend = total;
                }
                return totalBlend;
            }
        }

        [Association("MixList-MixFormula", typeof(MixFormula))]
        public XPCollection MixFormulas
        {
            get
            {
                return GetCollection("MixFormulas");
            }
        }

        /*private bool isCorrectSaved;
        protected override void OnSaving()
        {
            isCorrectSaved = true;
            if (this.Oid != new Guid())            
            {
                string MixExist = "";
                BusinessExecute _exc = new BusinessExecute();
                MixExist = _exc.CheckExistMixList(this.Oid.ToString());

                if (!string.IsNullOrEmpty(MixExist))
                    isCorrectSaved = false;
            }

            if (isCorrectSaved == true)
                base.OnSaving();
            else
                System.Windows.Forms.MessageBox.Show("The Mix Fomular is Exist");
        }

        protected override void OnSaved()
        {
            if (isCorrectSaved == true)
                base.OnSaved();
        }*/
    }
    [System.ComponentModel.DefaultProperty("WTF")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class WetFilmThickness : BaseObject
    {

        public WetFilmThickness(Session session) : base(session) { }

        [RuleRequiredField("WetFilmThickness WTF is required", DefaultContexts.Save, "WetFilmThickness WTF is required")]
        public string WTF { get; set; }
        public int SortList { get; set; }
    }
    [System.ComponentModel.DefaultProperty("ViscosityName")]
    [DefaultClassOptions, ImageName("Action_ChooseSkin")]
    [NavigationItem(GroupName = "Finishing")]
    public class ViscosityList : BaseObject
    {

        public ViscosityList(Session session) : base(session) { }

        [RuleRequiredField("ViscosityList ViscosityName is required", DefaultContexts.Save, "ViscosityList ViscosityName is required")]
        public string ViscosityName { get; set; }

        public bool SortList { get; set; }
    }
}