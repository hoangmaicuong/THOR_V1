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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace THOR_V1.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem(GroupName = "Master Lists")]
    [ImageName("BO_Product")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class ItemStock : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ItemStock(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://docs.devexpress.com/eXpressAppFramework/112619/ui-construction/controllers-and-actions/actions/how-to-create-an-action-using-the-action-attribute).
        //    this.PersistentProperty = "Paid";
        //}

        private Item _item;
        [XafDisplayName("Item Code")]
        [ModelDefault("Index", "0")]
        [ImmediatePostData] // để cập nhật thuộc tính phụ khi chọn Item
        public Item ItemID
        {
            get => _item;
            set
            {
                if (_item == value)
                    return;

                var prevItem = _item;
                _item = value;

                if (IsLoading)
                    return;

                // Hủy liên kết cũ
                if (prevItem != null && prevItem.ItemStockID == this)
                    prevItem.ItemStockID = null;

                // Gán liên kết mới
                if (_item != null)
                    _item.ItemStockID = this;

                OnChanged(nameof(ItemID));
            }
        }
        [ModelDefault("Index", "1")]
        [XafDisplayName("Description")]
        public string ItemDescription => ItemID?.Description;

        private int _BOMQty;
        public int BOMQty
        {
            get => _BOMQty;
            set => SetPropertyValue(nameof(BOMQty), ref _BOMQty, value);
        }

        private DateTime? _purDate;
        public DateTime? PurDate
        {
            get => _purDate;
            set => SetPropertyValue(nameof(PurDate), ref _purDate, value);
        }

        private string _suplier;
        public string Suplier
        {
            get => _suplier;
            set => SetPropertyValue(nameof(Suplier), ref _suplier, value);
        }

        private int _stockQty;
        public int StockQty
        {
            get => _stockQty;
            set => SetPropertyValue(nameof(StockQty), ref _stockQty, value);
        }

        private int _totalReceipt;
        public int TotalReceipt
        {
            get => _totalReceipt;
            set => SetPropertyValue(nameof(TotalReceipt), ref _totalReceipt, value);
        }

        private int _totalIssue;
        public int TotalIssue
        {
            get => _totalIssue;
            set => SetPropertyValue(nameof(TotalIssue), ref _totalIssue, value);
        }

        private int _priceAVG;
        public int PriceAVG
        {
            get => _priceAVG;
            set => SetPropertyValue(nameof(PriceAVG), ref _priceAVG, value);
        }

        // ✅ Quan hệ 1-n: Một ItemStock có nhiều PurchasingHistory
        [DevExpress.Xpo.Association("ItemStock-PurchasingHistories")]
        public XPCollection<PurchasingHistory> PurchasingHistories
        {
            get { return GetCollection<PurchasingHistory>(nameof(PurchasingHistories)); }
        }
    }
}