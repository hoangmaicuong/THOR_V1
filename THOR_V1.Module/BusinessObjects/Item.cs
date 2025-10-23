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
using System.Security.Policy;
using System.Text;

namespace THOR_V1.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem(GroupName = "Master Lists")]
    [ImageName("BO_Product")]
    //[NavigationItem(false)] // 👈 Không tạo mục menu
    [DefaultProperty(nameof(ItemCode))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class Item : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Item(Session session)
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
        private string _description;
        [Size(300)]
        [XafDisplayName("Description")]
        [VisibleInListView(true)]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
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
}