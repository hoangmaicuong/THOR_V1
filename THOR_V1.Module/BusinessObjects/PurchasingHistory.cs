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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace THOR_V1.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem(false)] // 👈 Không tạo mục menu
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class PurchasingHistory : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PurchasingHistory(Session session)
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
        
        private ItemStock _itemStock;

        // ✅ Quan hệ ngược lại: Mỗi PurchasingHistory thuộc về một ItemStock
        [DevExpress.Xpo.Association("ItemStock-PurchasingHistories")]
        [VisibleInDetailView(false)] // Ẩn trong DetailView
        [VisibleInListView(false)]   // Ẩn trong ListView
        [VisibleInLookupListView(false)] // Ẩn trong lookup
        public ItemStock ItemStockID
        {
            get => _itemStock;
            set => SetPropertyValue(nameof(ItemStockID), ref _itemStock, value);
        }
        [XafDisplayName("Item Code")]
        [VisibleInDetailView(false)] // Ẩn trong DetailView
        public string ItemCode => ItemStockID?.ItemID?.ItemCode;

        private string _poNum;
        public string PONum
        {
            get => _poNum;
            set => SetPropertyValue(nameof(PONum), ref _poNum, value);
        }

        private DateTime? _purDate;
        public DateTime? PurDate
        {
            get => _purDate;
            set => SetPropertyValue(nameof(PurDate), ref _purDate, value);
        }

        private string _supplier;
        public string Supplier
        {
            get => _supplier;
            set => SetPropertyValue(nameof(Supplier), ref _supplier, value);
        }

        private int _receiptQty;
        public int ReceiptQty
        {
            get => _receiptQty;
            set => SetPropertyValue(nameof(ReceiptQty), ref _receiptQty, value);
        }
    }
}