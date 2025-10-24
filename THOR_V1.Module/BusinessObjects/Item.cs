using Azure;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Office.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using THOR_V1.Module.DatabaseUpdate;
using static DevExpress.RichEdit.Core.Accessibility.TextModel;

namespace THOR_V1.Module.BusinessObjects
{
    [DefaultProperty(nameof(Name))]
    public abstract class Category : BaseObject, ITreeNode
    {

        protected abstract ITreeNode Parent
        {
            get;
        }
        protected abstract IBindingList Children
        {
            get;
        }
        public Category(Session session) : base(session) { }
        string _name;
        //[XafDisplayName("")]
        //[ToolTip("")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [ModelDefault(nameof(IModelMember.AllowEdit), "False")]
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }




        #region ITreeNode
        IBindingList ITreeNode.Children
        {
            get
            {
                return Children;
            }
        }
        string ITreeNode.Name
        {
            get
            {
                return Name;
            }
        }
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return Parent;
            }
        }
        #endregion
    }

    //------***---------------------------------
    [DefaultClassOptions]
    [NavigationItem(GroupName = "Master Lists")]
    [ImageName("BO_Product")]
    [DefaultProperty(nameof(ItemCode))]
    [Appearance("HideItemFields_When_ForComponent", AppearanceItemType = "ViewItem", TargetItems = nameof(BrandListID) + ";" + nameof(ItemClassID), Criteria = "ItemMode = 'ForComponent'", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    [Appearance("HideItemFields_When_ForItem", AppearanceItemType = "ViewItem", TargetItems = nameof(ComponentCategoryID), Criteria = "ItemMode = 'ForItem'", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    [Appearance("LockItemFields_When_Edit", AppearanceItemType = "ViewItem", TargetItems = nameof(ItemMode), Criteria = "IsEdit = true", Enabled = false, Context = "DetailView")]

    public class Item : BaseObject, ITreeNode
    { 
        public Item(Session session)
            : base(session)
        {
        }
        private bool isEdit;
        [ImmediatePostData]
        [NonPersistent]
        public bool IsEdit
        {
            get { return isEdit; }
            set { SetPropertyValue(nameof(IsEdit), ref isEdit, value); }
        }

        private bool isForComponent;
        [ImmediatePostData]
        [NonPersistent]
        public bool IsForComponent
        {
            get { return isForComponent; }
            set { SetPropertyValue(nameof(IsForComponent), ref isForComponent, value); }
        }

        private bool isNewMode = false;
        private ItemMode itemMode;

        [ImmediatePostData]                // đổi là cập nhật UI ngay
        public ItemMode ItemMode
        {
            get => itemMode;
            set
            {
                if (SetPropertyValue(nameof(ItemMode), ref itemMode, value))
                {
                    OnItemModeChanged();
                }
            }
        }
        // Action được gọi sau khi ItemMode thay đổi
        private void OnItemModeChanged()
        {
            // Chỉ thực hiện logic này khi đối tượng đang được người dùng tương tác
            // (không phải lúc đang tải hoặc lưu)
            if (IsLoading || IsSaving) return;

            // Dọn dẹp giá trị cũ nếu cần (tùy chọn)
            // CategoryID = null;
            // CNTLACCTID = null;
            // UnitID = null;

            if (ItemMode == ItemMode.ForComponent)
            {
                // Cập nhật giá trị cho mode "For Component"
                CategoryID = Session.FindObject<ItemCategory>(CriteriaOperator.Parse("CategoryCode = ?", "XVLTP"));
                CNTLACCTID = Session.FindObject<CNTLACCT>(CriteriaOperator.Parse("CNTLACCTNO = ?", "NVLTP"));
                UnitID = Session.FindObject<Units>(CriteriaOperator.Parse("UnitName = ?", "PCS"));
                isForComponent = true;
            }
            else // ItemMode.ForItem
            {
                // Cập nhật giá trị cho mode "For Item"
                CategoryID = Session.FindObject<ItemCategory>(CriteriaOperator.Parse("CategoryCode = ?", "BTPXK"));
                CNTLACCTID = Session.FindObject<CNTLACCT>(CriteriaOperator.Parse("CNTLACCTNO = ?", "TP"));
                UnitID = Session.FindObject<Units>(CriteriaOperator.Parse("UnitName = ?", "EACH"));
                isForComponent = false;
            }
        }
        private string strResult = "";
        private ComponentCategory componentCategoryID;

        [Association("Item-ComponentCategory", typeof(ComponentCategory))]
        [ImmediatePostData]
        public ComponentCategory ComponentCategoryID
        {
            get { return componentCategoryID; }
            set
            {
                if (SetPropertyValue(nameof(ComponentCategoryID), ref componentCategoryID, value))
                {
                    if (!IsLoading && !IsSaving && !IsDeleted)
                    {
                        if (ComponentCategoryID != null)
                        {
                            DBAccess bs = new DBAccess();
                            strResult = bs.GetMaxComponentCode(ComponentCategoryID.ShortName);
                            SetPropertyValue(nameof(ItemCode), ref _itemCode, strResult);
                            OnChanged();
                        }
                    }

                }
            }
        }
        

        private string oldItemCode;

        private String _itemCode;

        [Size(100)]
        [Indexed]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [ImmediatePostData]
        public String ItemCode
        {
            get
            {

                return _itemCode;
            }
            set
            {
                SetPropertyValue(nameof(ItemCode), ref _itemCode, value);
            }
        }

        private ItemClass itemClassID;

        [Association("Item-ItemClass", typeof(ItemClass)), ImmediatePostData]

        public ItemClass ItemClassID
        {
            get => itemClassID;
            set
            {
                if (SetPropertyValue(nameof(ItemClassID), ref itemClassID, value))
                {
                    if (!IsLoading && !IsSaving && !IsDeleted)
                    {
                        if (itemClassID != null && BrandListID != null)
                        {
                            DBAccess _db = new DBAccess();
                            strResult = _db.GetMaxItemCode(ItemClassID.Oid.ToString(), BrandListID.Oid.ToString());
                            SetPropertyValue(nameof(ItemCode), ref _itemCode, strResult);
                            OnChanged();
                        }
                    }
                }

            }

        }

        private BrandList brandListID;

        [Association("Item-BrandList", typeof(BrandList)), ImmediatePostData]


        public BrandList BrandListID
        {
            get { return brandListID; }
            set { brandListID = value; }
        }

        public String CustSKU { get; set; }

        [Size(250)]
        public String SKUName { get; set; }

        private String _description;
        [Size(300)]
        [ModelDefault("RowCount", "5")]
        public String Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        private Units unitID;
        [Association("Units-Item", typeof(Item))]
        public Units UnitID
        {
            get => unitID;
            set => SetPropertyValue(nameof(UnitID), ref unitID, value);
        }

        private Double w_mm;
        public Double W_mm
        {
            get => w_mm;
            set => SetPropertyValue(nameof(W_mm), ref w_mm, value);
        }

        private Double h_mm;
        public Double H_mm
        {
            get => h_mm;
            set => SetPropertyValue(nameof(H_mm), ref h_mm, value);
        }


        private Double d_mm;
        public Double D_mm
        {
            get => d_mm;
            set => SetPropertyValue(nameof(D_mm), ref d_mm, value);
        }

        private String inch;
        public String Inch
        {
            get
            {
                if (W_mm > 0 & H_mm > 0 & D_mm > 0)
                {
                    inch = $"{Math.Round(W_mm / 25.4, 2)} x {Math.Round(H_mm / 25.4, 2)} x {Math.Round(D_mm / 25.4, 2)}";
                    return inch;
                }
                return inch;
            }
            set => SetPropertyValue(nameof(Inch), ref inch, value);
        }

        private ITEMBRK _ITEMBRKID;


        [Association("ITEMBRK-Item", typeof(Item))]
        public ITEMBRK ITEMBRKID
        {
            get => _ITEMBRKID;
            set => SetPropertyValue(nameof(ITEMBRKID), ref _ITEMBRKID, value);
        }

        // Sửa lại thuộc tính CategoryID và CNTLACCTID, loại bỏ logic cũ
        private ItemCategory categoryID;
        [Association("ItemCategory-Item", typeof(Item))]
        public ItemCategory CategoryID
        {
            get => categoryID;
            set => SetPropertyValue(nameof(CategoryID), ref categoryID, value);
        }

        private CNTLACCT _CNTLACCT;
        [Association("CNTLACCT-Item", typeof(Item))]
        public CNTLACCT CNTLACCTID
        {
            get => _CNTLACCT;
            set => SetPropertyValue(nameof(CNTLACCTID), ref _CNTLACCT, value);
        }

        private DEFPRICLST _DEFPRICLST;
        [Association("DEFPRICLST-Item", typeof(Item))]
        public DEFPRICLST DEFPRICLSTID
        {
            get => _DEFPRICLST;
            set => SetPropertyValue(nameof(DEFPRICLST), ref _DEFPRICLST, value);
        }

        public String AdditionalItemInfo1 { get; set; }

        [ValueConverter(typeof(DevExpress.Xpo.Metadata.ImageValueConverter)), Delayed]
        [ImageEditor(
            ListViewImageEditorMode = ImageEditorMode.PictureEdit, 
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit, 
            ListViewImageEditorCustomHeight = 40)]
        [VisibleInListView(true)]
        [ModelDefault("Index", "1")]
        //[Delayed]  
        public Image IImage
        {
            get { return GetDelayedPropertyValue<System.Drawing.Image>("IImage"); }
            set { SetDelayedPropertyValue<System.Drawing.Image>("IImage", value); }

        }

        public bool DisContinue { get; set; }
        //public bool StockItem { get; set; }
        //public bool Saleable { get; set; }


        DateTime lastMaintanted;
        public DateTime LastMaintanted
        {
            get => lastMaintanted;
            set => SetPropertyValue(nameof(LastMaintanted), ref lastMaintanted, value);
        }



        private FinishList thFinishCodeVersion;
        public FinishList THFinishCodeVersion

        {
            get { return thFinishCodeVersion; }
            set { thFinishCodeVersion = value; }
        }

        private String thCompositeFinishName;

        [Persistent]
        public String THCompositeFinishName

        {
            get
            {
                if (THFinishCodeVersion != null)
                    thCompositeFinishName = THFinishCodeVersion.FinishName;

                return thCompositeFinishName;
            }

        }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get => _createDate;
            set => SetPropertyValue(nameof(CreateDate), ref _createDate, value);
        }
        [XafDisplayName("BOM")]
        [Association("Item-BillsOfMaterial", typeof(BillsOfMaterial))]
        public XPCollection BillsOfMaterials
        {
            get
            {
                return GetCollection("BillsOfMaterials");
            }
        }
        [XafDisplayName("BOM Detail")]
        [Association("Item-BillsOfMaterialDetail", typeof(BillsOfMaterialDetail))]
        public XPCollection BillsOfMaterialDetails
        {
            get
            {
                return GetCollection("BillsOfMaterialDetails");
            }
        }

        [Association("Item-BOMPacking", typeof(BOMPacking))]
        public XPCollection BOMPackings
        {
            get
            {
                return GetCollection("BOMPackings");
            }
        }

        [Association("Item-BOMPackingDetail", typeof(BOMPackingDetail))]
        public XPCollection BOMPackingDetails
        {
            get
            {
                return GetCollection("BOMPackingDetails");
            }
        }

        [Association("Item-ItemPack", typeof(ItemPack))]
        public XPCollection ItemPacks
        {
            get
            {
                return GetCollection("ItemPacks");
            }
        }

        [Association("PODetail-Item", typeof(PODetail))]
        public XPCollection PODetails
        {
            get
            {
                return GetCollection("PODetails");
            }
        }

        [Association("Tag-Item", typeof(Tag))]
        public XPCollection Tags
        {
            get
            {
                return GetCollection("Tags");
            }
        }

        [Association("ProformaInvoiceDetail-Item", typeof(ProformaInvoiceDetail))]
        public XPCollection ProformaInvoiceDetails
        {
            get
            {
                return GetCollection("ProformaInvoiceDetails");
            }
        }

        [Association("ItemFinishList-Item", typeof(ItemFinishList))]
        public XPCollection ItemFinishLists
        {
            get
            {
                return GetCollection("ItemFinishLists");
            }
        }

        //update by tuanpv 

        [Association("CommercialInvoiceDetail-Item", typeof(CommercialInvoiceDetail))]
        public XPCollection<CommercialInvoiceDetail> CommercialInvoiceDetails
        {
            get { return GetCollection<CommercialInvoiceDetail>(nameof(CommercialInvoiceDetails)); }
        }

        //thêm vào ItemBOM.cs chỗ function Item

        [Association("PackingListDetail-Item", typeof(PackingListDetail))]
        public XPCollection<PackingListDetail> PackingListDetails
        {
            get { return GetCollection<PackingListDetail>(nameof(PackingListDetails)); }
        }

        //[Association("PackingListDetail-Item", typeof(PackingListDetail))]
        //public XPCollection<PackingListDetail> PackingListDetails
        //{
        //    get { return GetCollection<PackingListDetail>(nameof(PackingListDetails)); }
        //}

        //end update

        [Association("Item-DetailImages", typeof(DetailImages))]
        public XPCollection DetailImagess
        {
            get
            {
                return GetCollection("DetailImagess");
            }
        }

        [Association("Item-ComponentItem", typeof(ComponentItem))]
        public XPCollection ComponentItems
        {
            get
            {
                return GetCollection("ComponentItems");
            }
        }

        [Association("ShipmentDetail-Item", typeof(ShipmentDetail))]
        public XPCollection ShipmentDetails
        {
            get
            {
                return GetCollection("ShipmentDetails");
            }
        }

        [Association("Item-ItemPriceHistory", typeof(ItemPriceHistory))]
        public XPCollection ItemPriceHistories
        {
            get
            {
                return GetCollection("ItemPriceHistories");
            }
        }
        public string Name => ItemCode;
        public ITreeNode Parent => null;

        //public IBindingList Children => BillsOfMaterials;
        [Browsable(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public IBindingList Children
        {
            get
            {
                var combinedList = new BindingList<object>();
                foreach (var bom in BillsOfMaterials)
                    combinedList.Add(bom);
                foreach (var packing in BOMPackings)
                    combinedList.Add(packing);
                return combinedList;
            }
        }

        [Appearance("HideBrandAndClass_WhenComponent", TargetItems = "BrandListID;ItemClassID", Criteria = "ItemMode = ##Enum#THOR_V20.Module.ItemMode,ForComponent#", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
        public void _dummy() { /* chỉ để mang attribute, không gọi ở runtime */ }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            oldItemCode = _itemCode; // lưu lại giá trị gốc từ DB
            isNewMode = false;
            isEdit = true;
        }
        
        //public IBindingList ChildrenPack => BOMPackings;

        protected override void OnSaving()
        {
            lastMaintanted = DateTime.Now;

            if (ItemMode == ItemMode.ForItem)
            {
                //if (string.IsNullOrEmpty(ItemCode) && Session.Connection != null && ItemClassID != null && BrandListID != null)
                //{
                //    DBAccess _db = new DBAccess();
                //    _itemCode = _db.GetMaxItemCode(ItemClassID.Oid.ToString(), BrandListID.Oid.ToString());

                //}
                //else
                //    _itemCode = _itemCode.Trim();

                if (!String.IsNullOrEmpty(oldItemCode) && oldItemCode != _itemCode && CNTLACCTID.CNTLACCTNO == "TP")
                {
                    //System.Windows.Forms.MessageBox.Show($@"Bạn vừa thay đổi ItemCode {oldItemCode} thành {_itemCode}, hay vui long kiểm tra lại BOM");
                    DBAccess _db = new DBAccess();
                    int nRst = _db.UpdateCategoryBOM(oldItemCode, _itemCode);
                    //_db.SendMailChangeItemCode(oldItemCode, _itemCode);
                }
            }


            isNewMode = false;
            base.OnSaving();

            //Name = ItemCode;
        }

        public override void AfterConstruction()
        {

            base.AfterConstruction();
            IsForComponent = false;
            isNewMode = true;
            isEdit = false;
            // Đặt giá trị mặc định cho ItemMode tại đây
            ItemMode = ItemMode.ForItem;
            CategoryID = Session.FindObject<ItemCategory>(CriteriaOperator.Parse("CategoryCode = ?", "BTPXK"));
            CNTLACCTID = Session.FindObject<CNTLACCT>(CriteriaOperator.Parse("CNTLACCTNO = ?", "TP"));
            UnitID = Session.FindObject<Units>(CriteriaOperator.Parse("UnitName = ?", "EACH"));

            if (_createDate == DateTime.MinValue)
                _createDate = DateTime.Now;


            // Các giá trị mặc định khác không phụ thuộc vào ItemMode
            ITEMBRKID = Session.FindObject<ITEMBRK>(CriteriaOperator.Parse("ITEMBRKID = ?", "TLTH"));
            DEFPRICLSTID = Session.FindObject<DEFPRICLST>(CriteriaOperator.Parse("DEFPRICLSTNO = ?", "TLTHPL"));

            if (!IsLoading && !IsSaving)
            {
                CategoryID = Session.FindObject<ItemCategory>(CriteriaOperator.Parse("CategoryCode = ?", "BTPXK"));
                ITEMBRKID = Session.FindObject<ITEMBRK>(CriteriaOperator.Parse("ITEMBRKID = ?", "TLTH"));
                DEFPRICLSTID = Session.FindObject<DEFPRICLST>(CriteriaOperator.Parse("DEFPRICLSTNO = ?", "TLTHPL"));
                CNTLACCTID = Session.FindObject<CNTLACCT>(CriteriaOperator.Parse("CNTLACCTNO = ?", "TP"));
                UnitID = Session.FindObject<Units>(CriteriaOperator.Parse("UnitName = ?", "EACH"));
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

                            throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");

                        }
                    }
                }
            }
            base.OnDeleting();
        }

        private ItemStock _itemStock;
        [VisibleInDetailView(false)] // Ẩn trong DetailView
        [VisibleInListView(false)]   // Ẩn trong ListView
        [VisibleInLookupListView(false)] // Ẩn trong lookup
        public ItemStock ItemStockID
        {
            get => _itemStock;
            set
            {
                if (_itemStock == value)
                    return;

                var prevStock = _itemStock;
                _itemStock = value;

                if (IsLoading)
                    return;

                // Hủy liên kết cũ
                if (prevStock != null && prevStock.ItemID == this)
                    prevStock.ItemID = null;

                // Gán liên kết mới
                if (_itemStock != null)
                    _itemStock.ItemID = this;

                OnChanged(nameof(ItemStockID));
            }
        }
    }

    [System.ComponentModel.DefaultProperty("ShortName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class ComponentCategory : BaseObject
    {
        private String description;
        private String shortName;

        public ComponentCategory(Session session)
            : base(session)
        {
        }

        [Size(250)]
        public String Description
        {
            get { return description; }
            set { description = value; }
        }
        [Size(20)]
        [RuleRequiredField("Component Category Short Name is required", DefaultContexts.Save, "Short Name is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }

        [Association("Item-ComponentCategory", typeof(Item))]
        public XPCollection<Item> Items
        {
            get
            { return GetCollection<Item>("Items"); }
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

    [DefaultProperty(nameof(VersionName))]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class BOMVersion : BaseObject
    {
        public BOMVersion(Session session) : base(session)
        {

        }

        string _versionName;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string VersionName
        {
            get => _versionName;
            set => SetPropertyValue(nameof(VersionName), ref _versionName, value);
        }

        [Association("BOMVersion-BillsOfMaterial", typeof(BillsOfMaterial))]
        public XPCollection BillsOfMaterials
        {
            get
            {
                return GetCollection("BillsOfMaterials");
            }
        }

        [Association("BOMVersion-BOMPacking", typeof(BOMPacking))]
        public XPCollection BOMPackings
        {
            get
            {
                return GetCollection("BOMPackings");
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

        }
    }

    [DefaultProperty(nameof(ItemID))]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class BillsOfMaterial : Category
    {
        public BillsOfMaterial(Session session) : base(session)
        {

        }


        Item _item;
        [Association("Item-BillsOfMaterial", typeof(BillsOfMaterial))]
        public Item ItemID
        {
            get => _item;
            set => SetPropertyValue(nameof(ItemID), ref _item, value);
        }

        BOMVersion _version;
        [Association("BOMVersion-BillsOfMaterial", typeof(BillsOfMaterial))]
        public BOMVersion VersionID
        {
            get => _version;
            set => SetPropertyValue(nameof(VersionID), ref _version, value);
        }

        public String Description { get; set; }

        public Double FixedCost { get; set; }

        public Double VariableCost { get; set; }

        public int BuildQuantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public String Comments { get; set; }

        private bool inActive;
        public bool InActive
        {
            get => inActive;
            set => SetPropertyValue(nameof(InActive), ref inActive, value);
        }


        [Association("BillsOfMaterial-BillsOfMaterialDetail", typeof(BillsOfMaterialDetail)), DevExpress.Xpo.Aggregated, VisibleInListView(false)]
        //[Association("BillsOfMaterial-BillsOfMaterialDetails"), DevExpress.Xpo.Aggregated]
        public XPCollection<BillsOfMaterialDetail> BillsOfMaterialDetails
        {
            get
            {
                return GetCollection<BillsOfMaterialDetail>(nameof(BillsOfMaterialDetails));
            }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            inActive = true;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            Name = $"{ItemID?.ItemCode.Trim()}-{VersionID?.VersionName.Trim()}";
        }

        protected override ITreeNode Parent => null;
        protected override IBindingList Children => new BindingList<Item>(BillsOfMaterialDetails.Select(x => x.ItemCode).ToList());

    }

    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class BillsOfMaterialDetail : BaseObject /*--, ICategorizedItem*/
    {
        /// <summary>
        /// <para>Used to initialize a new instance of a <see cref="BillsOfMaterial"/> descendant, in a particular <see cref="Session"/>.</para>
        /// </summary>
        /// <param name="session">A DevExpress.Xpo.Session object which represents a persistent object’s cache where the business object will be instantiated.</param>
        public BillsOfMaterialDetail(Session session) : base(session)
        {

        }

        /// <summary>
        /// <para>Creates a new instance of the <see cref="BillsOfMaterial"/> class.</para>
        /// </summary>
        public BillsOfMaterialDetail()
        {

        }


        BillsOfMaterial _bOM;
        //[XafDisplayName("")]
        //[ToolTip("")]
        [Association("BillsOfMaterial-BillsOfMaterialDetail", typeof(BillsOfMaterialDetail))]
        public BillsOfMaterial BillsOfMaterialID
        {
            get => _bOM;
            set => SetPropertyValue(nameof(BillsOfMaterialID), ref _bOM, value);
        }

        private Operation operationID;
        public Operation OperationID
        {
            get => operationID;
            set => SetPropertyValue(nameof(OperationID), ref operationID, value);
        }

        Item _item;

        //[XafDisplayName("")]
        //[ToolTip("")]
        [Association("Item-BillsOfMaterialDetail", typeof(BillsOfMaterialDetail))]
        public Item ItemCode
        {
            get => _item;
            set => SetPropertyValue(nameof(Item), ref _item, value);
        }

        public int LineNo { get; set; }

        Double _quantity;
        //[XafDisplayName("")]
        //[ToolTip("")]
        public Double Quantity
        {
            get => _quantity;
            set => SetPropertyValue(nameof(Quantity), ref _quantity, value);
        }

        private Units unitID;
        public Units UnitID
        {
            get => unitID;
            set => SetPropertyValue(nameof(UnitID), ref unitID, value);
        }

        Double _conversion;
        //[XafDisplayName("")]
        //[ToolTip("")]
        public Double Conversion
        {
            get => _conversion;
            set => SetPropertyValue(nameof(Conversion), ref _conversion, value);
        }

        Double _factor;
        //[XafDisplayName("")]
        //[ToolTip("")]
        public Double Factor
        {
            get => _factor;
            set => SetPropertyValue(nameof(Factor), ref _factor, value);
        }

        string _comment;
        //[XafDisplayName("")]
        //[ToolTip("")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Comment
        {
            get => _comment;
            set => SetPropertyValue(nameof(Comment), ref _comment, value);
        }

        private bool inactive;
        public bool Inactive
        {
            get => inactive;
            set => SetPropertyValue(nameof(Inactive), ref inactive, value);
        }

        public bool IsMainComp { get; set; }

        private Double waste;
        public Double Waste
        {
            get => waste;
            set => SetPropertyValue(nameof(Waste), ref waste, value);
        }

        private Double total;
        public Double Total
        {
            get => total;
            set => SetPropertyValue(nameof(Waste), ref total, value);
        }


        // Triển khai thuộc tính Category
        public Category Category
        {
            get => BillsOfMaterialID;
            set => BillsOfMaterialID = value as BillsOfMaterial;
        }

        // ICategorizedItem Implementation
        //ITreeNode ICategorizedItem.Category
        //{
        //    get => BillsOfMaterialID;
        //    set => BillsOfMaterialID = value as BillsOfMaterial;
        //}

        public override void AfterConstruction()
        {
            inactive = true;
            _quantity = 1;
            base.AfterConstruction();
        }

    }

    [DefaultProperty(nameof(ItemID))]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class BOMPacking : Category
    {
        public BOMPacking(Session session) : base(session)
        {

        }


        Item _item;
        [Association("Item-BOMPacking", typeof(BOMPacking))]
        public Item ItemID
        {
            get => _item;
            set => SetPropertyValue(nameof(ItemID), ref _item, value);
        }

        BOMVersion _version;
        [Association("BOMVersion-BOMPacking", typeof(BOMPacking))]
        public BOMVersion VersionID
        {
            get => _version;
            set => SetPropertyValue(nameof(VersionID), ref _version, value);
        }

        public String Description { get; set; }

        public Double FixedCost { get; set; }

        public Double VariableCost { get; set; }

        public int BuildQuantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public String Comments { get; set; }

        public bool InActive { get; set; }


        [Association("BOMPacking-BOMPackingDetail", typeof(BOMPackingDetail)), DevExpress.Xpo.Aggregated]
        public XPCollection<BOMPackingDetail> BOMPackingDetails
        {
            get
            {
                return GetCollection<BOMPackingDetail>(nameof(BOMPackingDetails));
            }
        }


        protected override void OnSaving()
        {
            base.OnSaving();
            Name = $"{ItemID?.ItemCode.Trim()}-{VersionID?.VersionName.Trim()}";
        }

        protected override ITreeNode Parent => null;
        protected override IBindingList Children => new BindingList<Item>(BOMPackingDetails.Select(x => x.ComponentID).ToList());

    }


    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class BOMPackingDetail : BaseObject
    {
        /// <summary>
        /// <para>Used to initialize a new instance of a <see cref="BillsOfMaterial"/> descendant, in a particular <see cref="Session"/>.</para>
        /// </summary>
        /// <param name="session">A DevExpress.Xpo.Session object which represents a persistent object’s cache where the business object will be instantiated.</param>
        public BOMPackingDetail(Session session) : base(session)
        {

        }

        /// <summary>
        /// <para>Creates a new instance of the <see cref="BillsOfMaterial"/> class.</para>
        /// </summary>
        public BOMPackingDetail()
        {

        }


        BOMPacking _bOM;
        //[XafDisplayName("")]
        //[ToolTip("")]
        [Association("BOMPacking-BOMPackingDetail", typeof(BOMPackingDetail))]
        public BOMPacking BOMPackingID
        {
            get => _bOM;
            set => SetPropertyValue(nameof(BOMPackingID), ref _bOM, value);
        }
        private Operation operationID;
        public Operation OperationID
        {
            get => operationID;
            set => SetPropertyValue(nameof(OperationID), ref operationID, value);
        }

        Item _item;

        //[XafDisplayName("")]
        //[ToolTip("")]
        [Association("Item-BOMPackingDetail", typeof(BOMPackingDetail))]
        public Item ComponentID
        {
            get => _item;
            set => SetPropertyValue(nameof(Item), ref _item, value);
        }

        public int LineNo { get; set; }

        Double _quantity;
        //[XafDisplayName("")]
        //[ToolTip("")]
        public Double Quantity
        {
            get => _quantity;
            set => SetPropertyValue(nameof(Quantity), ref _quantity, value);
        }

        private Units unitID;
        public Units UnitID
        {
            get => unitID;
            set => SetPropertyValue(nameof(UnitID), ref unitID, value);
        }

        Double _conversion;
        //[XafDisplayName("")]
        //[ToolTip("")]
        public Double Conversion
        {
            get => _conversion;
            set => SetPropertyValue(nameof(Conversion), ref _conversion, value);
        }

        Double _factor;
        //[XafDisplayName("")]
        //[ToolTip("")]
        public Double Factor
        {
            get => _factor;
            set => SetPropertyValue(nameof(Factor), ref _factor, value);
        }

        string _comment;
        //[XafDisplayName("")]
        //[ToolTip("")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Comment
        {
            get => _comment;
            set => SetPropertyValue(nameof(Comment), ref _comment, value);
        }

        private bool inactive;
        public bool Inactive
        {
            get => inactive;
            set => SetPropertyValue(nameof(Inactive), ref inactive, value);
        }

        public bool IsMainComp { get; set; }

        private Double waste;
        public Double Waste
        {
            get => waste;
            set => SetPropertyValue(nameof(Waste), ref waste, value);
        }

        private Double total;
        public Double Total
        {
            get => total;
            set => SetPropertyValue(nameof(Waste), ref total, value);
        }


        // Triển khai thuộc tính Category
        public Category Category
        {
            get => BOMPackingID;
            set => BOMPackingID = value as BOMPacking;
        }

        // ICategorizedItem Implementation
        //ITreeNode ICategorizedItem.Category
        //{
        //    get => BOMPackingID;
        //    set => BOMPackingID = value as BOMPacking;
        //}

        public override void AfterConstruction()
        {
            inactive = true;
            _quantity = 1;
            base.AfterConstruction();
        }

    }

    [RuleCriteria("DetailImages Image very big. It is must <= 200Kb", DefaultContexts.Save, "ReturnSize < 204800", "DetailImages Image very big. It is must <= 200Kb", SkipNullOrEmptyValues = false)]
    [System.ComponentModel.DefaultProperty("ItemID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class DetailImages : BaseObject, IPictureItem
    {

        private Item itemID;
        private DateTime createDate;
        public DetailImages(Session session) : base(session) { }
        //private DateTime createDate;


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            createDate = DateTime.Now;
            dateCreatedImage = DateTime.Now;
        }

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        [ValueConverter(typeof(DevExpress.Xpo.Metadata.ImageValueConverter)), Delayed]
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, DetailViewImageEditorMode = ImageEditorMode.PictureEdit, ListViewImageEditorCustomHeight = 40)]
        //[Delayed]  
        public Image DetailImage
        {
            get { return GetDelayedPropertyValue<Image>("DetailImage"); }
            set { SetDelayedPropertyValue<Image>("DetailImage", value); }

        }
        private long dReturnSize;
        [NonPersistent]
        public long ReturnSize
        {
            get
            {
                if (DetailImage != null)
                {
                    Bitmap bm = new Bitmap(DetailImage);
                    MemoryStream ms = new MemoryStream();
                    bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageByte = ms.ToArray();

                    dReturnSize = imageByte.LongLength;
                }

                return dReturnSize;
            }

        }

        [Association("Item-DetailImages", typeof(Item))]
        public Item ItemID
        {
            get
            {
                return itemID;
            }
            set
            {
                if (itemID == value)
                    return;
                itemID = value;
            }
        }
        Image IPictureItem.Image
        {
            get { return DetailImage; }
        }

        string IPictureItem.Text
        {
            get { return String.Format("{0}", ""); }
        }

        string IPictureItem.ID
        {
            get { return Oid.ToString(); }
        }
        private DateTime dateCreatedImage;

        [RuleRequiredField("DetailImage DateCreatedImage is required", DefaultContexts.Save, "DetailImage DateCreatedImage is required")]
        public DateTime DateCreatedImage
        {
            get { return dateCreatedImage; }
            set { dateCreatedImage = value; }
        }
    }

    [System.ComponentModel.DefaultProperty("ItemID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class ComponentItem : BaseObject
    {
        private Item itemID;
        private Item componentID;
        private PO poID;

        public ComponentItem(Session session)
            : base(session)
        {
        }

        [Association("Item-ComponentItem", typeof(Item))]
        public Item ComponentID
        {
            get
            {
                return itemID;
            }
            set
            {
                if (itemID == value)
                    return;
                itemID = value;
            }
        }
        public Item ItemID
        {
            get
            {
                return componentID;
            }
            set
            {
                if (componentID == value)
                    return;
                componentID = value;
            }
        }

        public PO POID
        {
            get
            {
                return poID;
            }
            set
            {
                if (poID == value)
                    return;
                poID = value;
            }
        }

        public PODetail PODetailID { get; set; }

        public DateTime FactoryLoadingDate { get; set; }

        public Double QtyBOM { get; set; }
        public Double QtyPO { get; set; }

        public Double QtyIssue { get; set; }

        public Double StockQty { get; set; }

        public Double QtyCompTotal { get; set; }

        public Operation OperationID { get; set; }

    }

    //update by tuanpv 18/9/2025
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class SampleMonitoring : BaseObject
    {
        public SampleMonitoring(Session session) : base(session) { }

        // đóng tạm
        //public Customer CustomerID { get; set; }

        public String CustName { get; set; }

        public DateTime RequestDate { get; set; }
        public String CustSKU { get; set; }

        public Item ItemID { get; set; }

        public String Description { get; set; }
        public String Dimension { get; set; }

        [ValueConverter(typeof(DevExpress.Xpo.Metadata.ImageValueConverter)), Delayed]
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, DetailViewImageEditorMode = ImageEditorMode.PictureEdit, ListViewImageEditorCustomHeight = 40)]
        //[Delayed]  
        public Image IImage
        {
            get { return GetDelayedPropertyValue<System.Drawing.Image>("IImage"); }
            set { SetDelayedPropertyValue<System.Drawing.Image>("IImage", value); }

        }

        public String CustRemark { get; set; }
        public DateTime AcceptedDate { get; set; }
        public DateTime DrawingApprovedDate { get; set; }
        public DateTime SampleApprovedDate { get; set; }
        public String THRemarks { get; set; }
        public DateTime QuotationDate { get; set; }
    }
    public enum ItemMode
    {
        ForItem = 0,
        ForComponent = 1
    }
}